using UnityEngine;

public class InstrumentScale : MonoBehaviour
{
	public Vector3[] Scales;
	public float ScaleTime = 1;
	public AnimationCurve ScaleCurve = AnimationCurve.Linear(0, 1, 1, 1);

	private Vector3 _baseScale;
	private Vector3 _targetScale;
	private float _lastPress = -1000;

	public void Awake()
	{
		_baseScale = transform.localScale;
	}

	public void OnKeyPress(int keyIndex)
	{
		_lastPress = Time.time;
		Vector3 s = Scales[keyIndex%Scales.Length];
		_targetScale = new Vector3(_baseScale.x * s.x, _baseScale.y * s.y, _baseScale.z * s.z);
	}

	public void Update()
	{
		float t = Time.time - _lastPress;
		t = ScaleCurve.Evaluate(t / ScaleTime);
		transform.localScale = Vector3.Lerp(_baseScale, _targetScale, t);
	}
}