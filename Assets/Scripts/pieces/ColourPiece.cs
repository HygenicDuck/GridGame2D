using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColourPiece : Piece 
{
	public enum PieceColour
	{
		RED,
		BLUE,
		YELLOW,
		BLACK,
		WHITE
	}
		
	[SerializeField] PieceColour m_colour;

	public PieceColour Colour
	{
		get { return m_colour; }
	}

	public override bool Equals(Piece p)
	{
		ColourPiece pc = p as ColourPiece;
		if (pc == null)
		{
			return false;
		}

		return (pc.Colour == m_colour);
	}

}
