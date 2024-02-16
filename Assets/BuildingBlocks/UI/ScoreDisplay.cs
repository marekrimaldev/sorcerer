using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private bool _lessIsBetter = true;
    [SerializeField] private int _goldThreshold;
    [SerializeField] private int _silverThreshold;
    [SerializeField] private int _bronzeThreshold;
    [SerializeField] private TMP_Text _scoreText;
     
    [SerializeField] private SpriteRenderer[] _stars;

    private void DisplayScore(int score)
    {
        int starCount = GetStarCount(score);
        ShowStars(starCount);
    }

    private int GetStarCount(int score)
    {
        int starCount = 0;

        if (_lessIsBetter)
        {
            if (score <= _goldThreshold)
            {
                starCount = 3;
            }
            else if (score <= _silverThreshold)
            {
                starCount = 2;
            }
            else if (score <= _bronzeThreshold)
            {
                starCount = 1;
            }
        }
        else
        {
            if (score >= _bronzeThreshold)
            {
                starCount = 3;
            }
            else if (score >= _silverThreshold)
            {
                starCount = 2;
            }
            else if (score >= _goldThreshold)
            {
                starCount = 1;
            }
        }

        return starCount;
    }

    private void ShowStars(int starCount)
    {
        for (int i = starCount; i < _stars.Length; i++)
        {
            _stars[i].color = Color.gray;
        }
    }
}
