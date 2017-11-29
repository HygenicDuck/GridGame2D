using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridCell : MonoBehaviour 
{
	[SerializeField] GameObject m_pieceParent;

	public Piece GetAttachedPiece()
	{
		return m_pieceParent.GetComponentInChildren<Piece> ();
	}

	public Piece RemovePiece()
	{
		Piece piece = null;

		Transform t = m_pieceParent.transform;

		for (int i = t.childCount - 1; i >= 0; --i)
		{
			GameObject child = t.GetChild(i).gameObject;
			child.transform.SetParent(null);
			piece = child.GetComponent<Piece> ();
		}

		return piece;
	}

	public void AttachPiece(Piece piece)
	{
		Transform t = m_pieceParent.transform;
		piece.transform.SetParent (t);
		piece.transform.localScale = Vector2.one;
		piece.transform.localPosition = Vector2.zero;
	}
}
