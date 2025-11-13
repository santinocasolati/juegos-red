using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviourPun
{
    [Header("Character Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private List<RuntimeAnimatorController> animations;

    [Header("Movement Settings")]
    [SerializeField] private Transform spriteContainer;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform headCheck;
    [SerializeField] private float headRadius = 0.2f;
    [SerializeField] private Transform leftCheck;
    [SerializeField] private Transform rightCheck;
    [SerializeField] private Vector2 wallSize;

    [Header("UI")]
    [SerializeField] private TMP_Text nameText;

    private float moveInput;
    private float verticalVelocity;
    private bool isGrounded = true;
    private bool hitLeftWall = false;
    private bool hitRightWall = false;

    public bool canMove = false;

    private PlayerInputActions inputs;

    private void Awake()
    {
        if (nameText != null)
            nameText.text = photonView.Owner.NickName;

        if (photonView.IsMine)
            inputs = new PlayerInputActions();

        if (photonView.Owner.CustomProperties.TryGetValue("characterIndex", out object index))
        {
            animator.runtimeAnimatorController = animations[(int)index];
        }
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

            animator.SetBool("Floating", !isGrounded);

            if (Physics2D.OverlapCircle(headCheck.position, headRadius, groundLayer) && verticalVelocity > 0)
            {
                verticalVelocity = 0;
            }

            hitLeftWall = Physics2D.OverlapBox(leftCheck.position, wallSize, 0f, groundLayer) != null;
            hitRightWall = Physics2D.OverlapBox(rightCheck.position, wallSize, 0f, groundLayer) != null;

            if (!canMove) return;
            HandleMovement();
            HandleFlip();
        }
    }

    private void HandleMovement()
    {
        moveInput = inputs.Player.Move.ReadValue<float>();

        animator.SetBool("Walking", moveInput != 0);

        Vector3 deltaX = new Vector3(moveInput * moveSpeed * Time.deltaTime, 0f, 0f);

        if ((moveInput > 0 && hitRightWall) || (moveInput < 0 && hitLeftWall))
            deltaX.x = 0f;

        transform.position += deltaX;

        if (!isGrounded)
            verticalVelocity += gravity * Time.deltaTime;

        transform.position += new Vector3(0f, verticalVelocity, 0f) * Time.deltaTime;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        if (isGrounded && verticalVelocity < 0f)
            verticalVelocity = 0f;
    }

    private void HandleFlip()
    {
        Vector3 currentScale = spriteContainer.localScale;

        if (moveInput > 0.01f && currentScale.x < 0)
        {
            spriteContainer.localScale = new Vector3(1, 1, 1);
            photonView.RPC("RPC_FlipSprite", RpcTarget.Others, 1f);
        }
        else if (moveInput < -0.01f && currentScale.x > 0)
        {
            spriteContainer.localScale = new Vector3(-1, 1, 1);
            photonView.RPC("RPC_FlipSprite", RpcTarget.Others, -1f);
        }
    }

    [PunRPC]
    private void RPC_FlipSprite(float direction)
    {
        spriteContainer.localScale = new Vector3(direction, 1, 1);
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
        Gizmos.color = Color.red;

        if (groundCheck != null)
        {
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }

        if (headCheck != null)
        {
            Gizmos.DrawWireSphere(headCheck.position, headRadius);
        }

        if (leftCheck != null)
        {
            Gizmos.DrawWireCube(leftCheck.position, wallSize);
        }

        if (rightCheck != null)
        {
            Gizmos.DrawWireCube(rightCheck.position, wallSize);
        }
    }
#endif
}
