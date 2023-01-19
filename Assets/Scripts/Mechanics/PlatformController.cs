using System;
using System.Collections;
using UnityEngine;

namespace FrogJump.Mechanics
{
  class PlatformComparer : IComparer
  {
    public int Compare(object x, object y)
    {
      var a = (int)x;
      var b = (int)y;
      return a.CompareTo(b);
    }
  }

  public class PlatformController : MonoBehaviour
  {
    const int TOTAL_PLATFORMS = 20;
    const int PLATFORM_SPACING = 4;
    const float DOWN_DECELERATION = 0.05f;
    [SerializeField] private GameObject platform;
    [SerializeField] private Transform player;

    private int platformCount = 0;
    private SortedList platforms;
    private Rect cameraBounds;

    public Rect CameraBounds { get { return cameraBounds; } }

    public void MoveDown(float playerVelocity)
    {
      // Move grid down
      transform.position = new Vector2(transform.position.x, transform.position.y - (playerVelocity * DOWN_DECELERATION));

      // Update SortedList
      SortedList updatedPlatforms = new SortedList(new PlatformComparer(), TOTAL_PLATFORMS + 1);
      foreach (GameObject platform in platforms.Values)
      {
        if (platform != null)
        {
          updatedPlatforms.Add(++platformCount, platform);
        }
      }

      platforms = updatedPlatforms;
    }

    private void RemoveOldestPlatform()
    {
      GameObject oldestPlatform = (GameObject)platforms.GetByIndex(0);

      if (oldestPlatform.transform.position.y < cameraBounds.yMin - 2f)
      {
        platforms.RemoveAt(0);
        GameObject.Destroy(oldestPlatform);

        GameObject latestPlatform = (GameObject)platforms.GetByIndex(platforms.Count - 1);
        SpawnPlatform(latestPlatform.transform.position.y + UnityEngine.Random.Range(0, PLATFORM_SPACING));
      }
    }

    public static void PrintKeysAndValues(SortedList myList)
    {
      Debug.Log("\t-KEY-\t-VALUE-");
      for (int i = 0; i < myList.Keys.Count; i++)
      {
        Debug.Log($"yPos: {myList.GetKey(i)}");
      }
    }

    private void Awake()
    {
      var camera = Camera.main;

      Vector3 bottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
      Vector3 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));

      cameraBounds = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);

      platforms = new SortedList(new PlatformComparer(), TOTAL_PLATFORMS);

      var startingYPos = player.position.y - 1;
      for (var i = 1; i < TOTAL_PLATFORMS; i++)
      {
        startingYPos += UnityEngine.Random.Range(0, PLATFORM_SPACING);
        SpawnPlatform(startingYPos);
      }
    }

    private void Update()
    {
      RemoveOldestPlatform();
    }

    private void SpawnPlatform(float y)
    {
      var randomXPos = UnityEngine.Random.Range(cameraBounds.xMin, cameraBounds.xMax);
      var location = new Vector2(randomXPos, y);
      var hits = Physics2D.RaycastAll(location, Vector2.right);
      foreach(var hit in hits)
      {
        if (hit.collider != null)
        {
          float distance = Mathf.Abs(hit.point.x - randomXPos);
          if (distance < 4)
          {
            randomXPos = hit.point.x + 3;
          }
        }
      }
      hits = Physics2D.RaycastAll(location, Vector2.left);
      foreach(var hit in hits)
      {
        if (hit.collider != null)
        {
          float distance = Mathf.Abs(hit.point.x - randomXPos);
          if (distance < 4)
          {
            randomXPos = hit.point.x - 3;
          }
        }
      }
      platforms.Add(++platformCount, Instantiate<GameObject>(platform, new Vector2(randomXPos, y), Quaternion.identity, transform));
    }
  }
}