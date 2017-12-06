using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessPiece : Piece 
{
	public enum PieceColour
	{
		BLACK,
		WHITE
	}
		
	[SerializeField] PieceColour m_colour;

	public enum PieceType
	{
		PAWN,
		KING,
		QUEEN,
		ROOK,
		KNIGHT,
		BISHOP
	}

	[SerializeField] PieceType m_type;

	public PieceColour Colour
	{
		get { return m_colour; }
	}

	public PieceType Type
	{
		get { return m_type; }
	}


	public override bool Equals(Piece p)
	{
		ChessPiece pc = p as ChessPiece;
		if (pc == null)
		{
			return false;
		}

		return ((pc.Colour == m_colour) && (pc.Type == m_type));
	}
}
