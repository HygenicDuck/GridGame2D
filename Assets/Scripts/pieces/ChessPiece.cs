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
}
