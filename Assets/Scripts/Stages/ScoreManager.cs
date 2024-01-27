using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    
    private int score = 0;

    void Start()
    {
        scoreText.text = $"Score: {score}";
    }

    public void OnScoreChange(Component sender, object data)
    {
        if (sender is not IScorable)
        {
            Debug.LogError("Sender is not IScorable");
            return;
        }
        int scoreToAdd = (int)data;
        score = Mathf.Max(score + scoreToAdd, 0);
        Debug.Log("Score changed");
        scoreText.text = $"Score: {score}";
    }
}
