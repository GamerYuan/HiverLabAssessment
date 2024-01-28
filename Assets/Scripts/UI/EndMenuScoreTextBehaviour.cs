using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndMenuScoreTextBehaviour : MonoBehaviour
{
    private TMP_Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<TMP_Text>();
        scoreText.text = "Score: " + ScoreManager.instance.GetScore();
    }
}
