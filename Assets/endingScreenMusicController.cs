using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endingScreenMusicController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (StaticGameData.isMusicOn)
            GetComponent<AudioSource>().volume = 0.775f;
        else
            GetComponent<AudioSource>().volume = 0;
    }

}
