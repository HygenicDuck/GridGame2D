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

	List<Group> m_matchingGroups = null;

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
	}

	void ScanGridFor3InARow()
	{
		m_matchingGroups = new List<Group> ();

		for(int y=0; y<GridCreator.Instance.m_gridYDim; y++)
		{
			for(int x=0; x<GridCreator.Instance.m_gridXDim; x++)
			{
				IntVec2 pos = new IntVec2 (x, y);
				CheckFor3InARow (pos, new IntVec2 (1, 0));
				CheckFor3InARow (pos, new IntVec2 (0, 1));
			}
		}

		DeleteAllMatchingGroups ();
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
			}
			cellPos = cellPos + dPos;
		}
	}
}
