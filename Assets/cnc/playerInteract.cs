using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerInteract : MonoBehaviour
{

    private int interactions = 0;

    private AudioSource ads1;
    private AudioSource ads2;

    [SerializeField]
    private GameObject failedOverlay;

    [SerializeField]
    private float timeRemaining = 0.0f;

    bool hasWon = false;

    //public Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        failedOverlay.SetActive(false);
        ads1 = GetComponents<AudioSource>()[1];
        ads2 = GetComponents<AudioSource>()[0];

        if (StaticGameData.isMusicOn)
            ads1.volume = 0.775f;
        else
            ads1.volume = 0;

        if(StaticGameData.isSoundOn)
            ads2.volume = 0.775f;
        else
            ads2.volume = 0;



    }

    // Update is called once per frame
    void Update()
    {
        if(interactions > 0)
        {
            failedOverlay.SetActive(true);
        }

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            //timeText.text = timeRemaining.ToString();
            print(timeRemaining);
        }
        else if(!hasWon)
        {
            GetComponents<AudioSource>()[0].Stop();
            GetComponents<AudioSource>()[1].Stop();
            if(StaticGameData.isSoundOn) GetComponents<AudioSource>()[1].PlayOneShot(StaticGameData.winSoundEffect, 0.5f);
            hasWon = true;
            print("Gagne");                                                                                     //Jeu gagné
            StaticGameData.Game.Points++;
            StartCoroutine(StaticGameData.swapScene());
        }
    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey)
        {
            GetComponents<AudioSource>()[0].Stop();
            GetComponents<AudioSource>()[1].Stop();
            if (StaticGameData.isSoundOn)  GetComponents<AudioSource>()[1].PlayOneShot(StaticGameData.lossSoundEffect, 0.5f);
            interactions++;
            print("Perdu");                                                                                     //Jeu perdu
            print("Interactions : " + interactions + ", KeyCode : " + e.keyCode);
            StaticGameData.isLost = true;
            StartCoroutine(StaticGameData.swapScene());
        }
        if(e.isMouse)
        {

            GetComponents<AudioSource>()[0].Stop();
            GetComponents<AudioSource>()[1].Stop();
            if (StaticGameData.isSoundOn)  GetComponents<AudioSource>()[1].PlayOneShot(StaticGameData.lossSoundEffect, 0.5f);
            interactions++;
            print("Perdu");                                                                                     //Jeu perdu
            print("Interaction : " + interactions + ", Mouse clicked !");
            StaticGameData.isLost = true;
            StartCoroutine(StaticGameData.swapScene());
        }
    }
}
