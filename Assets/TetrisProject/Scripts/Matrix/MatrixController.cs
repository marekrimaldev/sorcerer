using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTetris
{
    public class MatrixController : MonoBehaviourSingleton<MatrixController>
    {
        [SerializeField] private Vector3Int _dimensions;
        [SerializeField] private GameObject _placementVisualizationPrefab;  // Maybe put to Resources
        [SerializeField] private GameObject _cellVisualizationPrefab;

        private Matrix _matrix;
        private List<Piece> _activePieces = new List<Piece>();

        public static System.Action<int> OnChangeActivePieceCount;
        public static System.Action<Piece> OnPiecePlacement;
        public static System.Action OnGameOver;

        #region MONOBEHAVIOUR

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        private void OnEnable()
        {
            PieceSpawner.OnNewPieceSpawned += OnNewPieceGeneratedListener;
            VRPlayer.OnPieceDropped += OnPieceDroppedListener;
        }

        private void OnDisable()
        {
            PieceSpawner.OnNewPieceSpawned -= OnNewPieceGeneratedListener;
            VRPlayer.OnPieceDropped -= OnPieceDroppedListener;
        }

        private void Update()
        {
            VisualizeClosestPiece();
        }

        #endregion

        #region PUBLIC INTERFACE

        public void OnNewPieceGeneratedListener(Piece piece)
        {
            piece.OnPieceGrabbed += OnPieceGrabbed;
        }

        private void OnPieceGrabbed(Piece piece)
        {
            piece.OnPieceGrabbed -= OnPieceGrabbed;

            _activePieces.Add(piece);
            OnChangeActivePieceCount?.Invoke(_activePieces.Count);
        }

        public void OnPieceDroppedListener(Piece piece)
        {
            TryAddPiece(piece);
        }

        #endregion

        #region PRIVATE METHODS

        private void PositionMatrix()
        {
            transform.position += Vector3.left * _dimensions.x * PieceSpawner.PieceScale / 2;
            transform.position += Vector3.forward * _dimensions.z * PieceSpawner.PieceScale / 2;
            transform.position += Vector3.forward * .15f;
            transform.position += Vector3.up * 1;
        }

        private void Init()
        {
            PositionMatrix();
            _matrix = new Matrix(transform.position, _dimensions);
            _matrix.CubeHolder.SetParent(transform);
        }

        private void VisualizeClosestPiece()
        {
            Piece pieceToVisualize = null;
            float minDist = 9999;
            for (int i = 0; i < _activePieces.Count; i++)
            {
                float dist = Mathf.Abs(_activePieces[i].transform.position.z - transform.position.z);
                if (dist < minDist)
                {
                    minDist = dist;
                    pieceToVisualize = _activePieces[i];
                }
            }

            if (pieceToVisualize != null)
                TryVisualizePiecePlacement(pieceToVisualize);
        }

        private bool TryVisualizePiecePlacement(Piece piece)
        {
            if (!_matrix.IsPlacementValid(piece))
                return false;

            _matrix.ShowPieceProjection(piece);

            return true;
        }

        private bool TryAddPiece(Piece piece)
        {
            if (!_matrix.IsPlacementValid(piece))
                return false;
                
            LockInPiece(piece);
            _matrix.ClearFullRows();

            return true;
        }

        private void LockInPiece(Piece piece)
        {
            piece.LockIn();

            _activePieces.Remove(piece);
            _matrix.PlacePieceToMatrix(piece);

            OnPiecePlacement?.Invoke(piece);
            OnChangeActivePieceCount?.Invoke(_activePieces.Count);
        }

        private void OnPieceCollision()
        {
            //for (int i = 0; i < _activePieces.Count; i++)
            //{
            //    // Would be nice to ungrab them before destroying
            //    Destroy(_activePieces[i].gameObject);
            //}
            //_activePieces.Clear();

            //if (_matrix.AddPenaltyRow())
            //    OnGameOver?.Invoke();
        }

        #endregion
    }
}
