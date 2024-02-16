using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace VRTetris
{
    public class PieceSpawn : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private TimeVisualization _timer;

        private Piece _spawnedPiece;
        
        public bool IsEmpty => _spawnedPiece == null;

        private void OnEnable()
        {
            // TODO
            _timer.OnTimesUp += () => { Debug.Log("Time is up"); };
        }

        public Piece SpawnPiece(Piece piecePrefab)
        {
            _spawnedPiece = InstantiateNextPiece(piecePrefab);
            _spawnedPiece.OnPieceGrabbed += OnPieceGrabbedListener;

            _timer.StartTimer(PieceSpawner.Instance.GrabLimit);

            return _spawnedPiece;
        }

        public void SetInteractability(bool val)
        {
            if (_spawnedPiece != null)
                _spawnedPiece.SetInteractability(val);
        }

        private Piece InstantiateNextPiece(Piece piecePrefab)
        {
            Piece piece = Instantiate(piecePrefab, _spawnPoint.position, _spawnPoint.rotation);
            piece.transform.localScale = Vector3.one * PieceSpawner.PieceScale;

            for (int i = 0; i < piece.Cubes.Length; i++)
            {
                piece.Cubes[i].localScale = Vector3.one * PieceSpawner.CubeInnerScale;
            }

            return piece;
        }

        private void OnPieceGrabbedListener(Piece piece)
        {
            piece.OnPieceGrabbed -= OnPieceGrabbedListener;

            _timer.StopTimer();
            _spawnedPiece = null;
        }
    }
}
