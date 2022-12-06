using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameControllerEC : MonoBehaviour
{


    //Timer management
    private float tickTimer = 0;
    private int current_tick_count = 0;
    private const float TICK_LENGTH = 1f;
    [SerializeField]
    private int countDownLength;

    bool letter1Pressed = false;
    bool letter2Pressed = false;
    bool letter3Pressed = false;

    int[] lettersToPress = new int[3];

    bool hasWon = false;
    
    // Start is called before the first frame update
    void Start()
    {
        current_tick_count = 0;
        System.Random rnd = new System.Random();
        for (int i = 0; i<3; i++)
        {
            int temp = rnd.Next(97, 123);
            for (int x = 0; x < i; x++)
            {
                if (lettersToPress[x] == temp)
                {
                    i--;
                    break;
                }
            }
            lettersToPress[i] = temp;
        }
        if (StaticGameData.isMusicOn)
            GetComponent<AudioSource>().volume = 0.775f;
        else
            GetComponent<AudioSource>().volume = 0;


    }

    void Update()
    {
        if(letter1Pressed &  letter2Pressed && letter3Pressed && !hasWon)
        {
            hasWon = true;
            AudioSource ads = GetComponent<AudioSource>();
            ads.Stop();
            if(StaticGameData.isSoundOn) ads.PlayOneShot(StaticGameData.winSoundEffect, 0.5f);
            StaticGameData.Game.Points++;
            StartCoroutine(StaticGameData.swapScene());
        }
        tickTimer += Time.deltaTime;
        if (tickTimer > TICK_LENGTH)
        {
            current_tick_count++;
            if (current_tick_count == 2) {
                for (int i = 0; i < 3; i++)
                {
                    GameObject.Find("Canvas/Letter" + (i + 1)).GetComponent<Text>().text = "" + (char)lettersToPress[i];
                }
                GameObject.Find("Canvas/Instructions").GetComponent<Text>().text = "";
            }
            if (current_tick_count >= countDownLength)
            {
                StaticGameData.isLost = true;
                StartCoroutine(StaticGameData.swapScene());
            }
            tickTimer = 0;
        }
    }

    void OnGUI()
    {
        if (current_tick_count < 2) return;
        Event e = Event.current;
        if (e.isKey && e.type == EventType.KeyUp)
        {
            if (e.keyCode == (KeyCode)lettersToPress[0])
            {
                letter1Pressed = true;
                GameObject.Find("tige1").transform.position +=  new Vector3(0, 1, 0);
            }
            else if (e.keyCode == (KeyCode)lettersToPress[1])
            {
                letter2Pressed = true;
                GameObject.Find("tige2").transform.position += new Vector3(0, 1, 0);
            }
            else if (e.keyCode == (KeyCode)lettersToPress[2])
            {
                letter3Pressed = true;
                GameObject.Find("tige3").transform.position += new Vector3(0, 1, 0);
            }
            else
            {
                StaticGameData.isLost = true;
                StartCoroutine(StaticGameData.swapScene());
            }
        }
    }
}
