using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrogJump.Mechanics
{
  public class FloorController : MonoBehaviour
  {
    // Start is called before the first frame update
    void Start()
    {
      transform.position = new Vector2(Screen.safeArea.center.x, Screen.safeArea.yMin);
    }
  }
}
