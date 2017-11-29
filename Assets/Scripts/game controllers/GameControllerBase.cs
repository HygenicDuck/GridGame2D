using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameControllerBase : MonoBehaviour 
{
	static GameControllerBase m_instance = null;


	public static GameControllerBase Instance
	{
		get
		{
			return m_instance;
		}
	}

	public GameControllerBase()
	{
		m_instance = this;
	}

	public abstract void MovePiece(Piece piece, IntVec2 newPos);
}
