using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace FrogJump.Editor
{
  public class RemoveWebGLSupportWarning
  {
    private const string TEXT_TO_COMMENT_OUT = "unityShowBanner('WebGL builds are not supported on mobile devices.');";

    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget target, string targetPath)
    {
      if (target != BuildTarget.WebGL)
      {
        return;
      }

      var info = new DirectoryInfo(targetPath);
      var files = info.GetFiles("index.html");
      for (int i = 0; i < files.Length; i++)
      {
        var file = files[i];
        var filePath = file.FullName;
        var text = File.ReadAllText(filePath);
        text = text.Replace(TEXT_TO_COMMENT_OUT, "//" + TEXT_TO_COMMENT_OUT);

        Debug.Log("Removing mobile warning from " + filePath);
        File.WriteAllText(filePath, text);
      }
    }
  }
}