using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridObserverMatch3 : GridObserver 
{
	protected class Group
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
	protected List<Group> m_matchingGroups = null;
	int m_numPiecesFalling; 

	protected override void Start () 
	{
		base.Start ();
	}

	void Update()
	{
		if (m_pauseTimer > 0f)
		{
			m_pauseTimer -= Time.deltaTime;
			if (m_pauseTimer < 0)
			{
				m_pauseTimer = 0f;
			}
			return;
		}

		if (!m_doObserving)
		{
			return;
		}

		ScanGridForGroups ();
		if (m_matchingGroups.Count > 0)
		{
			StartCoroutine (BubblePopSequence ());
		}
		else if (ScanGridForAnyEmptyCells ())
		{
			StartCoroutine (FallingPiecesSequence ());
		}

	}

	IEnumerator BubblePopSequence()
	{
		m_doObserving = false;
		DeleteAllMatchingGroups ();
		yield return new WaitForSeconds (0.1f);
		yield return FallingPiecesSequence ();
	}

	IEnumerator FallingPiecesSequence()
	{
		m_doObserving = false;
		RowsFallDown ();
		while (m_numPiecesFalling > 0)
		{
			yield return null;
		}
		m_doObserving = true;
	}

	protected abstract void ScanGridForGroups();

	bool ScanGridForAnyEmptyCells()
	{
		for(int y=0; y<m_gridCreator.m_gridYDim; y++)
		{
			for(int x=0; x<m_gridCreator.m_gridXDim; x++)
			{
				IntVec2 pos = new IntVec2 (x, y);
				if (m_gridCreator.GetPieceAt (pos) == null)
					return true;
			}
		}

		return false;
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

		movement.MoveBy (offset, timeRequired, Movement.MoveMode.USECURVE);

		yield return new WaitForSeconds (timeRequired);

		Destroy (movingPiece);
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

		Transform cell0 = m_gridCreator.CellTransform (x, 0);
		Transform cell1 = m_gridCreator.CellTransform (x, 1);
		Vector3 pitch = cell1.position - cell0.position;
		Vector3 offset = pitch * dropDistance;

		Vector3 pitchLocal = cell1.localPosition - cell0.localPosition;
		Vector3 offsetLocal = pitchLocal * dropDistance;

		Vector3 sourcePosition = destinationPosition - offset;
		movingPiece.transform.position = sourcePosition;

		float timeRequired = offsetLocal.magnitude / PIECE_FALL_SPEED;
		Movement movement = movingPiece.GetComponent<Movement> ();

		movement.MoveBy (offsetLocal, timeRequired, Movement.MoveMode.USECURVE);

		yield return new WaitForSeconds (timeRequired);

		Destroy (movingPiece);
		m_gridCreator.AttachNewPieceToCell (randomPiecePrefab, x, y);

		m_numPiecesFalling--;
	}
}
