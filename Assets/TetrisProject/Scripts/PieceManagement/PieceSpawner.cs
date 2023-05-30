using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace VRTetris
{
    public class PieceSpawner : MonoBehaviourSingleton<PieceSpawner>
    {
        [SerializeField] private bool _waitUntilPlacement = false;
        [SerializeField] private float _secondsGrabLimit = 5f;
        [SerializeField] private float _secondsGrabLimitLevelSpeedUp = 0.1f;
        [SerializeField] private Piece[] _piecePrefabs;
        [SerializeField] private PieceSpawn[] _spawns;

        public float GrabLimit => Mathf.Max(_secondsGrabLimit - (ScoreTracker.Instance.Level * _secondsGrabLimitLevelSpeedUp), 1f);

        public static readonly float CubeInnerScale = 0.9f;    // The scale of the cube inside the block volume
        public static readonly float PieceScale = 0.1f;

        public static Action<Piece> OnNewPieceSpawned;

        private void OnEnable()
        {
            MatrixController.OnChangeActivePieceCount += OnChangeActivePieceCountListener;
        }

        private void OnDisable()
        {
            MatrixController.OnChangeActivePieceCount -= OnChangeActivePieceCountListener;
        }

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
            SetSpawnInteractability(false);
        }

        public void StopSpawning()
        {
        }

        private PieceSpawn GetEmptySpawn()
        {
            List<PieceSpawn> freeSpawns = new List<PieceSpawn>();
            for (int i = 0; i < _spawns.Length; i++)
            {
                if (_spawns[i].IsEmpty)
                    freeSpawns.Add(_spawns[i]);
            }

            int idx = UnityEngine.Random.Range(0, freeSpawns.Count - 1);
            return freeSpawns[idx];
        }

        private Piece GetRandomPiecePrefab()
        {
            int idx = UnityEngine.Random.Range(0, _piecePrefabs.Length);
            return _piecePrefabs[idx];
        }

        private void SpawnNewPiece()
        {
            StartCoroutine(SpawnNewPieceCoroutine());
        }

        private IEnumerator SpawnNewPieceCoroutine()
        {
            // TODO: Here wait until the hand is not far enough to spawn the next piece
            yield return null;

            PieceSpawn spawn = GetEmptySpawn();
            Piece piece = spawn.SpawnPiece(GetRandomPiecePrefab());

            piece.OnPieceGrabbed += OnPieceGrabbedListener;

            OnNewPieceSpawned?.Invoke(piece);
        }


        private void OnChangeActivePieceCountListener(int activePieces)
        {
            SetSpawnInteractability(activePieces < 2);
        }

        private void OnPieceGrabbedListener(Piece piece)
        {
            piece.OnPieceGrabbed -= OnPieceGrabbedListener;
            SpawnNewPiece();
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
