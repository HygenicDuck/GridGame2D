using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PipePiece : Piece 
{
	public enum PieceType
	{
		HORIZONTAL,
		VERTICAL,
		END_LEFT,
		END_RIGHT,
		END_UP,
		END_DOWN,
		UP_LEFT,
		UP_RIGHT,
		DOWN_LEFT,
		DOWN_RIGHT
	}
		
	[SerializeField] PieceType m_type;

	public PieceType Type
	{
		get { return m_type; }
	}

	public override bool Equals(Piece p)
	{
		PipePiece pc = p as PipePiece;
		if (pc == null)
		{
			return false;
		}

		return (pc.Type == m_type);
	}

}
