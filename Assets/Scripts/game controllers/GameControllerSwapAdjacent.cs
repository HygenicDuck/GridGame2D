using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerSwapAdjacent : GameControllerBase 
{
	public override void MovePiece(Piece piece, IntVec2 newPos)
	{
		IntVec2 previousGridPos = GridCreator.Instance.FindPieceInGrid (piece);
		if (!previousGridPos.IsNull())
		{
			GridCreator.Instance.RemovePieceFromCell (previousGridPos.x, previousGridPos.y);

			if (!GridCreator.Instance.IsAdjacent (newPos, previousGridPos))
			{
				// destination is not adjacent to the old position. Put it back where it was.
				newPos = previousGridPos;
			}
			else
			{
				Piece displacedPiece = GridCreator.Instance.GetPieceAt (newPos.x, newPos.y);

				// swap the displaced piece into the space vacated by the moved piece.
				if (displacedPiece != null)
				{
					MovePiece (displacedPiece, previousGridPos);
				}
			}
		}

		GridCreator.Instance.AttachExistingPieceToCell (piece, newPos.x, newPos.y);
	}


}
