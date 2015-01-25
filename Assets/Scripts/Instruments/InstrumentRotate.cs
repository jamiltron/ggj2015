using UnityEngine;

public class InstrumentRotate : MonoBehaviour
{
	public float[] Rotations;
	public float RotTime = 1;
	public AnimationCurve RotCurve = AnimationCurve.Linear(0, 0, 1, 1);

	private Vector3 _baseRot;
	private Vector3 _targetRot;
	private float _lastPress = -1000;

	public void Awake()
	{
		_baseRot = transform.localEulerAngles;
		_targetRot = _baseRot;
	}

	public void OnKeyPress(int keyIndex)
	{
		_lastPress = Time.time;
		_targetRot = new Vector3(0, 0, Rotations[keyIndex%Rotations.Length]);
	}

	public void Update()
	{
		float t = Time.time - _lastPress;
		t = t < RotTime ? RotCurve.Evaluate(t/RotTime) : 0;
		transform.localEulerAngles = _baseRot*(1 - t) + _targetRot*t;
	}
}