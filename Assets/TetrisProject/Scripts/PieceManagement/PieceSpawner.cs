using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace VRTetris
{
    public class PieceSpawner : MonoBehaviour
    {
        [SerializeField] private bool _waitUntilPlacement;
        [SerializeField] private float _secondsBetweenPieces = 1;
        [SerializeField] private float _secondsDecrement = 0.05f;   // Bind this to ScoreTracker.Level
        [SerializeField] private float _cubeInnerScale = 0.9f;      // The scale of the cube inside the block volume
        [SerializeField] private Piece[] _piecePrefabs;
        [SerializeField] private Transform _spawn;

        private List<Piece> _preparedPieces = new List<Piece>();    // Pieces prepared to be grabbed

        private float _currSpawnTime;
        private bool _isSpawningOn = true;
        private float _currPieceRemainingTime;
        private int _numberOfActivePieces = 0;

        public static float PieceScale => 0.1f;

        public static Action<Piece> OnNewPieceGenerated;

        private void Start()
        {
            _currSpawnTime = _secondsBetweenPieces;

            if (_waitUntilPlacement)
            {
                _secondsBetweenPieces = 99999;
                MatrixController.OnPiecePlacement += (Piece piece) => { SpawnPiece(); };

                SpawnPiece();
            }
            else
            {
                StartGenerator();
            }
        }

        public void StartGenerator()
        {
            _isSpawningOn = true;
            StartCoroutine(SpawnCoroutine());
        }

        public void StopGenerator()
        {
            _isSpawningOn = false;
        }

        private Piece InstantiateNextPiece()
        {
            int idx = UnityEngine.Random.Range(0, _piecePrefabs.Length);
            Piece piece = Instantiate(_piecePrefabs[idx]);
            piece.transform.localScale = Vector3.one * PieceScale;

            for (int i = 0; i < piece.Cubes.Length; i++)
            {
                piece.Cubes[i].localScale = new Vector3(_cubeInnerScale, _cubeInnerScale, _cubeInnerScale);
            }

            return piece;
        }

        private void SpawnPiece()
        {
            Piece piece = InstantiateNextPiece();
            OnNewPieceGenerated?.Invoke(piece);

            piece.SetInteractability(_numberOfActivePieces < 2);
            piece.OnPieceGrabbed += OnPieceGrabbed;
            piece.OnPieceLocked += OnPieceLocked;

            _preparedPieces.Add(piece);
        }

        private void OnPieceGrabbed(Piece piece)
        {
            piece.OnPieceGrabbed -= OnPieceGrabbed;
            _numberOfActivePieces++;

            _preparedPieces.Remove(piece);
            MakePreparedPiecesInteractable(_numberOfActivePieces < 2);
        }

        private void OnPieceLocked(Piece piece)
        {
            piece.OnPieceGrabbed -= OnPieceLocked;
            _numberOfActivePieces--;

            MakePreparedPiecesInteractable(true);
        }

        private void MakePreparedPiecesInteractable(bool val)
        {
            for (int i = 0; i < _preparedPieces.Count; i++)
            {
                _preparedPieces[i].SetInteractability(val);
            }
        }

        private IEnumerator SpawnCoroutine()
        {
            while (_isSpawningOn)
            {
                yield return new WaitForSeconds(_currSpawnTime);

                SpawnPiece();
            }
        }
    }
}
