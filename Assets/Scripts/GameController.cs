using System.Collections.Generic;
using UnityEngine;
using TIBLibrary;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Game game;
    // Start is called before the first frame update
    public AudioClip impact;

    [SerializeField]
    private AudioClip winSoundEffect;
    [SerializeField]
    private AudioClip lossSoundEffect;
    [SerializeField]
    private Sprite noSoundImage;
    [SerializeField]
    private Sprite soundImage;
    void Start()
    {
        if (StaticGameData.isMusicOn)
            GetComponent<AudioSource>().volume = 0.775f;
        else
            GetComponent<AudioSource>().volume = 0;
        Screen.fullScreen = true;
        StaticGameData.Game = StaticGameData.getNewGame();
        StaticGameData.winSoundEffect = this.winSoundEffect;
        StaticGameData.lossSoundEffect = this.lossSoundEffect;
    }

    // Update is called once per frame
    void Update()
    {
        if(StaticGameData.isMusicOn)
            GetComponent<AudioSource>().volume = 0.775f;
        else
            GetComponent<AudioSource>().volume = 0;
    }

    public void onClickStart()
    {
        if(StaticGameData.isSoundOn)GetComponent<AudioSource>().PlayOneShot(impact, 1F);
        SceneManager.LoadScene("LoadingScreen");
        
    }

    public void onClickQuit()
    {
        if (StaticGameData.isSoundOn) GetComponent<AudioSource>().PlayOneShot(impact, 1F);
        Application.Quit(0);
    }

    public void onClickOptions()
    {
        if (StaticGameData.isSoundOn) GetComponent<AudioSource>().PlayOneShot(impact, 1F);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Options");
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Menu");
    }

    public void onClickMenu()
    {
        if (StaticGameData.isSoundOn) GetComponent<AudioSource>().PlayOneShot(impact, 1F);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Options");
    }

    public void onClickSound()
    {
        if (StaticGameData.isSoundOn) GetComponent<AudioSource>().PlayOneShot(impact, 1F);
        if (StaticGameData.isSoundOn)
            StaticGameData.isSoundOn = false;
        else
            StaticGameData.isSoundOn = true;

        if(GameObject.Find("SoundButton").GetComponent<Image>().sprite == soundImage)
            GameObject.Find("SoundButton").GetComponent<Image>().sprite = noSoundImage;
        else
            GameObject.Find("SoundButton").GetComponent<Image>().sprite = soundImage;
    }

    public void onClickMusic()
    {
        if (StaticGameData.isSoundOn) GetComponent<AudioSource>().PlayOneShot(impact, 1F);
        if (StaticGameData.isMusicOn)
            StaticGameData.isMusicOn = false;
        else
            StaticGameData.isMusicOn = true;

        if (GameObject.Find("MusicButton").GetComponent<Image>().sprite == soundImage)
            GameObject.Find("MusicButton").GetComponent<Image>().sprite = noSoundImage;
        else
            GameObject.Find("MusicButton").GetComponent<Image>().sprite = soundImage;
    }
}

public static class StaticGameData{
    public static Game Game { get; set; }
    public static int ActualMinigame { get; set; } = 0;
    public static bool isLost { get; set; } = false;
    public static AudioClip winSoundEffect { get; set; }
    public static AudioClip lossSoundEffect { get; set; }
    public static bool isSoundOn { get; set; } = true;
    public static bool isMusicOn { get; set; } = true;

    public static IEnumerator swapScene(float timeBetweenScene = 0.5f)
    {

        yield return new WaitForSeconds(timeBetweenScene);

        StaticGameData.Game.inMinigame = false;
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(StaticGameData.Game.Minigames[StaticGameData.ActualMinigame].SceneName);
        StaticGameData.ActualMinigame++;

        if (StaticGameData.ActualMinigame >= StaticGameData.Game.Minigames.Count)
        {
            StaticGameData.ActualMinigame = 0;
            Randomizer.ShuffleMinigames<Minigame>(StaticGameData.Game.Minigames);
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("LoadingScreen");
        
    }

    public static Game getNewGame()
    {
        List<string> sceneList = new List<string>();

        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        string[] scenes = new string[sceneCount];


        for (int i = 0; i < sceneCount; i++)
        {
            if (System.IO.Path.GetFullPath(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i)).Contains("Minigames"))
            {
                sceneList.Add(System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i)));
            }
        }


        Game game = new Game(sceneList);

        Randomizer.ShuffleMinigames<Minigame>(game.Minigames);
        return game;
    }
}
