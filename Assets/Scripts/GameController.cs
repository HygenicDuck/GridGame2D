using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
	static GameController m_instance = null;


	public static GameController Instance
	{
		get
		{
			return m_instance;
		}
	}

	public GameController()
	{
		m_instance = this;
	}


}
