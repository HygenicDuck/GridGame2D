using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObserverMatch3 : GridObserver 
{
	class Group
	{
		public IntVec2 position;
		public IntVec2 direction;

		public Group(IntVec2 p, IntVec2 dir)
		{
			position = p;
			direction = dir;
		}
	}

	[SerializeField] public GameObject m_bubblePopEffect;
	List<Group> m_matchingGroups = null;
	int m_numPiecesFalling; 

	protected override void Start () 
	{
		base.Start ();
	}

	void Update()
	{
		if (!m_doObserving)
		{
			return;
		}

		ScanGridFor3InARow ();
		if (m_matchingGroups.Count > 0)
		{
			StartCoroutine(BubblePopSequence ());
		}

	}

	IEnumerator BubblePopSequence()
	{
		m_doObserving = false;
		DeleteAllMatchingGroups ();
		yield return new WaitForSeconds (0.1f);
		RowsFallDown ();
		while (m_numPiecesFalling > 0)
		{
			yield return null;
		}
		m_doObserving = true;
	}

	void ScanGridFor3InARow()
	{
		m_matchingGroups = new List<Group> ();

		for(int y=0; y<m_gridCreator.m_gridYDim; y++)
		{
			for(int x=0; x<m_gridCreator.m_gridXDim; x++)
			{
				IntVec2 pos = new IntVec2 (x, y);
				CheckFor3InARow (pos, new IntVec2 (1, 0));
				CheckFor3InARow (pos, new IntVec2 (0, 1));
			}
		}
	}



	bool CheckFor3InARow(IntVec2 firstPos, IntVec2 dPos)
	{
		// checks for 3 matching cells, starting at cellPos, and adding dPos each time.

		IntVec2 cellPos = firstPos;
		Piece p = m_gridCreator.GetPieceAt (cellPos);
		if (p == null)
		{
			return false;
		}

		for (int i = 0; i < 2; i++)
		{
			cellPos = cellPos + dPos;
			Piece p2 = m_gridCreator.GetPieceAt (cellPos);
			if ((p2 == null) || !p.Equals (p2))
			{
				return false;
			}
		}
								
		m_matchingGroups.Add(new Group(firstPos, dPos));
		return true;
	}

	void DeleteAllMatchingGroups()
	{
		foreach (Group g in m_matchingGroups)
		{
			DeleteGroup (g.position, g.direction);
		}

		m_matchingGroups.Clear ();
	}

	void DeleteGroup(IntVec2 cellPos, IntVec2 dPos)
	{
		for (int i = 0; i < 3; i++)
		{
			Piece piece = m_gridCreator.RemovePieceFromCell (cellPos);
			if (piece != null)
			{
				Destroy (piece.gameObject);
				GameObject effect = Instantiate (m_bubblePopEffect,m_gridCreator.CellTransform(cellPos.x, cellPos.y));
			}
			cellPos = cellPos + dPos;
		}
	}



	void RowsFallDown()
	{
		m_numPiecesFalling = 0;
		for (int x = 0; x < m_gridCreator.m_gridYDim; x++)
		{
			SingleRowFallsDown(x);
		}
	}

	void SingleRowFallsDown(int x)
	{
		int y = m_gridCreator.m_gridYDim - 1;
		int dropDistance = 0;
		while (y >= 0)
		{
			Piece piece = m_gridCreator.GetPieceAt (x, y);
			if (piece == null)
			{
				dropDistance++;
			}
			else
			{
				if (dropDistance > 0)
				{
					MovePiece (new IntVec2 (x, y), new IntVec2 (x, y + dropDistance));
				}
			}

			y -= 1;
		}

		// fill the gap left at the top with random pieces
		for (int gapY = dropDistance - 1; gapY >= 0; gapY--)
		{
			StartCoroutine(AddPieceAtTopCoroutine (x, gapY, dropDistance));
			//m_gridCreator.AttachNewPieceToCell (GridInitialiser.Instance.RandomPiecePrefab (), x, gapY);
		}
	}

	void MovePiece(IntVec2 from, IntVec2 to)
	{
		StartCoroutine (MovePieceCoroutine(from, to));
	}

	const float PIECE_FALL_SPEED = 800f;

	IEnumerator MovePieceCoroutine(IntVec2 from, IntVec2 to)
	{
		m_numPiecesFalling++;
		Piece piece = m_gridCreator.GetPieceAt (from);

		GameObject movingPiece = MovingPiecesController.Instance.CreateMovingPiece (piece);
		movingPiece.transform.position = piece.transform.position;
		piece.gameObject.SetActive (false);
		m_gridCreator.RemovePieceFromCell (from);

		Transform fromTrans = m_gridCreator.CellTransform (from.x, from.y);
		Transform toTrans = m_gridCreator.CellTransform (to.x, to.y);
		Vector3 offset = toTrans.localPosition - fromTrans.localPosition;
		float timeRequired = offset.magnitude / PIECE_FALL_SPEED;

		Movement movement = movingPiece.GetComponent<Movement> ();

		movement.MoveBy (offset, timeRequired, false);

		yield return new WaitForSeconds (timeRequired);

		Destroy (movingPiece);
		//piece = m_gridCreator.RemovePieceFromCell (from);
		piece.gameObject.SetActive (true);
		m_gridCreator.AttachExistingPieceToCell (piece, to.x, to.y);
		m_numPiecesFalling--;
	}


	IEnumerator AddPieceAtTopCoroutine(int x, int y, int dropDistance)
	{
		m_numPiecesFalling++;

		GameObject randomPiecePrefab = GridInitialiser.Instance.RandomPiecePrefab ();
		GameObject movingPiece = MovingPiecesController.Instance.CreateMovingPiece (randomPiecePrefab.GetComponent<Piece>());
		RectTransform rt = movingPiece.transform as RectTransform;
		RectTransform cellRT = m_gridCreator.CellTransform (0, 0) as RectTransform;
		rt.sizeDelta = cellRT.sizeDelta;

		Transform cellTransform = m_gridCreator.CellTransform (x, y);
		Vector3 destinationPosition = cellTransform.position;
		Debug.Log ("destinationPosition "+destinationPosition.x+", "+destinationPosition.y);

		Transform cell0 = m_gridCreator.CellTransform (x, 0);
		Transform cell1 = m_gridCreator.CellTransform (x, 1);
		Vector3 pitch = cell1.position - cell0.position;
		Debug.Log ("pitch "+pitch.x+", "+pitch.y);
		Vector3 offset = pitch * dropDistance;
		Debug.Log ("offset "+offset.x+", "+offset.y);

		Vector3 pitchLocal = cell1.localPosition - cell0.localPosition;
		Debug.Log ("pitchLocal "+pitchLocal.x+", "+pitchLocal.y);
		Vector3 offsetLocal = pitchLocal * dropDistance;
		Debug.Log ("offsetLocal "+offsetLocal.x+", "+offsetLocal.y);


		Vector3 sourcePosition = destinationPosition - offset;
		Debug.Log ("sourcePosition "+sourcePosition.x+", "+sourcePosition.y);
		movingPiece.transform.position = sourcePosition;

		float timeRequired = offsetLocal.magnitude / PIECE_FALL_SPEED;
		Movement movement = movingPiece.GetComponent<Movement> ();

		movement.MoveBy (offsetLocal, timeRequired, false);

		yield return new WaitForSeconds (timeRequired);

		Destroy (movingPiece);
		m_gridCreator.AttachNewPieceToCell (randomPiecePrefab, x, y);

		m_numPiecesFalling--;
	}
}
