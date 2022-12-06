using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimonController : MonoBehaviour
{
    [SerializeField]
    private GameObject TileContainer;
    [SerializeField]
    private int difficulty = 5;

    System.Random rdn = new System.Random();

    private GameObject[] tiles = new GameObject[4];

    private Sprite[] darkColors = new Sprite[4];
    private Sprite[] lightColors = new Sprite[4];

    private List<int> sequence = new List<int>();
    private bool sequenceFase = true;
    private bool gameIsEnd = false;
    private int actualSequenceItem = 0;

    // Start is called before the first frame update
    void Start()
    {
        StaticGameData.Game.inMinigame = true;

        ///Initializing the colors
        darkColors[0] = Resources.Load<Sprite>("simon/dark_green") as Sprite;
        darkColors[1] = Resources.Load<Sprite>("simon/dark_red") as Sprite;
        darkColors[2] = Resources.Load<Sprite>("simon/dark_blue") as Sprite;
        darkColors[3] = Resources.Load<Sprite>("simon/dark_yellow") as Sprite;

        lightColors[0] = Resources.Load<Sprite>("simon/light_green") as Sprite;
        lightColors[1] = Resources.Load<Sprite>("simon/light_red") as Sprite;
        lightColors[2] = Resources.Load<Sprite>("simon/light_blue") as Sprite;
        lightColors[3] = Resources.Load<Sprite>("simon/light_yellow") as Sprite;

        ///Fetching the gameobjects
        for (int i = 0; i < 4; i++)
        {
            tiles[i] = TileContainer.transform.GetChild(i).gameObject;
            tiles[i].GetComponent<Image>().sprite = darkColors[i];

            tiles[i].name = i.ToString();
        }

        tiles[0].GetComponent<Button>().onClick.AddListener(() => { onClickListener(tiles[0]); });
        tiles[1].GetComponent<Button>().onClick.AddListener(() => { onClickListener(tiles[1]); });
        tiles[2].GetComponent<Button>().onClick.AddListener(() => { onClickListener(tiles[2]); });
        tiles[3].GetComponent<Button>().onClick.AddListener(() => { onClickListener(tiles[3]); });

        ///Preparing the sequence
        for (int i = 0; i <= difficulty; i++)
        {
            int sequenceItem = rdn.Next(0, 4);
            sequence.Add(sequenceItem);
        }

        if (StaticGameData.isMusicOn)
            GetComponent<AudioSource>().volume = 0.775f;
        else
            GetComponent<AudioSource>().volume = 0;


    }

    private void resetColors()
    {
        for (int i = 0; i < 4; i++)
        {
            tiles[i].GetComponent<Image>().sprite = darkColors[i];
        }
    }

    public void onClickListener(GameObject button)
    {
        if (!gameIsEnd)
        {
            if (sequenceFase == false)
            {
                resetColors();
                button.GetComponent<Image>().sprite = lightColors[int.Parse(button.name)];
                if (actualSequenceItem == 5)
                {
                    goodEnd();
                    StartCoroutine(StaticGameData.swapScene());
                }

                if (sequence[actualSequenceItem] == int.Parse(button.name))
                {
                    actualSequenceItem++;
                }
                else
                {
                    StaticGameData.isLost = true;
                    badEnd();
                    StartCoroutine(StaticGameData.swapScene());
                }
            }
        }
    }

    private void badEnd()
    {
        GetComponent<AudioSource>().Stop();
        if(StaticGameData.isSoundOn) GetComponent<AudioSource>().PlayOneShot(StaticGameData.lossSoundEffect, 0.5f);
        gameIsEnd = true;
        tiles[0].GetComponent<Image>().sprite = lightColors[1];
        tiles[1].GetComponent<Image>().sprite = lightColors[1];
        tiles[2].GetComponent<Image>().sprite = lightColors[1];
        tiles[3].GetComponent<Image>().sprite = lightColors[1];
    }

    private void goodEnd()
    {
        GetComponent<AudioSource>().Stop();
        if (StaticGameData.isSoundOn) GetComponent<AudioSource>().PlayOneShot(StaticGameData.winSoundEffect, 0.5f);
        StaticGameData.Game.Points++;
        gameIsEnd = true;
        tiles[0].GetComponent<Image>().sprite = lightColors[0];
        tiles[1].GetComponent<Image>().sprite = lightColors[0];
        tiles[2].GetComponent<Image>().sprite = lightColors[0];
        tiles[3].GetComponent<Image>().sprite = lightColors[0];
    }

    private float elapsed = 0f;
    void Update()
    {
        if (sequenceFase)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= 1f)
            {
                elapsed = elapsed % 1f;
                ReadSequence();
            }
        }
    }

    private int sequenceNumber = 0;
    private void ReadSequence()
    {
        resetColors();
        if (sequenceNumber < sequence.Count)
        {
            switch (sequence[sequenceNumber])
            {
                case 0:
                    tiles[0].GetComponent<Image>().sprite = lightColors[0];
                    break;
                case 1:
                    tiles[1].GetComponent<Image>().sprite = lightColors[1];
                    break;
                case 2:
                    tiles[2].GetComponent<Image>().sprite = lightColors[2];
                    break;
                case 3:
                    tiles[3].GetComponent<Image>().sprite = lightColors[3];
                    break;
            }
            sequenceNumber++;
        }
        else
        {
            sequenceFase = false;
        }
    }
}
