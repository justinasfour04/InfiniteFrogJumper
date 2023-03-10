using System;
using System.Collections;
using UnityEngine;
using FrogJump.Core;
using FrogJump.Model;

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
    const int TOTAL_PLATFORMS = 15;
    const int PLATFORM_SPACING = 5;
    const float DOWN_DECELERATION = 0.05f;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private Transform player;
    private GameModel model = Util.GetModel<GameModel>();
    private int platformCount = 0;
    private SortedList platforms;
    private Hashtable collisionByInstanceId;

    public void MoveDown(float playerVelocity)
    {
      // Move grid down
      transform.position = new Vector2(transform.position.x, transform.position.y - (playerVelocity * DOWN_DECELERATION));

      // Update SortedList
      SortedList updatedPlatforms = new SortedList(new PlatformComparer(), TOTAL_PLATFORMS);
      foreach (GameObject platform in platforms.Values)
      {
        if (platform != null)
        {
          updatedPlatforms.Add(++platformCount, platform);
        }
      }

      platforms = updatedPlatforms;
    }

    public bool IsCollisionWith(Guid uniqueId)
    {
      return (bool)collisionByInstanceId[uniqueId];
    }

    public void SetCollisionOfPlatform(Guid uniqueId)
    {
      collisionByInstanceId[uniqueId] = true;
    }

    private void RemoveOldestPlatform()
    {
      GameObject oldestPlatform = (GameObject)platforms.GetByIndex(0);

      if (oldestPlatform.transform.position.y < model.CameraBounds.yMin - 2f)
      {
        platforms.RemoveAt(0);
        GameObject.Destroy(oldestPlatform);

        GameObject latestPlatform = (GameObject)platforms.GetByIndex(platforms.Count - 1);
        SpawnPlatform(latestPlatform.transform.position.y + UnityEngine.Random.Range(2, PLATFORM_SPACING));
      }
    }

    private void Awake()
    {
      collisionByInstanceId = new Hashtable();
      platforms = new SortedList(new PlatformComparer(), TOTAL_PLATFORMS);

      var startingYPos = Screen.safeArea.yMin;
      for (var i = 1; i < TOTAL_PLATFORMS; i++)
      {
        startingYPos += UnityEngine.Random.Range(2, PLATFORM_SPACING);
        SpawnPlatform(startingYPos);
      }
    }

    private void Update()
    {
      RemoveOldestPlatform();
    }

    private void SpawnPlatform(float y)
    {
      float randomXPos;
      if (platformCount == 0)
      {
        randomXPos = UnityEngine.Random.Range(model.CameraBounds.xMin, model.CameraBounds.center.x);
      }
      else
      {
        randomXPos = UnityEngine.Random.Range(model.CameraBounds.xMin, model.CameraBounds.xMax);
      }
      var position = new Vector2(randomXPos, y);
      GameObject gameObject = Instantiate<GameObject>(platformPrefab, position, Quaternion.identity, transform);
      Platform platform = gameObject.GetComponent<Platform>();
      if (!collisionByInstanceId.ContainsKey(platform.UniqueId))
      {
        var random = UnityEngine.Random.Range(0, 3);
        switch (random)
        {
          case 0:
            platform.MovingDirection = Movement.HORIZONTAL;
            break;
          case 1:
            platform.MovingDirection = Movement.VERTICAL;
            break;
          case 2:
            platform.MovingDirection = Movement.NONE;
            break;
        }
        collisionByInstanceId.Add(platform.UniqueId, false);
      }
      platforms.Add(++platformCount, gameObject);
    }
  }
}