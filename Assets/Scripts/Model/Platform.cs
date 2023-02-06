using System;
using UnityEngine;
using FrogJump.Core;

namespace FrogJump.Model
{
  public enum Movement
  {
    NONE,
    HORIZONTAL,
    VERTICAL,
  }

  public class Platform : MonoBehaviour
  {
    public Guid UniqueId { get; }
    public Movement MovingDirection { get; set; }
    private Rigidbody2D rb;
    private Collider2D col;
    private GameModel model = Util.GetModel<GameModel>();
    [SerializeField] private float moveFrequency;
    [SerializeField] private float moveAmplitude;

    public Platform()
    {
      UniqueId = Guid.NewGuid();
    }

    void Awake()
    {
      rb = GetComponent<Rigidbody2D>();
      col = GetComponent<Collider2D>();
      rb.isKinematic = true;
    }

    void Update()
    {
      float amplitude = UnityEngine.Random.Range(moveAmplitude - 3, moveAmplitude + 3);
      switch (MovingDirection)
      {
        case Movement.HORIZONTAL:
          {
            rb.velocity = new Vector2(MathF.Sin(Time.time * moveFrequency) * amplitude, rb.velocity.y);
            ReturnFromBorders();
            break;
          }
        case Movement.VERTICAL:
          {
            rb.velocity = new Vector2(rb.velocity.x, MathF.Sin(Time.time * moveFrequency) * amplitude);
            break;
          }
        case Movement.NONE:
          {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            break;
          }
      }
    }

    private void ReturnFromBorders()
    {
      // if player leaves border bring them back
      if (col.bounds.max.x > model.CameraBounds.xMax)
      {
        transform.position = new Vector2(model.CameraBounds.xMin, transform.position.y);
      }
      else if (col.bounds.min.x < model.CameraBounds.xMin)
      {
        transform.position = new Vector2(model.CameraBounds.xMax, transform.position.y);
      }
    }
  }
}
