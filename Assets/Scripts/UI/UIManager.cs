using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI scoreText;
  [SerializeField] private TextMeshProUGUI highscoreText;
  private int score;

  void Awake()
  {
    score = 0;
    scoreText.text = $"Score: {score}";
    highscoreText.text = $"High Score: {PlayerPrefs.GetInt("highscore")}";
  }

  public void AddScore()
  {
    score++;
    scoreText.text = $"Score: {score}";
  }

  public int getScore()
  {
    return this.score;
  }
}
