using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreUploadManager : MonoBehaviour
{
    private static string LeaderboardAPI = "https://hiverlab-assessment-backend.yuan0801.workers.dev/";

    [SerializeField] TMP_InputField nameInputField;

    public void UploadScore()
    {
        StartCoroutine(UploadScoreCoroutine(nameInputField.text, ScoreManager.instance.GetScore()));
    }

    private IEnumerator UploadScoreCoroutine(string name, int score)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(LeaderboardAPI + "newscore?name=" + name + "&score=" + score,
            "", "text/plain;charset=UTF-8"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Error uploading score: " + webRequest.error);
            }
            else
            {
                Debug.Log("Upload score successful");
            }
            webRequest.Dispose();
        }
        TransitionManager.instance.GoToLevel("MainMenu");
        yield return null;
    }
}
