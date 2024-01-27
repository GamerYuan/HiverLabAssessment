using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public bool isStageActive;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        isStageActive = true;
    }

    private void EndStage()
    {
        Debug.Log("Ending stage");
        isStageActive = false;
        Time.timeScale = 0f;
    }

    public void OnPlayerDie(Component sender, object data)
    {
        EndStage();
    }

}
