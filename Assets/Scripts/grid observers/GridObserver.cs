using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObserver : MonoBehaviour 
{
	protected GridCreator m_gridCreator;
	protected bool m_doObserving = false;


	protected virtual void Start()
	{
		m_gridCreator = FindObjectOfType<GridCreator>();
		m_doObserving = false;
		StartCoroutine (EnableAfterShortDelay());
	}

	IEnumerator EnableAfterShortDelay()
	{
		yield return null;
		m_doObserving = true;
	}
}
