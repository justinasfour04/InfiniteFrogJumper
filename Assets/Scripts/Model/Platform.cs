using System;
using UnityEngine;

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
    [SerializeField] private float moveFrequency;
    [SerializeField] private float moveAmplitude;

    public Platform()
    {
      UniqueId = Guid.NewGuid();
    }

    void Awake()
    {
      rb = GetComponent<Rigidbody2D>();
      rb.isKinematic = true;
    }

    void Update()
    {
      float amplitude = UnityEngine.Random.Range(moveAmplitude - 5, moveAmplitude + 5);
      switch (MovingDirection)
      {
        case Movement.HORIZONTAL:
          {
            rb.velocity = new Vector2(MathF.Sin(Time.time * moveFrequency) * amplitude, rb.velocity.y);
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
  }
}
