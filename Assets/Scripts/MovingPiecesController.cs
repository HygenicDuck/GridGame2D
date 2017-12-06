using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingPiecesController : MonoBehaviour 
{
	static MovingPiecesController m_instance = null;


	public static MovingPiecesController Instance
	{
		get
		{
			return m_instance;
		}
	}

	public MovingPiecesController()
	{
		m_instance = this;
	}

	public GameObject CreateMovingPiece(Piece piece)
	{
		GameObject gOb = Instantiate (piece.gameObject, transform);
		return gOb;
	}

}
