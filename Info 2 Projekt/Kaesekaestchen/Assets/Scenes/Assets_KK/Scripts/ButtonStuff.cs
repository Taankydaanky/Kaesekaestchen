using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ButtonStuff : MonoBehaviour {

    public Text groeße;
    public int gr;
    public GameManager gm;

    //Start Settings
    void Awake()
    {
        Time.timeScale = 1;
        gr = 1;
        //gm.groeße = gr;
    }
    //Reload function
    public void Reload()
    {
        gm.neuladen();
    }
    //Switch-case for the Sizes
    public void Groeße()
    {
        switch(gr)
        {
            case 1:
                gr = 2;
                gm.groeße = gr;
                groeße.text = "5*7";
                break;
            case 2:
                gr = 3;
                gm.groeße = gr;
                groeße.text = "7*10";
                break;
            case 3:
                gr = 4;
                gm.groeße = gr;
                groeße.text = "Random";
                break;
            case 4:
                gr = 5;
                gm.groeße = gr;
                groeße.text = "10*10";
                break;
            case 5:
                gr = 6;
                gm.groeße = gr;
                groeße.text = "User";
                break;
            case 6:
                gr = 1;
                gm.groeße = gr;
                groeße.text = "3*5";
                break;
        }
    }
}
