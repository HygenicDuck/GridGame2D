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

	public void RemovePiece()
	{
		Transform t = m_pieceParent.transform;

		for (int i = t.childCount - 1; i >= 0; --i)
		{
			GameObject child = t.GetChild(i).gameObject;
			Destroy(child);
		}
	}

	public void AttachPiece(GameObject piece)
	{
		Transform t = m_pieceParent.transform;
		piece.transform.SetParent (t);
		piece.transform.localScale = Vector2.one;
		piece.transform.localPosition = Vector2.zero;
	}
}
