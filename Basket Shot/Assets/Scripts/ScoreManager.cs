using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI starsText;
    [SerializeField] private TextMeshProUGUI bestText;
    private int score;
    private int bestScore;
    private int stars;

    private void Start()
    {
        if (PlayerPrefs.HasKey("BestScore"))
        {
            bestScore = PlayerPrefs.GetInt("BestScore");
        }
        else
        {
            bestScore = 0;
        }
        bestScore = 0;

        if (PlayerPrefs.HasKey("Stars"))
        {
            stars = PlayerPrefs.GetInt("Stars");
        }
        else
        {
            stars = 0;
        }

        starsText.text = stars.ToString();

        score = 0;
        scoreText = GetComponent<TextMeshProUGUI>();        
    }

    public void AddScore(int score)
    {
        this.score += score;
        scoreText.text = this.score.ToString();
        if (bestScore < score)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
        }
    }

    public void AddStar()
    {
        stars++;
        starsText.text = stars.ToString();
        PlayerPrefs.SetInt("Stars", stars);
    }

    public void ShowScreenDie()
    {
        bestText.text = bestScore.ToString();
    }
}