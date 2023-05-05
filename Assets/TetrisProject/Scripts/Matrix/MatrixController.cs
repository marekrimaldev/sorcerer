using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTetris
{
    public class MatrixController : MonoBehaviour
    {
        [SerializeField] private Vector3Int _dimensions;
        [SerializeField] private GameObject _placementVisualizationPrefab;
        [SerializeField] private GameObject _cellVisualizationPrefab;

        private Matrix _matrix;
        //private Matrix _helperMatrix;

        private List<Piece> _activePieces = new List<Piece>();


        public static System.Action<Piece> OnPiecePlacement;
        public static System.Action OnGameOver;

        private void Awake()
        {
            Init();
        }

        private void OnEnable()
        {
            PieceGenerator.OnNewPieceGenerated += OnNewPieceGenerated;
            VRPlayer.OnPieceDropped += OnPieceDropped;
            PieceThreadmill.OnPieceCollision += OnPieceCollision;
        }

        private void OnDisable()
        {
            PieceGenerator.OnNewPieceGenerated -= OnNewPieceGenerated;
            VRPlayer.OnPieceDropped -= OnPieceDropped;
            PieceThreadmill.OnPieceCollision -= OnPieceCollision;
        }

        private void Update()
        {
            VisualizeClosestPiece();
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

            if(pieceToVisualize != null)
                TryVisualizePiecePlacement(pieceToVisualize);
        }

        public void OnNewPieceGenerated(Piece piece)
        {
            piece.OnPieceGrabbed += OnPieceGrabbed;
        }

        private void OnPieceGrabbed(Piece piece)
        {
            piece.OnPieceGrabbed -= OnPieceGrabbed;
            _activePieces.Add(piece);
        }

        public void OnPieceDropped(Piece piece)
        {
            TryAddPiece(piece);
        }

        private void PositionMatrix()
        {
            transform.position += Vector3.left * _dimensions.x * PieceGenerator.PieceScale / 2;
            transform.position += Vector3.forward * _dimensions.z * PieceGenerator.PieceScale / 2;
            transform.position += Vector3.forward * .15f;
            transform.position += Vector3.up * 1;
        }

        private void Init()
        {
            PositionMatrix();
            _matrix = new Matrix(transform.position, _dimensions);
        }

        private bool TryVisualizePiecePlacement(Piece piece)
        {
            if (!_matrix.IsPlacementValid(piece))
                return false;

            _matrix.VisualizePiecePlacement(piece);

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
    }
}
