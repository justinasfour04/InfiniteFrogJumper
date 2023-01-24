using UnityEngine;

namespace FrogJump.Model
{
  [System.Serializable]
  public class GameModel
  {
    public Rect CameraBounds 
    {
      get {
        var camera = Camera.main;

        Vector3 bottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        Vector3 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));

        return new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
      }
    }
  }
}