using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        TransitionManager.instance.StartGame();
    }

    public void QuitGame()
    {
        TransitionManager.instance.QuitGame();
    }
}
