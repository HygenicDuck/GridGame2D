using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnOffTouchController : TouchController 
{
	[SerializeField] GameObject m_usedPiece;


	Vector2 m_touchPosition;
	Vector2 m_touchDownPosition;
	bool m_touching;

	// Use this for initialization
	void Start () 
	{
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
			if (piece.GetComponent<DragAndDrop> () == null)
			{
				Piece deletedPiece = m_gridCreator.RemovePieceFromCell (gridPos.x, gridPos.y);
				Destroy (deletedPiece.gameObject);
			}
		}
	}
}
