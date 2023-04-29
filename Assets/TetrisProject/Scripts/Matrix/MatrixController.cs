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
        private Matrix _helperMatrix;

        private List<Piece> _activePieces = new List<Piece>();
        private List<Transform> _helperCubes = new List<Transform>();

        private const int MaxCubesPerPiece = 4;

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
            _helperMatrix = new Matrix(transform.position, _dimensions);
            _helperMatrix.FillMatrix(_cellVisualizationPrefab);

            InitHelperCubes();
        }

        /// <summary>
        /// The image of the piece has to be 4-connected to ensure valid rotation of the piece
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        private bool IsPieceImage4Connected(Piece piece)
        {
            // You can try to place the piece in the middle of the grid with no rotation and count the connectivity
            // Each piece might also have its own little grid and be able to calculate the connectivity for you

            _helperMatrix.ClearMatrix();

            Transform[] cubes = piece.Cubes;
            for (int i = 0; i < cubes.Length; i++)
            {
                _helperCubes[i].position = cubes[i].position;
                _helperMatrix.PlaceCubeToMatrix(_helperCubes[i]);
            }

            int neighbouring = 0;
            for (int i = 0; i < cubes.Length; i++)
            {
                neighbouring += _helperMatrix.Get4NeighbourCount(_helperCubes[i]);
            }

            if(neighbouring < 6)
            {
                _helperMatrix.ClearMatrix();
                return false;
            }

            return true;
        }

        private bool IsPlacementValid(Piece piece)
        {
            bool isBottomConnected = false;
            Transform[] cubes = piece.Cubes;
            for (int i = 0; i < cubes.Length; i++)
            {
                if (!_matrix.IsCubeInsideMatrix(cubes[i]))
                    return false;

                if (_matrix.IsCubeColliding(cubes[i]))
                    return false;

                if (_matrix.IsCubeBottomConnected(cubes[i]))
                    isBottomConnected = true;
            }

            if (!isBottomConnected)
                return false;

            if (!IsPieceImage4Connected(piece))
                return false;

            return true;
        }

        private bool TryAddPiece(Piece piece)
        {
            if (!IsPlacementValid(piece))
                return false;
                
            LockInPiece(piece);
            ActivateVisualizationCubes(false);
            _matrix.ClearFullRows();

            return true;
        }

        private void LockInPiece(Piece piece)
        {
            piece.LockIn();

            _activePieces.Remove(piece);

            //_matrix.PlacePieceToMatrix(piece);
            Transform[] cubes = piece.Cubes;
            for (int i = 0; i < cubes.Length; i++)
            {
                _matrix.PlaceCubeToMatrix(cubes[i]);
            }

            OnPiecePlacement?.Invoke(piece);
        }

        private bool TryVisualizePiecePlacement(Piece piece)
        {
            bool validPlacement = IsPlacementValid(piece);
            ActivateVisualizationCubes(validPlacement);

            if (!validPlacement)
                return false;

            Transform[] cubes = piece.Cubes;
            for (int i = 0; i < cubes.Length; i++)
            {
                _helperCubes[i].position = cubes[i].position;
                _helperMatrix.PlaceCubeToMatrix(_helperCubes[i]);
            }

            return true;
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

        private void InitHelperCubes()
        {
            for (int i = 0; i < MaxCubesPerPiece; i++)
            {
                GameObject cube = Instantiate(_placementVisualizationPrefab, transform);
                cube.name = "Visualization cube";
                cube.SetActive(false);

                _helperCubes.Add(cube.transform);
            }
        }

        private void ActivateVisualizationCubes(bool enable)
        {
            for (int i = 0; i < _helperCubes.Count; i++)
            {
                _helperCubes[i].gameObject.SetActive(enable);
            }
        }
    }
}
