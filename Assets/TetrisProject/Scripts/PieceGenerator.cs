using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace VRTetris
{
    public class PieceGenerator : MonoBehaviour
    {
        [SerializeField] private bool _waitUntilPlacement;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _secondsBetweenPieces = 1;
        [SerializeField] private float _secondsDecrement = 0.05f;
        [SerializeField] private Color[] _colors;
        [SerializeField] private Piece[] _piecePrefabs;
        [SerializeField] private PieceThreadmillManager _threadmillManager;

        private float _currSpawnTime;
        private bool _isSpawningOn = true;

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
            Piece piece = Instantiate(_piecePrefabs[idx], _spawnPoint.position, _spawnPoint.rotation);
            piece.transform.localScale = Vector3.one * PieceScale;

            return piece;
        }

        private void AssignColorToPiece(Piece piece)
        {
            int idx = UnityEngine.Random.Range(0, _colors.Length);
            piece.SetColor(_colors[idx]);
        }

        private void SpawnPiece()
        {
            Piece piece = InstantiateNextPiece();
            AssignColorToPiece(piece);
            _threadmillManager.AddPiece(piece);

            OnNewPieceGenerated?.Invoke(piece);
        }

        private IEnumerator SpawnCoroutine()
        {
            while (_isSpawningOn)
            {
                yield return new WaitForSeconds(_currSpawnTime);

                SpawnPiece();
                _currSpawnTime -= _secondsDecrement;
            }
        }
    }
}
