using UnityEngine;

[ExecuteInEditMode]
public class CameraFix : MonoBehaviour
{
  [Range(1f, 4f)]
  public float pixelScale = 1f;

  [Range(1, 100)]
  public int pixelsPerUnit = 100;

  private Camera _camera;

  void Awake()
  {
    Screen.autorotateToLandscapeLeft = false;
    Screen.autorotateToLandscapeRight = false;
    Screen.autorotateToPortrait = true;
    Screen.autorotateToPortraitUpsideDown = false;
  }

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
