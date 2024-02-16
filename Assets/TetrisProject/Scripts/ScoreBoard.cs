using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace VRTetris
{
    public class ScoreBoard : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleTxt;
        [SerializeField] private TMP_Text _currScoreTxt;
        [SerializeField] private TMP_Text _currLevelTxt;
        [SerializeField] private TMP_Text _highscoreTxt;

        private void OnEnable()
        {
            _titleTxt.text = GameManager.Instance.IsGameOver ? "Game Over" : "Game Paused";
            _currScoreTxt.text = ScoreTracker.Instance.Score.ToString("D8");
            _currLevelTxt.text = ScoreTracker.Instance.Level.ToString("D0");
            _highscoreTxt.text = ScoreTracker.Instance.Highscore.ToString("D8");
        }
    }
}
