using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardEntryBehaviour : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText, scoreText;
    
    public void SetEntry(LeaderboardBehaviour.LeaderboardEntry entry)
    {
        nameText.text = entry.name;
        scoreText.text = entry.score.ToString();
    }
}
