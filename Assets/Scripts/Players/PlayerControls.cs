using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviourPun
{
    [Header("Movement Settings")]
    [SerializeField] private Transform spriteContainer;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("UI")]
    [SerializeField] private TMP_Text nameText;

    private float moveInput;
    private float verticalVelocity;
    private bool isGrounded = true;

    public bool canMove = false;

    private PlayerInputActions inputs;

    private void Awake()
    {
        if (nameText != null)
            nameText.text = photonView.Owner.NickName;

        if (photonView.IsMine)
            inputs = new PlayerInputActions();
    }

    private void OnEnable()
    {
        if (!photonView.IsMine) return;

        inputs.Player.Enable();
        inputs.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        if (!photonView.IsMine) return;

        inputs.Player.Jump.performed -= OnJump;
        inputs.Player.Disable();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

            if (!canMove) return;
            HandleMovement();
            HandleFlip();
        }
    }

    private void HandleMovement()
    {
        moveInput = inputs.Player.Move.ReadValue<float>();

        transform.position += new Vector3(moveInput * moveSpeed, 0f, 0f) * Time.deltaTime;

        if (!isGrounded)
            verticalVelocity += gravity * Time.deltaTime;

        transform.position += new Vector3(0f, verticalVelocity, 0f) * Time.deltaTime;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        if (isGrounded && verticalVelocity < 0f)
            verticalVelocity = 0f;
    }

    private void HandleFlip()
    {
        if (moveInput > 0.01f)
            spriteContainer.localScale = new Vector3(1, 1, 1);
        else if (moveInput < -0.01f)
            spriteContainer.localScale = new Vector3(-1, 1, 1);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!photonView.IsMine) return;
        if (!canMove) return;

        if (context.performed && isGrounded)
        {
            verticalVelocity = jumpForce;
            isGrounded = false;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
#endif
}
