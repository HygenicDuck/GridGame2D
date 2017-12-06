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

	public bool m_dragAndDropIsActive = true;

	public abstract void MovePiece(Piece piece, IntVec2 newPos);

	public void MovePiece(IntVec2 oldpos, IntVec2 newPos)
	{
		Piece piece = GridCreator.Instance.GetPieceAt (oldpos.x, oldpos.y);
		if (piece != null)
		{
			MovePiece (piece, newPos);
		}
	}
}
