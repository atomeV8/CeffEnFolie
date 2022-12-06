using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerDP : MonoBehaviour
{

    //Objects
    GameObject[] gameObjects = new GameObject[10];
    GameObject character;
    //Parts
    List<Part> parts;
    private int currentPart;
    //Timer management
    private float tickTimer = 0;
    private const float TICK_LENGTH = 1f;
    private int current_tick_count = 0;

    CharacterMovement cm;
    bool hasWon = false;
    GameObject instructions;
    //Parts
    [SerializeField]
    float launchSpeed = 100;



    // Start is called before the first frame update
    void Start()
    {
        current_tick_count=0;
        cm = GameObject.Find("Character").GetComponent<CharacterMovement>();
        parts = new List<Part>();
        instructions = GameObject.Find("Canvas/Instructions");
        character = GameObject.Find("Character");
        //Fetching game objects
        for (int x = 0; x < 10; x++)
        {
            gameObjects[x] = GameObject.Find("Part" + (x+1));
        }
        //Initializing array with data
        parts.Add(new Part(-10, -6, gameObjects[0]));
        parts.Add(new Part(-10, 6, gameObjects[1]));
        parts.Add(new Part(10, 0, gameObjects[2]));
        parts.Add(new Part(0, 6, gameObjects[3]));
        parts.Add(new Part(10, 6, gameObjects[4]));
        parts.Add(new Part(10, 0, gameObjects[5]));
        parts.Add(new Part(-4, -8, gameObjects[6]));
        parts.Add(new Part(-10, -3, gameObjects[7]));
        parts.Add(new Part(10, -5, gameObjects[8]));

        if (StaticGameData.isMusicOn)
            GetComponent<AudioSource>().volume = 0.775f;
        else
            GetComponent<AudioSource>().volume = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (!cm.hasLost)
        {
            tickTimer += Time.deltaTime;
            if (tickTimer > TICK_LENGTH && parts.Count > 0)
            {
                current_tick_count++;
                if (current_tick_count > 2)
                {
                    instructions.SetActive(false);
                    currentPart = Random.Range(0, parts.Count);
                    print(current_tick_count);
                    parts[currentPart].Launch(character, launchSpeed);
                    parts.RemoveAt(currentPart);
                }
                tickTimer = 0;
            }
            else if (parts.Count == 0 && !hasWon)
            {
                hasWon = true;
                AudioSource ads = GetComponent<AudioSource>();
                ads.Stop();
                if (StaticGameData.isSoundOn) ads.PlayOneShot(StaticGameData.winSoundEffect, 0.5f);
                GameObject.Find("Character").GetComponent<CharacterMovement>().hasWon = true;
                //Mini-jeu gagn�
                //VICTOIRE
                StaticGameData.Game.Points++;
                StartCoroutine(StaticGameData.swapScene());
            }
        }
    }

    
}
class Part
{
    int positionX;
    int positionY;
    GameObject gameObject;

    public Part(int positionX, int positionY, GameObject gameObject)
    {
        this.positionX = positionX;
        this.positionY = positionY;
        this.gameObject = gameObject;
    }

    public void Launch(GameObject part, float speed) {
        
        Rigidbody2D rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        float x = part.transform.position.x;
        float y = part.transform.position.y;
        rigidbody.velocity = new Vector2((x-gameObject.transform.position.x)*speed*Time.deltaTime, (y - gameObject.transform.position.y)*speed * Time.deltaTime);
    }
}