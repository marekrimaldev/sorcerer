using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VRTetris
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject _gameStateScene;
        [SerializeField] private GameObject _pauseStateScene;

        private bool _isGameOver = false;
        public bool IsGameOver => _isGameOver;

        private static GameManager _instance;
        public static GameManager Instance => _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this);
                return;
            }
            _instance = this;

            DontDestroyOnLoad(this.gameObject);
        }

        private void OnEnable()
        {
            ButtonEvents.OnPauseGameRequested += PauseGame;
            ButtonEvents.OnResumeGameRequested += ResumeGame;
            ButtonEvents.OnRestartGameRequested += RestartGame;
        }

        private void OnDisable()
        {
            ButtonEvents.OnPauseGameRequested -= PauseGame;
            ButtonEvents.OnResumeGameRequested -= ResumeGame;
            ButtonEvents.OnRestartGameRequested -= RestartGame;
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
        }
    }
}