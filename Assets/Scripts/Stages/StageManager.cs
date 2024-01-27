using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

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
    }

    private void EndStage()
    {
        Debug.Log("Ending stage");
        Time.timeScale = 0f;
    }

    public void OnPlayerDie(Component sender, object data)
    {
        EndStage();
    }
}
