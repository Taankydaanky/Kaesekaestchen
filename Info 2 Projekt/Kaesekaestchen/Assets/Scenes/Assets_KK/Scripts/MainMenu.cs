using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public GameObject mainMenu;
	public GameObject gameMenu;
	public GameObject test;
	// Use this for initialization

	//Name Input Field
	public InputField p1Field;
	public InputField p2Field;
	public Text p1;
	public Text p2;
	// Character name string
	public static string playerName1;
	public static string playerName2;
	bool isMute;
	//Music Mute
    public void Mute (){
    isMute = ! isMute;
    AudioListener.volume =  isMute ? 0 : 1;
 }
	public void OnSubmit1(){
		//Set playerName string to test in p1Field
		playerName1 = p1Field.text;
		PlayerPrefs.SetString("playerName1", playerName1);
		//Display Player Name in console
		Debug.Log("Player 1: " + playerName1);
	}
	public void OnSubmit2(){
		//Set playerName string to test in p1Field
		playerName2 = p2Field.text;
		PlayerPrefs.SetString("playerName2", playerName2);
		//Display Player Name in console
		Debug.Log("Player 2: " + playerName2);
	}
	void Awake() {
		test = GameObject.FindWithTag ("test");
        //DontDestroyOnLoad(test);
    }

	void Start () {
		mainMenu = GameObject.FindWithTag ("MainMenu");
		//gameMenu = GameObject.FindWithTag ("GameMenu");

	}
	//Button to reset all Prefs
	public void HighscoreReset()
	{
		PlayerPrefs.DeleteAll();
	}
	//Exit Button
	public void EndGame(){
		Application.Quit();
	}
	//Toggle Menus
	public void startMenu (){
		mainMenu.SetActive (false);
		gameMenu.SetActive (true);
	}
	//Load Maingame
	public void Example() {
        Application.LoadLevel("test");
    }
	//Load Maingame from WinScreen
	public void Rematch() {
        Application.LoadLevel("test");
    }
	//Load MainMenu from WinScreen
	public void Menu() {
        Application.LoadLevel("MainMenu");
    }

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			gameMenu.SetActive (false);
			mainMenu.SetActive (true);
		}
	}
}
