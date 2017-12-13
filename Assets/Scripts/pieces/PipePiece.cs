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

	public enum Connector
	{
		ANY = 0,
		NONE,
		UP,
		DOWN,
		LEFT,
		RIGHT
	}
		
	[SerializeField] public PieceType m_type;

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

	public bool HasConnector(Connector c)
	{
		switch (c)
		{
		case Connector.ANY:
			return true;
			break;
		case Connector.NONE:
			return false;
			break;
		case Connector.DOWN:
			switch (m_type)
			{
			case PieceType.VERTICAL:
			case PieceType.END_DOWN:
			case PieceType.DOWN_LEFT:
			case PieceType.DOWN_RIGHT:
				return true;
			}
			break;
		case Connector.UP:
			switch (m_type)
			{
			case PieceType.VERTICAL:
			case PieceType.END_UP:
			case PieceType.UP_LEFT:
			case PieceType.UP_RIGHT:
				return true;
			}
			break;
		case Connector.LEFT:
			switch (m_type)
			{
			case PieceType.HORIZONTAL:
			case PieceType.END_LEFT:
			case PieceType.UP_LEFT:
			case PieceType.DOWN_LEFT:
				return true;
			}
			break;
		case Connector.RIGHT:
			switch (m_type)
			{
			case PieceType.HORIZONTAL:
			case PieceType.END_RIGHT:
			case PieceType.UP_RIGHT:
			case PieceType.DOWN_RIGHT:
				return true;
			}
			break;
		}

		return false;
	}

}
