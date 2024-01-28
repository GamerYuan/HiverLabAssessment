using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    private float initialWidth;

    void Awake()
    {
        initialWidth = healthBar.rectTransform.sizeDelta.x;    
    }

    public void OnHealthChange(Component sender, object data)
    {
        if (sender is not IHealth)
        {
            Debug.LogError("Sender is not IHealth");
            return;
        }
        float health = Mathf.Clamp((float)data, 0f, 1f);
        healthBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, initialWidth * health);
    }
}
