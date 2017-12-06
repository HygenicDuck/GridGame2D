using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDropTouchController : TouchController 
{
	// Use this for initialization
	protected override void Start () 
	{
		base.Start ();
		GameControllerBase.Instance.m_dragAndDropIsActive = true;
	}

}
