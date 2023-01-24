using UnityEngine;

namespace FrogJump.Core
{
  public static partial class Util
  {
    static class InstanceRegister<T> where T : class, new()
    {
      public static T instance = new T();
    }
  }
}