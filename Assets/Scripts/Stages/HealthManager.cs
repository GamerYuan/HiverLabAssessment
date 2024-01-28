using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;

    private float currHealth = 0;

    public void Start()
    {
        healthText.text = $"Health: {currHealth}";
    }

    public void OnHealthChange(Component sender, object data)
    {
        if (sender is not IHealth)
        {
            Debug.LogError("Sender is not IHealth");
            return;
        }
        currHealth = (float)data;
        healthText.text = $"Health: {currHealth}";
    }
}
