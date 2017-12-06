using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchController : MonoBehaviour 
{
	protected GridCreator m_gridCreator;

	protected virtual void Start()
	{
		m_gridCreator = FindObjectOfType<GridCreator>();
	}
}
