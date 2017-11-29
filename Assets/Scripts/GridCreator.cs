using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridCreator : MonoBehaviour {

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

	public void AttachPieceToCell(PieceManager.PieceID pieceID, int x, int y)
	{
		GameObject prefab = PieceManager.Instance.GetPiecePrefab (pieceID);
		GridCell cell = m_grid [x, y];
        cell.RemovePiece();
		GameObject newPiece = Instantiate (prefab);
		cell.AttachPiece (newPiece);

		// make the new piece the same size as the cell
		RectTransform rt = newPiece.transform as RectTransform;
		Rect sourceRect = (cell.transform as RectTransform).rect;
		rt.sizeDelta = new Vector2(sourceRect.width, sourceRect.height);
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

	public Piece GetPieceAt(int x, int y)
	{
		GridCell cell = m_grid [x, y];
		Piece piece = cell.GetAttachedPiece ();
		return piece;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
