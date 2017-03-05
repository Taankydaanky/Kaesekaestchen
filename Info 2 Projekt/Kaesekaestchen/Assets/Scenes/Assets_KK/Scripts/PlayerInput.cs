using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
    Camera MainCam;
    public GameManager gm;
    public AudioSource[] sounds;
    public AudioSource noise1;
    public AudioSource noise2;

    //Setting the Cameraview
    void Awake()
    {
        MainCam = Camera.main;
    }
    //Startoptions for Audio
    void Start()
    {
        sounds = GetComponents<AudioSource>();
        noise1 = sounds[0];
        noise2 = sounds[1];

    }

    //Mouse/Touchfunctions
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            noise2.Play(); //Play sound
            Vector2 pos = MainCam.ScreenToWorldPoint(Input.mousePosition); //Mouse/Touch position

            for (int x = 0; x < gm.Spielfeld.Count; x++) 
            {
                float dx = pos.x - gm.Spielfeld[x].x;
                float dy = pos.y - gm.Spielfeld[x].y; 

                if (Mathf.Abs(dx) < 0.5f && Mathf.Abs(dy) < 0.5f) //Mouse/Touch in positions box?
                {
                    if (Mathf.Abs(dx) > Mathf.Abs(dy)) //if Mouse/Touch near to x-koords
                    {
                        if (dx > 0) //direction = right
                        {
                            if (!gm.Spielfeld[x].bRight) //Border not placed
                            {
                                gm.RahmenSetzen(2, x);
                            }
                        }
                        else //direction = left
                        {
                            if (!gm.Spielfeld[x].bLeft) //Border not placed
                            {
                                gm.RahmenSetzen(4, x);
                            }
                        }
                    }
                    else //if Mouse/Touch near to x-koords
                    {
                        if (dy > 0) //direction = up
                        {
                            if (!gm.Spielfeld[x].bUp) //Border not placed
                            {
                                gm.RahmenSetzen(1, x);
                            }
                        }
                        else //direction = down
                        {
                            if (!gm.Spielfeld[x].bDown) //Border not placed
                            {
                                gm.RahmenSetzen(3, x);
                            }
                        }
                    }
                    break;
                }

            }
        }
    }
	
}
