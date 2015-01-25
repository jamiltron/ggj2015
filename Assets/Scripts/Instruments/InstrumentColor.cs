using UnityEngine;

public class InstrumentColor : MonoBehaviour
{
	public Color[] Colors;
	public float ColorTime = 1;
	public AnimationCurve ColorCurve = AnimationCurve.Linear(0, 0, 1, 1);

	private Color _baseColor;
	private Color _targetColor;
	private float _lastPress = -1000;

	public void Awake()
	{
		_baseColor = renderer.material.color;
		_targetColor = _baseColor;
	}

	public void OnKeyPress(int keyIndex)
	{
		_lastPress = Time.time;
		_targetColor = Colors[keyIndex%Colors.Length];
	}

	public void Update()
	{
		float t = Time.time - _lastPress;
		t = t < ColorTime ? ColorCurve.Evaluate(t/ColorTime) : 0;
		renderer.material.color = _baseColor*(1 - t) + _targetColor*t;
	}
}