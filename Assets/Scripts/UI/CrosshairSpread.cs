using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairSpread : MonoBehaviour
{
    [SerializeField] private float offSet;
    private Vector3 originalPosition;
     void Start()
    {
        originalPosition = transform.localPosition;
    }
    
    public void SpreadCrosshair(Component sender, object data)
    {
        float spread = (float)data;
        transform.localPosition = new Vector3(Mathf.Min(originalPosition.x - spread * offSet, originalPosition.x), 
            originalPosition.y, originalPosition.z);
    }
}
