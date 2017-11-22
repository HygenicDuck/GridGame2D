using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceManager : MonoBehaviour {

	[SerializeField] GameObject[] piecePrefabs;


	static PieceManager m_instance = null;


	public static PieceManager Instance
	{
		get
		{
			return m_instance;
		}
	}




	// Use this for initialization
	void Start ()
	{
		m_instance = this;
	}


	public enum PieceID
	{
		RED,
		BLUE,
		YELLOW,
		BLACK,
		EMPTY
	}

	public GameObject GetPiecePrefab(PieceID piece)
	{
		if (piece == PieceID.EMPTY)
			return null;
		else
			return piecePrefabs [(int)piece];
	}
}
