using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTetris
{
    public class ScoreTracker : MonoBehaviour
    {
        [SerializeField] private int _rowClearScore = 1000;
        [SerializeField] private int _tetrisScore;
        [SerializeField] private int[] _comboMultipliers;
        private int _currCombo = 0;
        private int _rowsClearedWithLastPiece = 0;

        private const int TetrisStreak = 4;

        private int _score;
        public int Score => _score;

        private static ScoreTracker _instance;
        public static ScoreTracker Instance => _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this);
                return;
            }
            _instance = this;

            PieceGenerator.OnNewPieceGenerated += OnNewPieceGenerated;
        }

        public void RowClearScored()
        {
            AddScore(_rowClearScore);

            _rowsClearedWithLastPiece++;
            if (_rowsClearedWithLastPiece >= TetrisStreak)
                TetrisScored();
        }

        private void AddScore(int score)
        {
            _score += (int)(score * GetComboMultiplier());
        }

        private void TetrisScored()
        {
            AddScore(_tetrisScore);
        }

        private void OnNewPieceGenerated(Piece piece)
        {
            if (_rowsClearedWithLastPiece > 0)
                _currCombo++;
            else
                _currCombo = 0;

            _rowsClearedWithLastPiece = 0;
        }

        private float GetComboMultiplier()
        {
            int combo = Mathf.Min(_currCombo, _comboMultipliers.Length);
            return _comboMultipliers[combo];
        }
    }
}
