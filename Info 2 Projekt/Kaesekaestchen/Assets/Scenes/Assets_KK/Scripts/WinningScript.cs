using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningScript : MonoBehaviour {

	public string spieler1;
	public string spieler2;
	public int gesamtPunkte;
	public int spielfeld;
	public GameObject winningScreen;

	// Use this for initialization
	void Start () {
		string spieler1 = PlayerPrefs.GetString("playerName1");
		string spieler2 = PlayerPrefs.GetString("playerName2");
		int gesamtPunkte = PlayerPrefs.GetInt("allPoints");
		int spielfeld = PlayerPrefs.GetInt("fieldSize");
		Debug.Log(spielfeld);
		winningScreen = GameObject.FindWithTag ("winningScreen");
	}
	
	// Update is called once per frame
	void Update () {

	}
	void Victory()
	{
		if(gesamtPunkte == spielfeld)
		{
			winningScreen.SetActive(true);
		}
	}
}
