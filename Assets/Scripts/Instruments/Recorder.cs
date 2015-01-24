using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour
{
	private bool _isRecording;
	private Coroutine _playbackCoroutine;
	private readonly List<KeyPress> _recordedKeys = new List<KeyPress>();
	private float _recordingStartTime;
	private float _recordingEndTime;

	public void OnGUI()
	{
		if (_isRecording)
		{
			GUIStyle style = new GUIStyle("Label");
			style.clipping = TextClipping.Overflow;
			style.alignment = TextAnchor.MiddleCenter;

			Vector3 guiPos = WorldToGuiPoint(transform.position);
			GUI.color = Color.red;
			GUI.Label(new Rect(guiPos.x, guiPos.y, 0, 0), "REC", style);
			GUI.color = Color.white;
		}
	}

	public void ToggleRecord()
	{
		if (_isRecording)
			StopRecording();
		else
			StartRecording();
	}

	private void StartRecording()
	{
		_isRecording = true;
		_recordingStartTime = Time.time;
		if (_playbackCoroutine != null)
			StopCoroutine(_playbackCoroutine);
		_recordedKeys.Clear();
	}

	private void StopRecording()
	{
		_isRecording = false;
		_recordingEndTime = Time.time;
		_playbackCoroutine = StartCoroutine(PlaybackCoroutine());
	}

	public void OnDirectKeyPress(int keyIndex)
	{
		if (!_isRecording && _playbackCoroutine != null)
			StopCoroutine(_playbackCoroutine);
	}

	public void OnKeyPress(int keyIndex)
	{
		if (!_isRecording)
			return;

		if (_recordedKeys.Count == 0)
			_recordingStartTime = Time.time;
		float time = Time.time - _recordingStartTime;
		_recordedKeys.Add(new KeyPress(keyIndex, time));
	}

	public Vector3 WorldToGuiPoint(Vector3 position)
	{
		var guiPosition = Camera.main.WorldToScreenPoint(position);
		guiPosition.y = Screen.height - guiPosition.y;
		return guiPosition;
	}

	private IEnumerator PlaybackCoroutine()
	{
		while (true)
		{
//			networkView.RPC("SendOnKeyPress", RPCMode.All, 0);
//			yield return new WaitForSeconds(1);

			float loopStartTime = Time.time;
			foreach (KeyPress kp in _recordedKeys)
			{
				float waitUntil = loopStartTime + kp.Time;
				float waitTime = waitUntil - Time.time;
				if (waitTime > 0)
					yield return new WaitForSeconds(waitTime);

				networkView.RPC("SendOnKeyPress", RPCMode.All, kp.KeyIndex);
			}

			// Wait for repeat.
			{
				float loopEndTime = loopStartTime + (_recordingEndTime - _recordingStartTime);
				float waitTime = loopEndTime - Time.time;
				if(waitTime > 0)
					yield return new WaitForSeconds(waitTime);
			}
		}
	}

	private struct KeyPress
	{
		public readonly int KeyIndex;
		public readonly float Time;

		public KeyPress(int keyIndex, float time)
		{
			KeyIndex = keyIndex;
			Time = time;
		}
	}
}