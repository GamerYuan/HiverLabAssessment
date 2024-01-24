using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private float moveSpeed, jumpHeight;

    private CharacterController controller;
    private Vector3 velocity, move;
    private float gravity = -9.81f;
    private bool grounded;

    private Vector2 input;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // isGrounded does not work if Min Move Distance != 0
        grounded = controller.isGrounded;

        ApplyMove();
        ApplyGravity();
    }

    void ApplyMove()
    {
        controller.Move(move * Time.deltaTime * moveSpeed);
    }

    void ApplyGravity()
    {
        if (grounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        move = head.right * input.x + head.forward * input.y;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!grounded) return;

        velocity.y += jumpHeight;
    }
}
