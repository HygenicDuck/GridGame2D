using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerSwap : GameControllerBase 
{
	public override void MovePiece(Piece piece, IntVec2 newPos)
	{
		IntVec2 previousGridPos = GridCreator.Instance.FindPieceInGrid (piece);
		if (!previousGridPos.IsNull())
		{
			GridCreator.Instance.RemovePieceFromCell (previousGridPos.x, previousGridPos.y);

			Piece displacedPiece = GridCreator.Instance.GetPieceAt (newPos.x, newPos.y);

			// default behaviour - swap the displaced piece into the space vacated by the moved piece.
			if (displacedPiece != null)
			{
				MovePiece (displacedPiece, previousGridPos);
			}
		}

		GridCreator.Instance.AttachExistingPieceToCell (piece, newPos.x, newPos.y);
	}

}
