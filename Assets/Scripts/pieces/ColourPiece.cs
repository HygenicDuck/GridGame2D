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
}
