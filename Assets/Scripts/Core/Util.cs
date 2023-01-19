namespace FrogJump.Core
{
  class Util
  {
    public static void Swap<T>(ref T left, ref T right)
    {
      T temp = left;
      left = right;
      right = temp;
    }
  }
}