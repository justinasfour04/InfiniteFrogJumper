using FrogJump.Core;
using FrogJump.Model;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FrogJump.Mechanics
{
  public class PlayerController : MonoBehaviour
  {
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2d;
    private Animator animator;
    private PlatformController platformController;
    private GameModel model = Util.GetModel<GameModel>();
    private UIManager uiManager;
    [SerializeField] private LayerMask ground;
    private float maxSpeed = 5;
    [SerializeField] private float jumpForce;

    private void Awake()
    {
      rb = GetComponent<Rigidbody2D>();
      spriteRenderer = GetComponent<SpriteRenderer>();
      animator = GetComponent<Animator>();
      boxCollider2d = GetComponent<BoxCollider2D>();
      uiManager = GetComponent<UIManager>();
      platformController = GameObject.FindGameObjectWithTag("Grid").GetComponent<PlatformController>();
    }

    void Start()
    {
      rb.freezeRotation = true;
      transform.position = new Vector2(Screen.safeArea.xMin, Screen.safeArea.yMin + 2);
    }

    private void Update()
    {
      float dirX = 0;
      if (Input.touchSupported)
      {
        Touch singleFingerTouch = Input.touches[0];
        switch (singleFingerTouch.phase)
        {
          case TouchPhase.Moved:
            {
              var delta = singleFingerTouch.deltaPosition;
              dirX = delta.x > 0 ? -1 : 1;
              maxSpeed = 10;
              rb.velocity = new Vector2(maxSpeed * dirX, rb.velocity.y);
              break;
            }
        }
      }
      else
      {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(maxSpeed * dirX, rb.velocity.y);
      }

      var grounded = isGrounded();

      // Jump off ground
      if (grounded && rb.velocity.y <= 0)
      {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
      }

      KeepPlayerCentered();
      ReturnFromBorders();
      UpdateAnimation(dirX, grounded);

      if (transform.position.y < model.CameraBounds.yMin)
      {
        SceneManager.LoadScene(0);
      }
    }

    private void KeepPlayerCentered()
    {
      // Move platforms and keep player centered
      var middleScene = (model.CameraBounds.yMax - model.CameraBounds.yMin) / 2;
      if (transform.position.y > middleScene && rb.velocity.y > 0)
      {
        platformController.MoveDown(rb.velocity.y);
        transform.position = new Vector2(transform.position.x, middleScene);
      }
    }

    private void ReturnFromBorders()
    {
      // if player leaves border bring them back
      if (transform.position.x > model.CameraBounds.xMax)
      {
        transform.position = new Vector2(model.CameraBounds.xMin, transform.position.y);
      }
      else if (transform.position.x < model.CameraBounds.xMin)
      {
        transform.position = new Vector2(model.CameraBounds.xMax, transform.position.y);
      }
    }

    private void UpdateAnimation(float dirX, bool grounded)
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

    private bool isGrounded()
    {
      RaycastHit2D collision = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, 0.1f, ground);
      Platform platform = collision.collider?.gameObject.GetComponent<Platform>();
      if (platform != null)
      {
        Guid uniqueId = platform.UniqueId;
        if (!platformController.IsCollisionWith(uniqueId))
        {
          platformController.SetCollisionOfPlatform(uniqueId);
          uiManager.AddScore();
        }
      }

      return collision;
    }

    private enum MovementState
    {
      Idle,
      Jumping,
    }
  }
}