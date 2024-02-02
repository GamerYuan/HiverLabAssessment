using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class ZombieBlood : MonoBehaviour
{
    private VisualEffect visualEffect;

    static readonly ExposedProperty triggerEvent = "OnPlay";

    private void Start()
    {
        visualEffect = GetComponent<VisualEffect>();
    }

    public void PlayParticle()
    {
        Debug.Log($"{transform.GetInstanceID()}: Playing blood particle");
        visualEffect.SendEvent(triggerEvent);
    }
}
