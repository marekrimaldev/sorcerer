using UnityEngine;
using System;

namespace VRTetris
{
    public class ScoreTracker : MonoBehaviourSingleton<ScoreTracker>
    {
        private int _score;
        public int Score => _score;
        public int Highscore => PlayerPrefs.HasKey(HighscoreKey) ? PlayerPrefs.GetInt(HighscoreKey) : 0;

        public int Level => Mathf.FloorToInt(_totalRowsCleared / 10) + 1;
        private int _totalRowsCleared = 0;

        private const string HighscoreKey = "highscore";

        private readonly int[] ScoreTable = new int[]{ 100, 300, 500, 800 };

        public static Action<int> OnScoreChange;

        private void ResetScore()
        {
            _score = 0;
            _totalRowsCleared = 0;
        }

        public void RowClearScored(int rowsCleared)
        {
            int score = ScoreTable[rowsCleared];
            AddScore(score);

            _totalRowsCleared += rowsCleared;
        }

        private void AddScore(int score)
        {
            _score += score * Level;
            OnScoreChange?.Invoke(_score);
        }

        public void SaveScore()
        {
            int highscore = 0;
            if (PlayerPrefs.HasKey(HighscoreKey))
            {
                highscore = PlayerPrefs.GetInt(HighscoreKey);
            }

            if(Score > highscore)
            {
                PlayerPrefs.SetInt(HighscoreKey, Score);
            }
        }
    }
}
