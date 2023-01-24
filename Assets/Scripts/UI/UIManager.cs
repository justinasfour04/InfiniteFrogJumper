using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI scoreText;
  private int score;

  void Awake()
  {
    score = 0;
    scoreText.text = $"Score: {score}";
  }

  public void AddScore()
  {
    score++;
    scoreText.text = $"Score: {score}";
  }
}
