using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrogJump.Mechanics
{
  public class PlayerController : MonoBehaviour
  {
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2d;
    private Animator animator;
    private PlatformController platformController;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpForce;

    private void Awake()
    {
      rb = GetComponent<Rigidbody2D>();
      spriteRenderer = GetComponent<SpriteRenderer>();
      animator = GetComponent<Animator>();
      boxCollider2d = GetComponent<BoxCollider2D>();
      platformController = GameObject.FindGameObjectWithTag("Grid").GetComponent<PlatformController>();
    }

    void Start()
    {
      rb.freezeRotation = true;
    }

    private void Update()
    {
      var dirX = Input.GetAxisRaw("Horizontal");
      rb.velocity = new Vector2(maxSpeed * dirX, rb.velocity.y);

      var grounded = isGrounded();

      // Jump off ground
      if (grounded && rb.velocity.y <= 0)
      {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
      }

      // Move platforms and keep player centered
      var middleScene = (platformController.CameraBounds.yMax - platformController.CameraBounds.yMin) / 2;
      if (transform.position.y > middleScene && rb.velocity.y > 0)
      {
        platformController.MoveDown(rb.velocity.y);
        transform.position = new Vector2(transform.position.x, middleScene);
      }

      // if player leaves border bring them back
      if (transform.position.x > platformController.CameraBounds.xMax)
      {
        transform.position = new Vector2(platformController.CameraBounds.xMin, transform.position.y);
      }
      else if (transform.position.x < platformController.CameraBounds.xMin)
      {
        transform.position = new Vector2(platformController.CameraBounds.xMax, transform.position.y);
      }

      updateAnimation(dirX, grounded);
    }

    private void updateAnimation(float dirX, bool grounded)
    {
      MovementState state;
      if (Math.Abs(dirX) > 0)
      {
        spriteRenderer.flipX = dirX == -1 ? true : false;
      }

      if (rb.velocity.y > 0.01f)
      {
        state = MovementState.Jumping;
      }
      else
      {
        state = MovementState.Idle;
      }

      animator.SetInteger("state", (int)state);
    }

    private bool isGrounded() => Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, 0.1f, ground);

    private enum MovementState
    {
      Idle,
      Jumping,
    }
  }
}