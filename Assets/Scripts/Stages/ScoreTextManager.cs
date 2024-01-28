using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreTextManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    void Start()
    {
        scoreText.text = $"Score: 0";
    }

    public void OnScoreChange(Component sender, object data)
    {
        Debug.Log("Score text changed");
        scoreText.text = $"Score: {data}";
    }
}
