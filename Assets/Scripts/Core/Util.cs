using UnityEngine;

namespace FrogJump.Core
{
  public static partial class Util
  {
    static public T GetModel<T>() where T : class, new()
    {
      return InstanceRegister<T>.instance;
    }
  }
}