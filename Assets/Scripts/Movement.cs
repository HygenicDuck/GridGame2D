using System;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
	private Vector2 startPos;
	private Vector2 destinationPos;
	private float travelTime;
	private float timePassed;
	private bool moving;

	public enum MoveMode
	{
		LINEAR,
		EASEINOUT,
		EASEOUTBOUNCE,
		USECURVE
	}
	private MoveMode m_mode;

	[SerializeField] private AnimationCurve moveCurve;

	// Use this for initialization
	private void Start()
	{

	}

	private void Awake()
	{
		moving = false;
	}

	public void MoveTo (Vector2 p, float time)
	{
		RectTransform rt = gameObject.GetComponent<RectTransform>();
		startPos = rt.localPosition;
		destinationPos = p;
		travelTime = time;
		timePassed = 0.0f;
		moving = true;
		m_mode = MoveMode.EASEINOUT;
	}

	public void ResetPosition()
	{
		// reset the transform to the start position
		RectTransform rt = gameObject.GetComponent<RectTransform>();
		rt.localPosition = startPos;
		moving = false;
	}

	public void MoveBy (Vector2 offset, float time, MoveMode mode = MoveMode.EASEINOUT)
	{
		RectTransform rt = gameObject.GetComponent<RectTransform>();
		startPos = rt.localPosition;
		destinationPos = startPos+offset;
		travelTime = time;
		timePassed = 0.0f;
		moving = true;
		m_mode = mode;
	}

	private void Update()
	{
		if (moving)
		{
			timePassed += Time.deltaTime;
			float t = timePassed / travelTime;
			if (t >= 1) 
			{
				t = 1;
				moving = false;
			}

			Vector2 p;

			switch (m_mode)
			{
			case MoveMode.EASEINOUT:
				t = t*t * (3f - 2f*t);
				p = Vector2.Lerp(startPos,destinationPos,t);
				break;
			case MoveMode.EASEOUTBOUNCE:
				p = EaseOutBounce(t,startPos,destinationPos-startPos,1f);
				break;
			case MoveMode.USECURVE:
				p = EaseUsingCurve(t,startPos,destinationPos-startPos);
				break;
			default:
				p = Vector2.Lerp(startPos,destinationPos,t);
				break;
			}

			RectTransform rt = gameObject.GetComponent<RectTransform>();
			rt.localPosition = new Vector3(p.x, p.y, rt.localPosition.z);
		}
	}

	private Vector2 EaseOutBounce(float t, Vector2 b, Vector2 c, float d)
	{
		if ((t /= d) < (1f / 2.75f)) 
		{
			return c * (7.5625f * t * t) + b;
		}
		else if (t < (2f / 2.75f)) 
		{
			return c * (7.5625f * (t -= (1.5f / 2.75f)) * t + .75f) + b;
		} 
		else if (t < (2.5f / 2.75f)) 
		{
			return c * (7.5625f * (t -= (2.25f / 2.75f)) * t + .9375f) + b;
		} 
		else 
		{
			return c * (7.5625f * (t -= (2.625f / 2.75f)) * t + .984375f) + b;
		}
	}

	private Vector2 EaseUsingCurve(float t, Vector2 b, Vector2 c)
	{
		float p = moveCurve.Evaluate (t);
		return (c * p) + b;
	}

}

