using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour 
{
	//[SerializeField]
	Vector2 m_touchAreaSize;

	Camera m_camera;
	bool m_touching = false;
	bool m_dragging = false;
	float m_dragStartTime = 0f;
	Vector3 m_touchPosition;

	// Use this for initialization
	void Start () 
	{
		GameObject camera = GameObject.Find("Main Camera");
		if (camera != null)
		{
			m_camera = camera.GetComponent<Camera>();
		}

		RectTransform rt = transform as RectTransform;
		m_touchAreaSize = new Vector2 (rt.rect.width/2, rt.rect.height/2);
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown(0))
		{
			m_touching = true;
			m_touchPosition = Input.mousePosition;

			Vector3 spritePos = m_camera.WorldToScreenPoint(transform.position);
			Vector3 dPos = spritePos - m_touchPosition;
			if ((Mathf.Abs(dPos.x) < m_touchAreaSize.x/2) && (Mathf.Abs(dPos.y) < m_touchAreaSize.y/2))
			{
				m_dragging = true;
				m_dragStartTime = Time.timeSinceLevelLoad;
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			if (m_dragging)
			{
				m_dragging = false;
				GridCreator.Instance.PieceDropped(gameObject.GetComponent<Piece>());
			}
		}
		else if (Input.GetMouseButton(0) && m_dragging)
		{
			Vector3 worldTouchPos = m_camera.ScreenToWorldPoint(m_touchPosition);
			Vector3 worldMousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
			Vector3 dPos = worldMousePos - worldTouchPos;
			m_touchPosition = Input.mousePosition;
			Vector3 pos = transform.position;
			pos += dPos;
			transform.position = pos;
		}
	}

}
