using UnityEngine;

namespace FrogJump.Core
{
  public static partial class Util
  {
    static public T GetModel<T>() where T : class, new()
    {
      return InstanceRegister<T>.instance;
    }

    public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
    {
      position.z = camera.nearClipPlane;
      return camera.ScreenToWorldPoint(position);
    }
  }
}