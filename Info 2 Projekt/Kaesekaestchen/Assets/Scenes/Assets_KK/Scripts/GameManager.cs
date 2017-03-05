using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    //All Variables and GameObjects
    Camera MainCam; //Place the Camera
    public List<Kaestchen> Spielfeld; 
    public int groeße; // 5 Modi für 5 groeßen
    public Transform KPref;
    public List<Rahmen> RahmenZu;
    int Zeilen = 0;
    int Spalten= 0;
    public Text playerT;
    public int player = 1;
    public bool abgeschlossen = false;
    public int points1 = 0;
    public int points2 = 0;
    public Text pkt1;
    public Text pkt2;
    public Text timer;
    public float targetTime = 15.0f;
    public Text playerName1;
    public Text playerName2;
    public string player1;
    public string player2;
    public int count = 0;
    int gesPoints;
    int winner = 0;
    public Text win;
    public GameObject winningScreen;
    public Text highscore1;
    public Text highscore2;
    public Text highscore3;
    public Text highscore4;
    public string winnerName;
    public int zero = 0;
    public Text xlength;
    public Text ylength;
    public int rSpalte;
    public int rZeile;
    bool isMute;
 
    //Mute Toggle
    public void Mute (){
    isMute = ! isMute;
    AudioListener.volume =  isMute ? 0 : 1;
 }
    
    //Start options
    void Awake() {
        MainCam = Camera.main;
        SpielfeldGenerieren();
        KameraAkt();
        PlayAkt();
        playerName1.text = PlayerPrefs.GetString("playerName1");
        playerName2.text = PlayerPrefs.GetString("playerName2");
    }
    //Reload options
    public void neuladen(){
        for (int z = 0; z < Spielfeld.Count; z++)
        {
            Destroy(Spielfeld[z].v.gameObject);
        }
        SpielfeldGenerieren();
        KameraAkt();
        PlayAkt();
        TimerReset();
        points1 = 0;
        points2 = 0;
        pkt1.text = " " + points1.ToString();
        pkt2.text = " " + points2.ToString();
    }
    //Fieldgenerator
    void SpielfeldGenerieren()
   
    { 
        switch(groeße)
        {
            case 1:
                Spalten = 3;
                Zeilen = 5;
                break;
            case 2:
                Spalten = 5;
                Zeilen = 7;
                break;
            case 3:
                Spalten = 7;
                Zeilen = 10;
                break;
            case 4:
                Spalten = Random.Range(3, 11);
                Zeilen = Random.Range(3, 11);
                break;
            case 5:
                Spalten = 10;
                Zeilen = 10;
                break;
            case 6:
                Spalten = System.Convert.ToInt32(xlength.text);
                Zeilen = System.Convert.ToInt32(ylength.text);
                break;
        }
        //Creating lists/dictionaries
        List<int> Spa = Ausrichten(Spalten);
        List<int> Zeil = Ausrichten(Zeilen);
        Spielfeld = new List<Kaestchen>();
        //Filling up the Array and creating the Gamefield
        for (int x = 0; x < Spa.Count; x++)
        {
            for (int y = 0; y < Zeil.Count; y++)
            {
                Spielfeld.Add(new Kaestchen(Spa[x], Zeil[y], null));
            }
        }
        //Setting the Prefabs
        for (int z = 0; z < Spielfeld.Count; z++)
        {
            int x = Spielfeld[z].x;
            int y = Spielfeld[z].y;
            Transform KPrefclone = (Transform)Instantiate(KPref, new Vector3(x, y, 0), Quaternion.identity);
            Spielfeld[z].v = KPrefclone.GetComponent<SpriteRenderer>();
        }
        //Finding Neighbour Prefabs
        for (int z = 0; z < Spielfeld.Count; z++)
        {
            int x = Spielfeld[z].x;
            int y = Spielfeld[z].y;
            Spielfeld[z].kUp = Spielfeld[z].kDown = Spielfeld[z].kRight = Spielfeld[z].kLeft = -1;
            for(int f = 0; f < Spielfeld.Count; f++)
            {
                int fx = Spielfeld[f].x;
                int fy = Spielfeld[f].y;
                if (fx==x-1 && fy == y)
                {
                    Spielfeld[z].kLeft = f;
                }
                else if(fx==x+1&& fy == y)
                {
                    Spielfeld[z].kRight = f;
                }
                else if(fy==y-1&& fx == x)
                {
                    Spielfeld[z].kDown = f;
                }
                else if(fy==y+1 && fx == x)
                {
                    Spielfeld[z].kUp = f;
                }
            }
            //Init bools
            Spielfeld[z].bUp = Spielfeld[z].kUp == -1;
            Spielfeld[z].bDown = Spielfeld[z].kDown == -1;
            Spielfeld[z].bRight = Spielfeld[z].kRight == -1;
            Spielfeld[z].bLeft = Spielfeld[z].kLeft == -1;
            UpdateRahmen(z);
        }

        //Here i tried to get random fields but failed because the SpriteRenderer is unable to check if its null or not
/*        if(Spalten==10&&Zeilen==10)
        {
            rZeile = Random.Range(0,11);
            rSpalte = Random.Range(0,11);
            
            for (int i = 0; i < Spielfeld.Count; i++)
            {int z = Random.Range(0,100);
            Destroy(Spielfeld[z].v.gameObject);}
            
        }*/
    }
    //Kamerazoom
    void KameraAkt()
    {
        MainCam.orthographicSize = Spalten;
        MainCam.orthographicSize = Zeilen;
    }
    //gibt Liste für die y/x Koordinaten bei ungerade und geraden Zahlen um 0 herum
    List<int> Ausrichten(int zahl)
    {
        List<int> aktList = new List<int>();
        if (((float)zahl) / 2 == Mathf.Round(((float)zahl) / 2))
        {
            int rh = 0;
            int lh = rh = zahl / 2;
            for (int x = 0; x < rh; x++)
            {
                aktList.Add(-Mathf.Abs(rh - x));
            }
            aktList.Add(0);
            lh--;
            for (int x = 0; x < lh; x++)
            {
                aktList.Add(x + 1);
            }
        }
        else
        {
            int rh = 0;
            int lh = rh = (zahl - 1) / 2;
            for (int x = 0; x < rh; x++)
            {
                aktList.Add(-Mathf.Abs(rh - x));
            }
            aktList.Add(0);
            for (int x = 0; x < lh; x++)
            {
                aktList.Add(x + 1);
            }
        }
        return aktList;

    }
    //Reload Box with id to know if they got a border or not
    void UpdateRahmen(int id)  
    {
        string cid= "";
        cid += (Spielfeld[id].bUp) ? "1" : "0";
        cid += (Spielfeld[id].bRight) ? "1" : "0";
        cid += (Spielfeld[id].bDown) ? "1" : "0";
        cid += (Spielfeld[id].bLeft) ? "1" : "0";
        Spielfeld[id].v.sprite = Sreturn(cid);

    }
    //Looking for right Sprite id's
    Sprite Sreturn(string id)
    {
        for(int x = 0; x < RahmenZu.Count; x++)
        {
            if(RahmenZu[x].id == id)
            {
                return RahmenZu[x].bild;
            }
        }
        return null;
    }
    //Change bools of box id and their Neighbour(if neccessary)
    public void RahmenSetzen(int i,int id)
    {
        switch (i)
        {
            case 1: //up
                Spielfeld[id].bUp = true;
                UpdateRahmen(id);
                Check(id);
                if (Spielfeld[id].kUp != -1) //another box is on the top
                {
                    Spielfeld[Spielfeld[id].kUp].bDown = true;
                    UpdateRahmen(Spielfeld[id].kUp);
                    Check(Spielfeld[id].kUp);
                }
                break;
            case 2: //right
                Spielfeld[id].bRight = true;
                UpdateRahmen(id);
                Check(id);
                if (Spielfeld[id].kRight != -1) //another box is on the right
                {
                    Spielfeld[Spielfeld[id].kRight].bLeft = true;
                    UpdateRahmen(Spielfeld[id].kRight);
                    Check(Spielfeld[id].kRight);
                }
                break;
            case 3: //down
                Spielfeld[id].bDown = true;
                UpdateRahmen(id);
                Check(id);
                if (Spielfeld[id].kDown != -1) //another box is on the bottom
                {
                    Spielfeld[Spielfeld[id].kDown].bUp = true;
                    UpdateRahmen(Spielfeld[id].kDown);
                    Check(Spielfeld[id].kDown);
                }
                break;
            case 4: //left
                Spielfeld[id].bLeft = true;
                UpdateRahmen(id);
                Check(id);
                if (Spielfeld[id].kLeft != -1) //another box is on the left
                {
                    Spielfeld[Spielfeld[id].kLeft].bRight = true;
                    UpdateRahmen(Spielfeld[id].kLeft);
                    Check(Spielfeld[id].kLeft);
                }
                break;
        }
        if (!abgeschlossen)
        {
            PlayAkt();
            TimerReset();
        }else
        {
            abgeschlossen = false;
        }
    }
    //Completed one box?
    void Check(int id)
    {
        if (Spielfeld[id].bUp && Spielfeld[id].bRight && Spielfeld[id].bDown && Spielfeld[id].bLeft) //Box id is completed
        {
            Spielfeld[id].player = player;
            abgeschlossen = true;
            count = count+1;
            TimerReset();   //Timer reset
            AudioSource audio = GetComponent<AudioSource>();    //Play audio
            audio.Play();
            

            //Score counter
            if (player == 1)
            {
                
                points1 = points1 + 1;
                Debug.Log(points1);
                pkt1.text = " " + points1.ToString();
            }else 
            {
                
                points2 = points2 + 1;
                Debug.Log(points2);
                pkt2.text = " " + points2.ToString();
            }
            
            //Colorize the box for the player which completed the box
            for (int x = 0; x < Spielfeld.Count; x++)
            {
                Color co = Color.white;
                if (Spielfeld[x].player == 1)
                {
                    co = Color.red;
                }
                if (Spielfeld[x].player == 2)
                {
                    co = Color.blue;
                }
                Spielfeld[x].v.transform.GetChild(0).GetComponent<SpriteRenderer>().color = co;
            }
            //Counting points together
            gesPoints = points1 + points2;
        }
        if (gesPoints == Spielfeld.Count)
        {
            Invoke("WScreen", 1);   //Wait 1 Second
            Highscore();    //Refresh Highscore
            ModeSelect();   
        }
    }
    //WaitForSeconds function
    public void ModeSelect()
    {
        StartCoroutine(WScreen("SurveillanceModeSelectScreen"));
    }
    //winningScreen 
    public IEnumerator WScreen(string SurveillanceModeSelectScreen)
    {
        yield return new WaitForSeconds(01);
            if (points1 > points2)
            {
                
                Debug.Log("Player 1 hat gewonnen");
                winner = 1;
                Application.LoadLevel("win1");
 
            }else
            {
                Debug.Log("Player 2 hat gewonnen");
                winner = 2;
                Application.LoadLevel("win2");

            }
    }
    //changing Text and Variable for each turn
    public void PlayAkt()
    {
        if(player == 1)
        {
            player = 2;
        }
        else
        {
            player = 1;
        }
        playerT.text = "Player " + player.ToString();
    }
    //Everything what should be opened after loading scene
    void Start()
    {
        
        highscore1.text = "3*5: " + PlayerPrefs.GetString("HScoreWinner1") + " " + PlayerPrefs.GetInt("Highscore1").ToString(); //Highscore for 3*5
        highscore2.text = "5*7: " + PlayerPrefs.GetString("HScoreWinner2") + " " + PlayerPrefs.GetInt("Highscore2").ToString(); //Highscore for 5*7
        highscore3.text = "7*10: " + PlayerPrefs.GetString("HScoreWinner3") + " " + PlayerPrefs.GetInt("Highscore3").ToString(); //Highscore for 7*10
        highscore4.text = "Rnd: " + PlayerPrefs.GetString("HScoreWinner4") + " " + PlayerPrefs.GetInt("Highscore4").ToString(); //Highscore for Random
        winningScreen = GameObject.FindWithTag ("winningScreen");
        points1=0;
        points2=0;
        UpdatePoints1();
        UpdatePoints2();
        pkt1.text = " " + points1.ToString();
        pkt2.text = " " + points2.ToString();
        Debug.Log(PlayerPrefs.GetString("playerName1"));    //Display names in console
        Debug.Log(PlayerPrefs.GetString("playerName2"));
    }
    //Updating points
    void UpdatePoints1()
    {
        pkt1.text = points1.ToString();
    }
    //Updating points
    void UpdatePoints2()
    {
        pkt2.text = points2.ToString();
    }
    //Highscore for four modes
    void Highscore()
    {
        if(Spielfeld.Count == 15)
        {   if(points1 > PlayerPrefs.GetInt("Highscore1"))
            {
                PlayerPrefs.SetString("HScoreWinner1", PlayerPrefs.GetString("playerName1"));
                PlayerPrefs.SetInt("Highscore1" , points1);
                PlayerPrefs.SetString("player1", PlayerPrefs.GetString("playerName1"));
                highscore1.text = PlayerPrefs.GetString("HScoreWinner1") + " " + PlayerPrefs.GetInt("Highscore1").ToString();
            }
            if(points2 > PlayerPrefs.GetInt("Highscore1"))
            {
                PlayerPrefs.SetString("HScoreWinner1", PlayerPrefs.GetString("playerName2"));
                PlayerPrefs.SetInt("Highscore1" , points2);
                PlayerPrefs.SetString("player2", PlayerPrefs.GetString("playerName2"));
                highscore1.text = PlayerPrefs.GetString("HScoreWinner1") + " " + PlayerPrefs.GetInt("Highscore1").ToString();
            }
        }
        else if(Spielfeld.Count == 35)
        {   if(points1 > PlayerPrefs.GetInt("Highscore2"))
            {
                PlayerPrefs.SetString("HScoreWinner2", PlayerPrefs.GetString("playerName1"));
                PlayerPrefs.SetInt("Highscore2" , points1);
                PlayerPrefs.SetString("player1", PlayerPrefs.GetString("playerName1"));
                highscore2.text = PlayerPrefs.GetString("HScoreWinner2") + " " + PlayerPrefs.GetInt("Highscore2").ToString();
            }
            if(points2 > PlayerPrefs.GetInt("Highscore2"))
            {
                PlayerPrefs.SetString("HScoreWinner2", PlayerPrefs.GetString("playerName2"));
                PlayerPrefs.SetInt("Highscore2" , points2);
                PlayerPrefs.SetString("player2", PlayerPrefs.GetString("playerName2"));
                highscore2.text = PlayerPrefs.GetString("HScoreWinner2") + " " + PlayerPrefs.GetInt("Highscore2").ToString();
            }
        }
        else if(Spielfeld.Count == 70)
        {   if(points1 > PlayerPrefs.GetInt("Highscore3"))
            {
                PlayerPrefs.SetString("HScoreWinner3", PlayerPrefs.GetString("playerName1"));
                PlayerPrefs.SetInt("Highscore3" , points1);
                PlayerPrefs.SetString("player1", PlayerPrefs.GetString("playerName1"));
                highscore3.text = PlayerPrefs.GetString("HScoreWinner3") + " " + PlayerPrefs.GetInt("Highscore3").ToString();
            }
            if(points2 > PlayerPrefs.GetInt("Highscore3"))
            {
                PlayerPrefs.SetString("HScoreWinner3", PlayerPrefs.GetString("playerName2"));
                PlayerPrefs.SetInt("Highscore3" , points2);
                PlayerPrefs.SetString("player2", PlayerPrefs.GetString("playerName2"));
                highscore3.text = PlayerPrefs.GetString("HScoreWinner3") + " " + PlayerPrefs.GetInt("Highscore3").ToString();
            }
        }        
        else
        {   if(points1 > PlayerPrefs.GetInt("Highscore4"))
            {
                PlayerPrefs.SetString("HScoreWinner4", PlayerPrefs.GetString("playerName1"));
                PlayerPrefs.SetInt("Highscore4" , points1);
                PlayerPrefs.SetString("player1", PlayerPrefs.GetString("playerName1"));
                highscore4.text = PlayerPrefs.GetString("HScoreWinner4") + " " + PlayerPrefs.GetInt("Highscore4").ToString();
            }
            if(points2 > PlayerPrefs.GetInt("Highscore4"))
            {
                PlayerPrefs.SetString("HScoreWinner4", PlayerPrefs.GetString("playerName2"));
                PlayerPrefs.SetInt("Highscore4" , points2);
                PlayerPrefs.SetString("player2", PlayerPrefs.GetString("playerName2"));
                highscore4.text = PlayerPrefs.GetString("HScoreWinner4") + " " + PlayerPrefs.GetInt("Highscore4").ToString();
            }
        }
    }
    //Timer 
    void Update(){
        targetTime -= Time.deltaTime;
        int res = (int) (targetTime * 10000);
        float fRes = res/10000;
        timer.text = " " + fRes.ToString();

 
        if (targetTime <= 0.0f)
        {
            timerEnded();
            targetTime = 15.0f;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.LoadLevel("MainMenu");
		}
        
        }
        
        void timerEnded()
        {
            PlayAkt();
        }
        void TimerReset()
        {
            targetTime = 15.0f;
        }
    }
[System.Serializable] //its possible to see all Variables in the Inspector
public class Kaestchen {
    public int x;
    public int y;
    public SpriteRenderer v;
    public bool bUp;
    public bool bDown;
    public bool bRight;
    public bool bLeft;
    public int kUp = -1;
    public int kDown = -1;
    public int kRight = -1;
    public int kLeft = -1;
    public int player = -1;



    public Kaestchen(int xx, int yy, SpriteRenderer vv) {
        x = xx;
        y = yy;
        v = vv;
    }
}
[System.Serializable]
public class Rahmen
{
    public string id;
    public Sprite bild;
}
