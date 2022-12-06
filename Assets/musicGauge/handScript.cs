using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handScript : MonoBehaviour
{
    private AudioSource music;

    [SerializeField]
    private KeyCode space;

    [SerializeField]
    private GameObject hand;

    private float rotation = 0;

    bool hasLost = false;
    bool hasWon = false;

    bool rightToLeft = true;

    [SerializeField]
    private float timer = 5;

    [SerializeField]
    private int speed = 200;

    [SerializeField]
    private int green = 20;

    AudioSource ads;

    // Start is called before the first frame update
    void Start()
    {
        ads = GetComponent<AudioSource>();
        if (StaticGameData.isMusicOn)
           ads.volume = 0.775f;
        else
           ads.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!hasWon && !hasLost)
        {
            if (timer < 0 && !hasLost)
            {
                hasLost = true;
                ads.Stop();
                if(StaticGameData.isSoundOn) ads.PlayOneShot(StaticGameData.lossSoundEffect, 0.5f);                                                                                     //Perdu
                StaticGameData.isLost = true;
                StartCoroutine(StaticGameData.swapScene());
            }
            else
            {
                timer -= Time.deltaTime;
            }

            if(Input.GetKey(space) && !hasWon)
            {
                hand.transform.Rotate(0f, 0f, 0f);
                if(rotation > (-green) && rotation < green)
                {
                    hasWon = true;
                    ads.Stop();
                    if (StaticGameData.isSoundOn) ads.PlayOneShot(StaticGameData.winSoundEffect, 0.5f);
                    StaticGameData.Game.Points++;
                    StartCoroutine(StaticGameData.swapScene());
                }
            }
            else if(!hasWon)
            {
                if(rightToLeft)
                {
                    float rotationAmount = speed * Time.deltaTime;
                    hand.transform.Rotate(0f, 0f, -rotationAmount);
                    rotation -= rotationAmount;
                    if(rotation < -90)
                    {
                        rightToLeft = false;
                    }
                }
                else
                {
                    float rotationAmount = speed * Time.deltaTime;
                    hand.transform.Rotate(0f, 0f, rotationAmount);
                    rotation += rotationAmount;
                    if (rotation > 90)
                    {
                        rightToLeft = true;
                    }
                }
            }
        }
    }
}
