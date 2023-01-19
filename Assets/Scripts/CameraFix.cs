using UnityEngine;

[ExecuteInEditMode]
public class CameraFix : MonoBehaviour
{
  [Range(1, 4)]
  public int pixelScale = 1;

  [Range(1, 100)]
  public int pixelsPerUnit = 100;

  private Camera _camera;

  // Update is called once per frame
  void Update()
  {
    if (_camera == null)
    {
      _camera = GetComponent<Camera>();
      _camera.orthographic = true;
    }
    _camera.orthographicSize = Screen.height * ((0.5f / pixelsPerUnit) / pixelScale);
  }
}
