using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace VRTetris
{
    public class PieceSpawn : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;

        public bool IsFree => _spawnedPiece == null;

        private Piece _spawnedPiece;
        private float _currPieceRemainingTime;
        private Coroutine _remainingTimeCoroutine;

        public Action<PieceSpawn> OnSpawnLimitOver;

        public Piece SpawnPiece(Piece piecePrefab)
        {
            _spawnedPiece = InstantiateNextPiece(piecePrefab);
            _spawnedPiece.OnPieceGrabbed += OnPieceGrabbed;

            _currPieceRemainingTime = PieceSpawner.Instance.GrabLimit;
            _remainingTimeCoroutine = StartCoroutine(AnimateRemainingTimeCoroutine());

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

        private void OnPieceGrabbed(Piece piece)
        {
            piece.OnPieceGrabbed -= OnPieceGrabbed;

            StopCoroutine(_remainingTimeCoroutine);
            _spawnedPiece = null;
        }

        /// <summary>
        /// Put this into some separate class like TimeTracker or smthing
        /// </summary>
        /// <returns></returns>
        private IEnumerator AnimateRemainingTimeCoroutine()
        {
            while (_currPieceRemainingTime > 0)
            {
                yield return null;

                _currPieceRemainingTime -= Time.deltaTime;  
            }

            OnSpawnLimitOver?.Invoke(this); // Wire this to some penalty event
        }
    }
}
