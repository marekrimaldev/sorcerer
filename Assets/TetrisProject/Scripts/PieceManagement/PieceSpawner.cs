using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace VRTetris
{
    public class PieceSpawner : MonoBehaviourSingleton<PieceSpawner>
    {
        [SerializeField] private bool _waitUntilPlacement;
        [SerializeField] private float _secondsGrabLimit = 5f;
        [SerializeField] private float _secondsGrabLimitLevelSpeedUp = 0.1f;
        [SerializeField] private Piece[] _piecePrefabs;
        [SerializeField] private PieceSpawn[] _spawns;

        public float GrabLimit => Mathf.Max(_secondsGrabLimit - (ScoreTracker.Instance.Level * _secondsGrabLimitLevelSpeedUp), 1f);

        public static readonly float CubeInnerScale = 0.9f;    // The scale of the cube inside the block volume
        public static readonly float PieceScale = 0.1f;

        public static Action<Piece> OnNewPieceSpawned;

        private void Start()
        {
            if (_waitUntilPlacement)
            {
                _secondsGrabLimit = 99999;
                MatrixController.OnPiecePlacement += (Piece piece) => { SpawnNewPiece(); };

                SpawnNewPiece();
            }
            else
            {
                StartSpawning();
            }
        }
                
        public void StartSpawning()
        {
            // There is no coroutine lol. It spawn automaticaly when grabbed.
            SpawnNewPiece();
            SpawnNewPiece();    // We start with a piece on both spawns
            SetSpawnInteractability(MatrixController.Instance.ActivePieces < 2);
        }

        public void StopSpawning()
        {
        }

        private void SpawnNewPiece()
        {
            PieceSpawn spawn = GetFreeSpawn();
            int idx = UnityEngine.Random.Range(0, _piecePrefabs.Length);
            Piece piece = spawn.SpawnPiece(_piecePrefabs[idx]);

            piece.OnPieceGrabbed += OnPieceGrabbed;
            piece.OnPieceLocked += OnPieceLocked;

            OnNewPieceSpawned?.Invoke(piece);
        }

        private PieceSpawn GetFreeSpawn()
        {
            List<PieceSpawn> freeSpawns = new List<PieceSpawn>();
            for (int i = 0; i < _spawns.Length; i++)
            {
                if (_spawns[i].IsFree)
                    freeSpawns.Add(_spawns[i]);
            }

            int idx = UnityEngine.Random.Range(0, freeSpawns.Count - 1);
            return freeSpawns[idx];
        }

        private void OnPieceGrabbed(Piece piece)
        {
            piece.OnPieceGrabbed -= OnPieceGrabbed;
            SpawnNewPiece();
            CallMethodWithDelay(() => SetSpawnInteractability(MatrixController.Instance.ActivePieces < 2), .1f);
        }

        private void OnPieceLocked(Piece piece)
        {
            piece.OnPieceLocked -= OnPieceLocked;
            CallMethodWithDelay(() => SetSpawnInteractability(true), .1f);
        }

        private void SetSpawnInteractability(bool val)
        {
            for (int i = 0; i < _spawns.Length; i++)
            {
                _spawns[i].SetInteractability(val);
            }
        }

        /// <summary>
        /// Put these into some helper class
        /// </summary>
        /// <param name="method"></param>
        /// <param name="delay"></param>
        private void CallMethodWithDelay(Action method, float delay)
        {
            StartCoroutine(CallMethodWithDelayCo(method, delay));
        }

        private IEnumerator CallMethodWithDelayCo(Action method, float delay)
        {
            yield return new WaitForSeconds(delay);
            method?.Invoke();
        }
    }
}
