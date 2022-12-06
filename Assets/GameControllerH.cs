using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerH : MonoBehaviour
{

    //pressCount
    [SerializeField]
    int pressCountToWin = 50;
    int pressCount = 0;
    int arrowCount = 0;
    int columnCount = 1;
    bool lastKeyState = false;
    //GameObjects
    GameObject downArrowUp;
    GameObject downArrowDown;
    GameObject pencil;
    Text timerDisplay;
    //Timer management
    [SerializeField]
    private float countDownLength = 10;
    public bool timerIsRunning = false;
    //Victory management
    bool hasWon = false;
    bool hasLost = false;
    // Start is called before the first frame update
    void Start()
    {
        timerIsRunning = true;
        downArrowUp = GameObject.Find("DownArrowKey");
        downArrowDown = GameObject.Find("DownArrowKeyDown");
        pencil = GameObject.Find("Pencil");
        timerDisplay = GameObject.Find("Canvas/TimerDisplay").GetComponent<Text>();
        downArrowUp.SetActive(true);
        downArrowDown.SetActive(false);

        if (StaticGameData.isMusicOn)
            GetComponent<AudioSource>().volume = 0.775f;
        else
            GetComponent<AudioSource>().volume = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (hasWon || hasLost) return;
        countDownLength -= Time.deltaTime;
        timerDisplay.text = Mathf.Round(countDownLength) + " secondes";
        if(countDownLength < 0)
        {
            hasLost = true;
            AudioSource ads = GameObject.Find("GameController").GetComponent<AudioSource>();
            ads.Stop();
            if (StaticGameData.isSoundOn) ads.PlayOneShot(StaticGameData.lossSoundEffect, 0.5f);
            StaticGameData.isLost = true;
            StartCoroutine(StaticGameData.swapScene());
        }
    }

    void OnGUI()
    {
        if (hasWon || hasLost) return;
        Event e = Event.current;
        if (e.isKey && e.keyCode==KeyCode.DownArrow)
        {
           if(e.type == EventType.KeyDown && lastKeyState==false)
            {
                lastKeyState = true;
                downArrowUp.SetActive(false);
                downArrowDown.SetActive(true);
                pressCount++;
                if(pressCount >= pressCountToWin)
                {
                    hasWon = true;
                    AudioSource ads = GetComponent<AudioSource>();
                    ads.Stop();
                    ads.PlayOneShot(StaticGameData.winSoundEffect, 0.5f);
                    StaticGameData.Game.Points++;
                    StartCoroutine(StaticGameData.swapScene());
                }
                columnCount++;
                if (pressCount % 12 == 0) { 
                    arrowCount++;
                    columnCount = 0;
                }
                pencil.transform.position = new Vector3((-6)+columnCount * 0.5f, 3f-arrowCount*1.2f, 1.0f);
            }
            else if(e.type == EventType.KeyUp && lastKeyState == true)
            {
                lastKeyState = false;
                downArrowUp.SetActive(true);
                downArrowDown.SetActive(false);

            }
        }
    }
}
