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

        private List<Transform> _cubes = new List<Transform>();
        private Transform[][] _matrix;
        private Transform[][] _matrixVisualization;

        private List<Piece> _activePieces = new List<Piece>();
        private List<Transform> _visualizationCubes = new List<Transform>();

        private const int MaxCubesPerPiece = 4;

        public static System.Action<Piece> OnPiecePlacement;

        private void Awake()
        {
            InitMatrix();
            InitVisualizationCubes();
        }

        private void OnEnable()
        {
            PieceGenerator.OnNewPieceGenerated += OnNewPieceGenerated;
        }

        private void OnDisable()
        {
            PieceGenerator.OnNewPieceGenerated -= OnNewPieceGenerated;
        }

        private void Start()
        {
            DrawMatrix();
        }

        private void Update()
        {
            for (int i = 0; i < _activePieces.Count; i++)
            {
                TryVisualizePiecePlacement(_activePieces[i]);
            }
        }

        public void OnNewPieceGenerated(Piece piece)
        {
            Debug.Log("New piece added");
            _activePieces.Add(piece);
        }

        public void OnPieceDropped(Piece piece)
        {
            Debug.Log("Piece dropped");
            TryAddPiece(piece);
        }

        private void InitMatrix()
        {
            transform.position += Vector3.left * _dimensions.x * PieceGenerator.PieceScale / 2;
            transform.position += Vector3.forward * _dimensions.z * PieceGenerator.PieceScale / 2;
            transform.position += Vector3.forward * .15f;
            transform.position += Vector3.up * 1;

            _matrix = new Transform[_dimensions.y][];
            for (int i = 0; i < _dimensions.y; i++)
            {
                _matrix[i] = new Transform[_dimensions.x];
            }
        }

        private void DrawMatrix()
        {
            _matrixVisualization = new Transform[_dimensions.y][];
            for (int y = 0; y < _dimensions.y; y++)
            {
                _matrixVisualization[y] = new Transform[_dimensions.x];
                for (int x = 0; x < _dimensions.x; x++)
                {
                    for (int z = 0; z < _dimensions.z; z++)
                    {
                        Transform cell = Instantiate(_cellVisualizationPrefab, transform).transform;
                        cell.localPosition = new Vector3(x, y, z) * PieceGenerator.PieceScale;
                        cell.localScale = Vector3.one * 0.85f * PieceGenerator.PieceScale;

                        _matrixVisualization[y][x] = cell;
                    }
                }
            }
        }

        private void InitVisualizationCubes()
        {
            for (int i = 0; i < MaxCubesPerPiece; i++)
            {
                GameObject cube = Instantiate(_placementVisualizationPrefab, transform);
                cube.name = "Visualization cube";
                cube.SetActive(false);

                _visualizationCubes.Add(cube.transform);
            }
        }

        private bool IsPlacementValid(Piece piece)
        {
            if (!IsPieceCorrectlyRotated(piece.transform))
                return false;

            bool isConnected = false;
            Transform[] cubes = piece.Cubes;
            for (int i = 0; i < cubes.Length; i++)
            {
                if (!IsCubeInsideMatrix(cubes[i]))
                    return false;

                if (IsCubeColliding(cubes[i]))
                    return false;

                if (IsCubeConnected(cubes[i]))
                    isConnected = true;
            }

            //if (!isConnected)
            //    return false;

            return true;
        }

        private bool TryAddPiece(Piece piece)
        {
            if (!IsPlacementValid(piece))
                return false;

            LockInPiece(piece);
            DetectRowClears();

            return true;
        }

        private void ActivateVisualizationCubes(bool enable)
        {
            for (int i = 0; i < _visualizationCubes.Count; i++)
            {
                _visualizationCubes[i].gameObject.SetActive(enable);
            }
        }

        private bool TryVisualizePiecePlacement(Piece piece)
        {
            // Here try add the piece to viz matrix and then check connectivity on all the cubes

            bool validPlacement = IsPlacementValid(piece);
            ActivateVisualizationCubes(validPlacement);

            if (!validPlacement)
                return false;

            Debug.Log("Placement valid - Vizualizing");

            for (int i = 0; i < piece.Cubes.Length; i++)
            {
                Vector3Int cubeMatrixPos = GetCubeMatrixPosition(piece.Cubes[i]);
                _visualizationCubes[i].localPosition = Vector3.Scale(cubeMatrixPos, piece.Cubes[i].lossyScale);
            }

            return true;
        }

        private Vector3Int GetCubeMatrixPosition(Transform cube)
        {
            Vector3 localPos = cube.position - transform.position;

            int xPos = Mathf.RoundToInt(localPos.x / cube.lossyScale.x);
            int yPos = Mathf.RoundToInt(localPos.y / cube.lossyScale.y);
            int zPos = Mathf.RoundToInt(localPos.z / cube.lossyScale.z);

            return new Vector3Int(xPos, yPos, zPos);
        }

        private bool IsCellEmpty(int x, int y, int z)
        {
            return _matrix[x][y] == null;
        }

        private bool IsCellEmpty(Vector3Int matrixPos)
        {
            return IsCellEmpty(matrixPos.x, matrixPos.y, matrixPos.z);
        }

        private bool IsPositionInBounds(int x, int y, int z)
        {
            bool xBounds = x >= 0 && x < _dimensions.x;
            bool yBounds = y >= 0 && y < _dimensions.y;
            bool zBounds = z >= 0 && z < _dimensions.z;

            return xBounds && yBounds && zBounds;
        }

        private bool IsPieceCorrectlyRotated(Transform piece)
        {
            // Make this detections better - the connectivity between piece cube must mathch the connectivity of their images in the matrix


            Vector3 eulers = piece.rotation.eulerAngles;

            int xDiff = (int)eulers.x % 90;
            int yDiff = (int)eulers.y % 90;
            int zDiff = (int)eulers.z % 90;

            xDiff = Mathf.Min(90 - xDiff, xDiff);
            yDiff = Mathf.Min(90 - yDiff, yDiff);
            zDiff = Mathf.Min(90 - zDiff, zDiff);

            Debug.Log("x = " + xDiff);
            Debug.Log("y = " + yDiff);
            Debug.Log("z = " + zDiff);

            //bool xBounds = xDiff < _allowedPlacementRotationError;
            //bool yBounds = yDiff < _allowedPlacementRotationError;
            //bool zBounds = zDiff < _allowedPlacementRotationError;

            return xBounds && yBounds && zBounds;
        }

        private bool IsPositionInBounds(Vector3Int matrixPos)
        {
            return IsPositionInBounds(matrixPos.x, matrixPos.y, matrixPos.z);
        }

        private bool IsCubeInsideMatrix(Transform cube)
        {
            Vector3Int cubeMatrixPos = GetCubeMatrixPosition(cube);
            Debug.Log(cubeMatrixPos);
            return IsPositionInBounds(cubeMatrixPos);
        }

        private bool IsCubeColliding(Transform cube)
        {
            Vector3Int cubeMatrixPos = GetCubeMatrixPosition(cube);
            return !IsCellEmpty(cubeMatrixPos);
        }

        private bool IsCubeConnected(Transform cube)
        {
            Vector3Int cubeMatrixPos = GetCubeMatrixPosition(cube);

            for (int x = -1; x <= 1; x+=2)
            {
                for (int y = -1; y <= 1; y += 2)
                {
                    int xx = cubeMatrixPos.x + x;
                    int yy = cubeMatrixPos.y + y;
                    int zz = cubeMatrixPos.z + 0;

                    if (!IsPositionInBounds(xx, yy, zz))
                        continue;

                    if (!IsCellEmpty(xx, yy, zz))
                        return true;
                }
            }

            return false;
        }

        private void PlaceCubeToMatrix(Transform cube)
        {
            Vector3Int cubeMatrixPos = GetCubeMatrixPosition(cube);
            _matrix[cubeMatrixPos.x][cubeMatrixPos.y] = cube;
            cube.localPosition = Vector3.Scale(cubeMatrixPos, cube.localScale) + cube.localScale * 0.5f;
        }

        private void LockInPiece(Piece piece)
        {
            piece.LockIn();
            piece.transform.SetParent(transform);

            for (int i = 0; i < piece.Cubes.Length; i++)
            {
                PlaceCubeToMatrix(piece.Cubes[i]);
                _cubes.Add(piece.Cubes[i]);
            }

            _activePieces.Remove(piece);

            OnPiecePlacement?.Invoke(piece);
        }

        private bool DetectRowClear(int row)
        {
            for (int x = 0; x < _dimensions.x; x++)
            {
                if (IsCellEmpty(x, row, 0))
                    return false;
            }

            return true;
        }

        private void ClearRow(int row)
        {
            for (int x = 0; x < _dimensions.x; x++)
            {
                // Probably some effect here

                Destroy(_matrix[x][row]);
                _matrix[x][row] = null;
            }
        }

        private void DetectRowClears()
        {
            for (int y = 0; y < _dimensions.y; y++)
            {
                if (DetectRowClear(y))
                {
                    ClearRow(y);
                    ScoreTracker.Instance.RowClearScored();
                }
            }
        }
    }
}
