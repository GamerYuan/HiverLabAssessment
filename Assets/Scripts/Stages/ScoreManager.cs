using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField] private GameEvent OnScoreChange;

    private int score = 0;

    private int Score 
    { get => score; set
        {
            score = Mathf.Max(value, 0);
            OnScoreChange.Raise(this, score);
            Debug.Log("Score changed");
        }
    }

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
        DontDestroyOnLoad(this);
    }

    public void OnScoreAdd(Component sender, object data)
    {
        if (sender is not IScorable)
        {
            Debug.LogError("Sender is not IScorable");
            return;
        }
        int scoreToAdd = (int)data;
        Score += scoreToAdd;
    }

    public void ResetScore()
    {
        Score = 0;
    }

    public int GetScore()
    {
        return Score;
    }

}
