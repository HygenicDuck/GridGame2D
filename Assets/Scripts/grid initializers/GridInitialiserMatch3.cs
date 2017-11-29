using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInitialiserMatch3 : GridInitialiser 
{
	[SerializeField] GameObject[] m_piecesPrefabs;

	public override void Initialise()
	{
		for(int y=0; y<GridCreator.Instance.m_gridYDim; y++)
		{
			for(int x=0; x<GridCreator.Instance.m_gridXDim; x++)
			{
				GridCreator.Instance.AttachNewPieceToCell(m_piecesPrefabs[UnityEngine.Random.Range(0,m_piecesPrefabs.Length)],x,y);
			}
		}
	}
}
