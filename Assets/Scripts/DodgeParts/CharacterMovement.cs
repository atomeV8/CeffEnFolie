using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private float characterSpeed = 5;
    Vector3 move;
    public bool hasLost = false;
    public bool hasWon = false;


    void Update()
    {
        if (!hasLost && !hasWon)
        {
            float moveLength = characterSpeed * Time.deltaTime;

            if (Input.GetKey(KeyCode.UpArrow) && transform.position.y < 4)
            {
                transform.position += new Vector3(0, moveLength, 0);
            }
            if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > -8.35)
            {
                transform.position += new Vector3(-moveLength, 0, 0);
            }
            if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < 8.35)
            {
                transform.position += new Vector3(moveLength, 0, 0);
            }
            if (Input.GetKey(KeyCode.DownArrow) && transform.position.y > -4 )
            {
                transform.position += new Vector3(0, -moveLength, 0);
            }

            transform.position += move;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasLost && !hasWon)
        {
            AudioSource ads = GameObject.Find("GameController").GetComponent<AudioSource>();
            ads.Stop();
            if(StaticGameData.isSoundOn) ads.PlayOneShot(StaticGameData.lossSoundEffect, 0.5f);
            hasLost = true;
            StaticGameData.isLost = true;
            StartCoroutine(StaticGameData.swapScene());
        }
    }
}
