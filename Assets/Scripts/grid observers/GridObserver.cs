using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObserver : MonoBehaviour 
{
	protected GridCreator m_gridCreator;
	protected bool m_doObserving = false;
	protected float m_pauseTimer = 0f;

	static GridObserver m_instance = null;


	public static GridObserver Instance
	{
		get
		{
			return m_instance;
		}
	}

	public GridObserver()
	{
		m_instance = this;
	}

	protected virtual void Start()
	{
		m_gridCreator = FindObjectOfType<GridCreator>();
		m_doObserving = false;
		StartCoroutine (EnableAfterShortDelay());
	}

	IEnumerator EnableAfterShortDelay()
	{
		yield return null;
		yield return null;
		m_doObserving = true;
	}

	public void PauseObserving(float time)
	{
		m_pauseTimer = time;
	}
}
