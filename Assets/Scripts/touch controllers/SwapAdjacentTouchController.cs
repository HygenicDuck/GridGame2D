using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapAdjacentTouchController : TouchController 
{
	Vector2 m_touchPosition;
	Vector2 m_touchDownPosition;
	bool m_touching;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown(0))
		{
			m_touching = true;
			m_touchPosition = Input.mousePosition;
			m_touchDownPosition = Input.mousePosition;

			OnTouchDown (m_touchPosition);
		}
		else if (Input.GetMouseButtonUp(0))
		{
			m_touching = false;
		}
		else if (Input.GetMouseButton(0) && m_touching)
		{
			Vector2 diff = (Vector2)(Input.mousePosition) - m_touchPosition;
			float distance;
			if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
			{
				diff.y = 0f;
				distance = diff.x;
			}
			else
			{
				diff.x = 0f;
				distance = diff.y;
			}

			IntVec2 gridPos1 = m_gridCreator.GridPosFromTouchPos (m_touchPosition);
			IntVec2 gridPos2 = m_gridCreator.GridPosFromTouchPos (m_touchPosition + diff);

			if (gridPos1 != gridPos2)
			{
				// we dragged far enough to cause a swap

				// make sure we havent dragged more than one cell
				IntVec2 iDiff = (gridPos2 - gridPos1);
				if (iDiff.x > 1)
					iDiff.x = 1;
				if (iDiff.y > 1)
					iDiff.y = 1;
				if (iDiff.x < -1)
					iDiff.x = -1;
				if (iDiff.y < -1)
					iDiff.y = -1;
				gridPos2 = gridPos1 + iDiff;

				GameControllerBase.Instance.MovePiece (gridPos1, gridPos2);

				m_touching = false;
			}
		}
	}

	void OnTouchDown(Vector3 touchPos)
	{
		// highlight the piece you have touched down on?
	}
}
