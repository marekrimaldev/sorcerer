using UnityEngine;
using TMPro;
using System.Collections;

namespace VRTetris
{
    public class ScoreDisplay : InGameDisplay
    {
        [SerializeField] private TMP_Text _scoreTxt;
        [SerializeField] private float _scoreAddAnimDuration;
        [SerializeField] private int _digits;

        private void Awake()
        {
            UpdateScore(0);
        }

        private void OnEnable()
        {
            ScoreTracker.OnScoreChange += UpdateScore;
        }

        private void OnDisable()
        {
            ScoreTracker.OnScoreChange -= UpdateScore;
        }

        private void UpdateScore(int currScore)
        {
            _scoreTxt.text = currScore.ToString("D" + _digits.ToString());
            StartCoroutine(AddScoreAnimCoroutine(_scoreAddAnimDuration));
        }

        private IEnumerator AddScoreAnimCoroutine(float duration)
        {
            float t = 0;
            Color c1 = Color.red;
            Color c2 = Color.white;
            _scoreTxt.color = c1;

            while (t <= 1)
            {
                yield return null;

                t += Time.deltaTime / duration;
                _scoreTxt.color = Color.Lerp(c1, c2, t);
            }
        }
    }
}
