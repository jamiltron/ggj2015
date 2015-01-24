using UnityEngine;

public class InstrumentScale : MonoBehaviour
{
	public Vector3 Scale = new Vector3(2, 2, 2);
	public float ScaleTime = 1;
	public AnimationCurve ScaleCurve = AnimationCurve.Linear(0, 1, 1, 1);

	private Vector3 _baseScale;
	private float _lastPress = -1000;

	public void Awake()
	{
		_baseScale = transform.localScale;
	}

	public void OnKeyPress(int keyIndex)
	{
		_lastPress = Time.time;
	}

	public void Update()
	{
		float t = Time.time - _lastPress;
		t = ScaleCurve.Evaluate(t / ScaleTime);
		Vector3 targetScale = new Vector3(_baseScale.x*Scale.x, _baseScale.y*Scale.y, _baseScale.z*Scale.z);
		transform.localScale = Vector3.Lerp(_baseScale, targetScale, t);
	}
}