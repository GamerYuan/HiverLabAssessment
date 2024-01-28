using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float cameraSpeed;

    private Vector2 input;

    private float xRot, yRot;

    void Update()
    {
        ApplyLook();
    }

    void ApplyLook()
    {
        xRot -= input.y;
        xRot = Mathf.Clamp(xRot, -90f, 90f);
        yRot += input.x;

        transform.localRotation = Quaternion.Euler(xRot, yRot, 0.0f);
    }

    public void Look(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }
}
