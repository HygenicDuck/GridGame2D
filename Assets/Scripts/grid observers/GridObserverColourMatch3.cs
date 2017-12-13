using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObserverColourMatch3 : GridObserverMatch3 
{

	protected override void ScanGridForGroups()
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
							
		Group group = new Group ();
		cellPos = firstPos;
		for (int i = 0; i < 3; i++)
		{
			group.AddPosition (cellPos);
			cellPos = cellPos + dPos;
		}
		m_matchingGroups.Add(group);

		return true;
	}
}
