using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public bool isStageActive;

    [SerializeField] private int stageTimer;
    [SerializeField] private int rampStages;
    [SerializeField] private TMP_Text timerText;

    private float rampTimer;
    private int currRamp = 0;
    readonly WaitForSeconds timer = new WaitForSeconds(1);

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
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        rampTimer = (float)stageTimer / rampStages;
        StartCoroutine(StageTimer());
        ScoreManager.instance.ResetScore();
    }

    private void EndStage()
    {
        Debug.Log("Ending stage");
        Cursor.lockState = CursorLockMode.None;
        isStageActive = false;
        Time.timeScale = 0f;
        TransitionManager.instance.GoToLevel("EndMenu");
    }

    public void OnPlayerDie(Component sender, object data)
    {
        EndStage();
    }

    private IEnumerator StageTimer()
    {
        for (int i = stageTimer; i >= 0; i--)
        {
            timerText.text = $"Timer: {i}";
            if (Mathf.Floor(stageTimer - i - rampTimer) > currRamp)
            {
                currRamp++;
                DifficultyManager.IncreaseDifficulty(currRamp * rampTimer / stageTimer);
            }
            yield return timer;
        }
        EndStage();
        yield return null;
    }

}
