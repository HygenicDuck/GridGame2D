using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnOffTouchController : TouchController 
{
	[SerializeField] GameObject m_usedPiece;
	[SerializeField] bool m_deleteAdjacent;


	Vector2 m_touchPosition;
	Vector2 m_touchDownPosition;
	bool m_touching;

	// Use this for initialization
	protected override void Start () 
	{
		base.Start ();
		GameControllerBase.Instance.m_dragAndDropIsActive = false;
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown(0))
		{
			m_touching = true;
			m_touchPosition = Input.mousePosition;
			m_touchDownPosition = Input.mousePosition;

			OnTouchDown (m_touchPosition);
		}
		else if (Input.GetMouseButtonUp(0))
		{
			m_touching = false;
		}
	}

	void OnTouchDown(Vector3 touchPos)
	{
		IntVec2 gridPos = m_gridCreator.GridPosFromTouchPos (touchPos);
		Piece piece = m_gridCreator.GetPieceAt (gridPos.x, gridPos.y);
		if (piece == null)
		{
			m_gridCreator.AttachNewPieceToCell (m_usedPiece, gridPos.x, gridPos.y);
		}
		else
		{
			if (m_deleteAdjacent)
			{
				DeletePieceAndSimilarNeighbours (gridPos);
			}
			else
			{
				Piece deletedPiece = m_gridCreator.RemovePieceFromCell (gridPos.x, gridPos.y);
				Destroy (deletedPiece.gameObject);
			}
		}
	}

	void DeletePieceAndSimilarNeighbours(IntVec2 gridPos)
	{
		// recursive

		Piece piece = m_gridCreator.RemovePieceFromCell (gridPos);

		IntVec2 leftPos = gridPos + new IntVec2 (-1, 0);
		Piece leftPiece = m_gridCreator.GetPieceAt (leftPos);
		if ((leftPiece != null) && (leftPiece.Equals(piece)))
		{
			DeletePieceAndSimilarNeighbours(leftPos);
		}

		IntVec2 rightPos = gridPos + new IntVec2 (1, 0);
		Piece rightPiece = m_gridCreator.GetPieceAt (rightPos);
		if ((rightPiece != null) && (rightPiece.Equals(piece)))
		{
			DeletePieceAndSimilarNeighbours(rightPos);
		}

		IntVec2 upPos = gridPos + new IntVec2 (0, -1);
		Piece upPiece = m_gridCreator.GetPieceAt (upPos);
		if ((upPiece != null) && (upPiece.Equals(piece)))
		{
			DeletePieceAndSimilarNeighbours(upPos);
		}

		IntVec2 downPos = gridPos + new IntVec2 (0, 1);
		Piece downPiece = m_gridCreator.GetPieceAt (downPos);
		if ((downPiece != null) && (downPiece.Equals(piece)))
		{
			DeletePieceAndSimilarNeighbours(downPos);
		}

		Destroy (piece.gameObject);

	}
}
