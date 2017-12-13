using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObserverPipeMatch3 : GridObserverMatch3 
{

	protected override void ScanGridForGroups()
	{
		m_matchingGroups = new List<Group> ();

		for(int y=0; y<m_gridCreator.m_gridYDim; y++)
		{
			for(int x=0; x<m_gridCreator.m_gridXDim; x++)
			{
				IntVec2 pos = new IntVec2 (x, y);
				CheckForConnectedPipe (pos);
			}
		}
	}


	void CheckForConnectedPipe(IntVec2 firstPos)
	{
		if (CellIsAlreadyInAGroup (firstPos))
		{
			return;
		}

		Group grp = new Group ();
		// search recursively for all adjacent pipe segments
		CheckAdjacentPipeSegments (firstPos, grp, PipePiece.Connector.ANY);
		if (grp.NumGridCells () >= 3)
		{
			m_matchingGroups.Add (grp);
		}
	}


	void CheckAdjacentPipeSegments(IntVec2 cellPos, Group grp, PipePiece.Connector requiredConnector)
	{
		// recursive

		if (!OnGrid (cellPos) || grp.ContainsPos(cellPos))
		{
			return;
		}

		PipePiece p = m_gridCreator.GetPieceAt (cellPos) as PipePiece;
		if (p == null)
		{
			return;
		}

		if (!p.HasConnector (requiredConnector))
		{
			return;
		}

		grp.AddPosition (cellPos);

		switch (p.m_type)
		{
		case PipePiece.PieceType.END_RIGHT:
			CheckAdjacentPipeSegments (cellPos + new IntVec2 (1, 0), grp, PipePiece.Connector.LEFT);
			break;
		case PipePiece.PieceType.END_LEFT:
			CheckAdjacentPipeSegments (cellPos + new IntVec2 (-1, 0), grp, PipePiece.Connector.RIGHT);
			break;
		case PipePiece.PieceType.END_UP:
			CheckAdjacentPipeSegments (cellPos + new IntVec2 (0, -1), grp, PipePiece.Connector.DOWN);
			break;
		case PipePiece.PieceType.END_DOWN:
			CheckAdjacentPipeSegments (cellPos + new IntVec2 (0, 1), grp, PipePiece.Connector.UP);
			break;
		case PipePiece.PieceType.HORIZONTAL:
			CheckAdjacentPipeSegments (cellPos + new IntVec2 (1, 0), grp, PipePiece.Connector.LEFT);
			CheckAdjacentPipeSegments (cellPos + new IntVec2 (-1, 0), grp, PipePiece.Connector.RIGHT);
			break;
		case PipePiece.PieceType.VERTICAL:
			CheckAdjacentPipeSegments (cellPos + new IntVec2 (0, 1), grp, PipePiece.Connector.UP);
			CheckAdjacentPipeSegments (cellPos + new IntVec2 (0, -1), grp, PipePiece.Connector.DOWN);
			break;
		case PipePiece.PieceType.UP_LEFT:
			CheckAdjacentPipeSegments (cellPos + new IntVec2 (-1, 0), grp, PipePiece.Connector.RIGHT);
			CheckAdjacentPipeSegments (cellPos + new IntVec2 (0, -1), grp, PipePiece.Connector.DOWN);
			break;
		case PipePiece.PieceType.UP_RIGHT:
			CheckAdjacentPipeSegments (cellPos + new IntVec2 (1, 0), grp, PipePiece.Connector.LEFT);
			CheckAdjacentPipeSegments (cellPos + new IntVec2 (0, -1), grp, PipePiece.Connector.DOWN);
			break;
		case PipePiece.PieceType.DOWN_LEFT:
			CheckAdjacentPipeSegments (cellPos + new IntVec2 (-1, 0), grp, PipePiece.Connector.RIGHT);
			CheckAdjacentPipeSegments (cellPos + new IntVec2 (0, 1), grp, PipePiece.Connector.UP);
			break;
		case PipePiece.PieceType.DOWN_RIGHT:
			CheckAdjacentPipeSegments (cellPos + new IntVec2 (1, 0), grp, PipePiece.Connector.LEFT);
			CheckAdjacentPipeSegments (cellPos + new IntVec2 (0, 1), grp, PipePiece.Connector.UP);
			break;
		}
	}


}
