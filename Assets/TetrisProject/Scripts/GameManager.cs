using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VRTetris
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        [SerializeField] private GameObject _gameStateScene;
        [SerializeField] private GameObject _pauseStateScene;

        private bool _isGameOver = false;
        public bool IsGameOver => _isGameOver;

        private void OnEnable()
        {
            ButtonEvents.OnPauseGameRequested += PauseGame;
            ButtonEvents.OnResumeGameRequested += ResumeGame;
            ButtonEvents.OnRestartGameRequested += RestartGame;
            MatrixController.OnGameOver += GameOver;
        }

        private void OnDisable()
        {
            ButtonEvents.OnPauseGameRequested -= PauseGame;
            ButtonEvents.OnResumeGameRequested -= ResumeGame;
            ButtonEvents.OnRestartGameRequested -= RestartGame;
            MatrixController.OnGameOver -= GameOver;
        }

        private void PauseGame()
        {
            _gameStateScene.SetActive(false);
            _pauseStateScene.SetActive(true);
        }

        private void ResumeGame()
        {
            if (IsGameOver)
                RestartGame();

            _gameStateScene.SetActive(true);
            _pauseStateScene.SetActive(false);
        }

        private void RestartGame()
        {
            _isGameOver = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void GameOver()
        {
            _isGameOver = true;
            ScoreTracker.Instance.SaveScore();

            PauseGame();
        }
    }
}
