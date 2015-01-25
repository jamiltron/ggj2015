using UnityEngine;
using System.Collections;

public class InstrumentScale : MonoBehaviour
{
	public Vector3[] Scales;
	public float ScaleTime = 1;
	public AnimationCurve ScaleCurve = AnimationCurve.Linear(0, 0, 1, 1);

	private Vector3 _baseScale;
	private Vector3 _targetScale;
	private float _lastPress = -1000;
  private bool continousLooping = false;

	public void Awake()
	{
		_baseScale = transform.localScale;
		_targetScale = _baseScale;
	}

	public void OnKeyPress(int keyIndex)
	{
		_lastPress = Time.time;
		_targetScale = Scales[keyIndex % Scales.Length];
	}

	public void Update()
	{
		float t = Time.time - _lastPress;
		t = ScaleCurve.Evaluate(t / ScaleTime);
		transform.localScale = _baseScale*(1 - t) + _targetScale*t;
	}

  public void ContinousPress(int keyIndex)
  {
    if (!continousLooping) {
      continousLooping = true;
      ScaleCurve.postWrapMode = WrapMode.Loop;
      OnKeyPress(keyIndex);
    }
  }

  public void Stop()
  {
    _lastPress = -1000;
    transform.localScale = _baseScale;
    continousLooping = false;
    ScaleCurve.postWrapMode = WrapMode.Clamp;
  }

}