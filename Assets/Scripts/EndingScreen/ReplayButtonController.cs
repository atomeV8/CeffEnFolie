using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayButtonController : MonoBehaviour
{
    public void onClickReplay()
    {
        StaticGameData.isLost = false;
        StaticGameData.Game.Points = 0;

        StaticGameData.Game = StaticGameData.getNewGame();

        UnityEngine.SceneManagement.SceneManager.LoadScene("LoadingScreen");
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("EndingScreen");

    }

}
