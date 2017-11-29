using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridInitialiser : MonoBehaviour 
{
	static GridInitialiser m_instance = null;


	public static GridInitialiser Instance
	{
		get
		{
			return m_instance;
		}
	}

	public GridInitialiser()
	{
		m_instance = this;
	}

	public abstract void Initialise();
}
