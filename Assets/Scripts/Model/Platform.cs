using System;
using UnityEngine;

namespace FrogJump.Model
{
  public class Platform : MonoBehaviour
  {
    public Guid UniqueId { get; }

    public Platform()
    {
      UniqueId = Guid.NewGuid();
    }
  }
}
