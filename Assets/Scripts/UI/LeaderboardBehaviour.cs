using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject contentPanel;
    [SerializeField] private GameObject entryPrefab;

    private static string LeaderboardAPI = "https://hiverlab-assessment-backend.yuan0801.workers.dev/";
    private static int EntryHeight = 120;

    private List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
    private List<GameObject> entryObjects = new List<GameObject>();
    private bool isUpdating = false;
    private WaitForSeconds updateInterval = new WaitForSeconds(0.1f);

    [System.Serializable]
    public class LeaderboardEntry
    {
        public string name;
        public int score;
    }

    [System.Serializable]
    public class RootObject
    {
        public LeaderboardEntry[] leaderboardEntries;
    }

    private void OnEnable()
    {
        ViewLeaderboard();
    }

    private void OnDisable() 
    {
        UnviewLeaderboard();
    }

    private void ViewLeaderboard()
    {
        Debug.Log("Viewing Leaderboard");
        isUpdating = true;
        StartCoroutine(UpdateLeaderboard());
    }

    private void UnviewLeaderboard()
    {
        Debug.Log("Not viewing Leaderboard");
        isUpdating = false;
        StopAllCoroutines();
    }

    IEnumerator UpdateLeaderboard()
    {
        Debug.Log("Updating leaderboard");
        entries.Clear();
        yield return FetchLeaderboard();
        if (entries.Count > 0)
        {
            UpdateContentSize(entries.Count);
        }

        entryObjects.ForEach(Destroy);
        foreach (LeaderboardEntry entry in entries)
        {
            GameObject entries = Instantiate(entryPrefab, contentPanel.transform);
            entries.GetComponent<LeaderboardEntryBehaviour>().SetEntry(entry);
            entryObjects.Add(entries);
        }
    }

    IEnumerator FetchLeaderboard()
    {
        Debug.Log("Fetching leaderboard data from API");
        using (UnityWebRequest webRequest = UnityWebRequest.Get(LeaderboardAPI + "highscores"))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Network Error: " + webRequest.error);

            }
            else
            {
                Debug.Log("Web request successful");
                
                RootObject json = JsonUtility.FromJson<RootObject>(
                    "{\"leaderboardEntries\":" + webRequest.downloadHandler.text + "}");
                Debug.Log(webRequest.downloadHandler.text);
                foreach (var item in json.leaderboardEntries)
                {
                    entries.Add(item);
                }
            }
            webRequest.Dispose();
        }
    }

    void UpdateContentSize(int entries)
    {
        contentPanel.GetComponent<RectTransform>().
            SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 60 + entries * EntryHeight);
    }
}
