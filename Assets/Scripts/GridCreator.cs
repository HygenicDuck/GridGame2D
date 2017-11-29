using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridCreator : MonoBehaviour {

	static GridCreator m_instance = null;


	public static GridCreator Instance
	{
		get
		{
			return m_instance;
		}
	}

	public GridCreator()
	{
		m_instance = this;
	}

	[SerializeField] int m_gridXDim;
	[SerializeField] int m_gridYDim;
	[SerializeField] GameObject m_emptyCellPrefab;
	[SerializeField] Camera m_camera;

	GridCell[,] m_grid;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine (BuildGrid());
	}

	IEnumerator BuildGrid()
	{
		yield return null;
		GridLayoutGroup glg = GetComponent<GridLayoutGroup> ();
		RectTransform rt = transform as RectTransform;
		Vector2 size = new Vector2 (rt.rect.width, rt.rect.height);
		Debug.Log("sizeDelta = "+size.x+", "+size.y);
		Vector2 cellSize = new Vector2 (size.x/m_gridXDim, size.y/m_gridYDim);
		Debug.Log("cellSize = "+cellSize.x+", "+cellSize.y);
		glg.cellSize = cellSize;

		m_grid = new GridCell[m_gridXDim,m_gridYDim];

		for (int y = 0; y < m_gridYDim; y++)
		{
			for (int x = 0; x < m_gridXDim; x++)
			{
				GameObject cellObj = Instantiate (m_emptyCellPrefab, transform);
				m_grid [x, y] = cellObj.GetComponent<GridCell> ();
			}
		}
	}

	public void AttachNewPieceToCell(GameObject piecePrefab, int x, int y)
	{
		GameObject newPieceObj = Instantiate (piecePrefab);
		Piece piece = newPieceObj.GetComponent<Piece> ();
		AttachExistingPieceToCell(piece, x, y);
	}

	public void AttachExistingPieceToCell(Piece piece, int x, int y)
	{
		GridCell cell = m_grid [x, y];
        cell.RemovePiece();
		cell.AttachPiece (piece);

		// make the new piece the same size as the cell
		RectTransform rt = piece.transform as RectTransform;
		Rect sourceRect = (cell.transform as RectTransform).rect;
		rt.sizeDelta = new Vector2(sourceRect.width, sourceRect.height);
    }

	public Piece RemovePieceFromCell(int x, int y)
	{
		GridCell cell = m_grid [x, y];
		Piece piece = cell.RemovePiece();
		return piece;
	}


	public IntVec2 GridPosFromTouchPos(Vector3 touchPos)
	{
		for (int y = 0; y < m_gridYDim; y++)
		{
			for (int x = 0; x < m_gridXDim; x++)
			{
				if (RectTransformUtility.RectangleContainsScreenPoint (m_grid [x, y].transform as RectTransform, (Vector2)touchPos, m_camera))
				{
					return new IntVec2 (x, y);
				}
			}
		}

		return IntVec2.NULL;
	}

	public IntVec2 GridPosFromWorldPos(Vector3 worldPos)
	{
		for (int y = 0; y < m_gridYDim; y++)
		{
			for (int x = 0; x < m_gridXDim; x++)
			{
				RectTransform cellRT = m_grid [x, y].transform as RectTransform;
				if (IsContained(worldPos,cellRT))
				{
					return new IntVec2 (x, y);
				}
			}
		}

		return IntVec2.NULL;
	}

	bool IsContained(Vector3 pos, RectTransform rt)
	{
		Vector3[] v = new Vector3[4];
		rt.GetWorldCorners(v);
		float[] xCoords = new float[4];
		float[] yCoords = new float[4];
		for (int i = 0; i < 4; i++)
		{
			xCoords [i] = v [i].x;
			yCoords [i] = v [i].y;
		}

		Vector2 min = new Vector2 (Mathf.Min (xCoords), Mathf.Min (yCoords));
		Vector2 max = new Vector2 (Mathf.Max (xCoords), Mathf.Max (yCoords));

		if ((pos.x > min.x) &&
		    (pos.x < max.x) &&
		    (pos.y > min.y) &&
		    (pos.y < max.y))
		{
			return true;
		}

		return false;
	}

	public Piece GetPieceAt(int x, int y)
	{
		GridCell cell = m_grid [x, y];
		Piece piece = cell.GetAttachedPiece ();
		return piece;
	}

	public IntVec2 FindPieceInGrid(Piece piece)
	{
		for (int y = 0; y < m_gridYDim; y++)
		{
			for (int x = 0; x < m_gridXDim; x++)
			{
				Piece xyPiece = GetPieceAt (x, y);
				if (xyPiece == piece)
				{
					return new IntVec2 (x, y);
				}
			}
		}

		return IntVec2.NULL;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void MovePiece(Piece piece, IntVec2 newPos)
	{
		IntVec2 previousGridPos = FindPieceInGrid (piece);
		if (!previousGridPos.IsNull())
		{
			RemovePieceFromCell (previousGridPos.x, previousGridPos.y);
		}
		AttachExistingPieceToCell (piece, newPos.x, newPos.y);
	}

	public bool PieceDropped(Piece piece)
	{
		IntVec2 gridPos = GridPosFromWorldPos (piece.transform.position);
		if (gridPos.IsNull())
		{
			// put it back where it was
			piece.transform.localPosition = Vector3.zero;
			return false;
		}

		MovePiece (piece, gridPos);


		return true;

//		// returns false if it wasn't dropped in a valid location
//		Vector3 touchPos = m_camera.WorldToScreenPoint(piece.transform.position);
//		IntVec2 gridPos = GetGridPosFromTouchPosition(touchPos);
//		IntVec2 previousGridPos = FindAnimalInGrid (piece.GetComponent<Animal> ());
//		piece.transform.localPosition = Vector3.zero;
//		int queueItemIndex = QueuePositionFromGameObject(piece);
//
//		AnimalDef droppedAnimalDef;
//		if (queueItemIndex >= 0)
//		{
//			droppedAnimalDef = m_animalQueue.QueuePosition (queueItemIndex);
//		} else
//		{
//			Animal droppedAnimal = piece.GetComponent<Animal> ();
//			droppedAnimalDef = droppedAnimal.GetDef ();
//		}
//
//		if (!gridPos.IsInvalid ())
//		{
//			// destroy the piece that you were carrying.
//			piece.transform.parent = null;
//			Destroy (piece);
//		}
//
//		return TryToPlacePiece(gridPos, previousGridPos, queueItemIndex, droppedAnimalDef);
	}
}
