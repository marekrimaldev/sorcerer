using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTetris
{
    /// <summary>
    /// This class is representing the grid the player is placing pieces into.
    /// It is basicaly a Transform[][] wrapper providing interface to interact with the grid.
    /// </summary>
    public class Matrix
    {
        private Vector3 _origin;
        private Vector3Int _dimensions;
        private Transform[][] _matrix;
        private Transform[][] _auxMatrix;   // Auxilary matrix used for visualization and some placement checks
        private Transform _cubeHolder;      // Used as a parent to all cubes in the grid

        private const float RowClearTime = .5f;
        private const int MaxCubesPerPiece = 4;

        private GameObject _cellVisPrefab = Resources.Load<GameObject>("Tetris/CellVisPrefab");
        private GameObject _placementVisPrefab = Resources.Load<GameObject>("Tetris/PlacementVisPrefab");

        private List<Transform> _placementVisCubes = new List<Transform>();

        #region PUBLIC INTERFACE

        public Transform CubeHolder => _cubeHolder;

        public Matrix(Vector3 origin, Vector3Int dimensions)
        {
            _origin = origin;
            _dimensions = dimensions;

            _cubeHolder = new GameObject("Cube Holder").transform;
            _cubeHolder.transform.position = _origin;

            _matrix = new Transform[_dimensions.y][];
            _auxMatrix = new Transform[_dimensions.y][];
            for (int i = 0; i < _dimensions.y; i++)
            {
                _matrix[i] = new Transform[_dimensions.x];
                _auxMatrix[i] = new Transform[_dimensions.x];
            }

            InitPlacementVisCubes();
            VisualizeGrid();
        }

        public bool IsPlacementValid(Piece piece)
        {
            bool isBottomConnected = false;
            Transform[] cubes = piece.Cubes;
            for (int i = 0; i < cubes.Length; i++)
            {
                if (!IsCubeInsideMatrix(cubes[i]))
                    return false;

                if (IsCubeColliding(cubes[i]))
                    return false;

                if (IsCubeBottomConnected(cubes[i]))
                    isBottomConnected = true;
            }

            if (!isBottomConnected)
                return false;

            if (!IsPieceImage4Connected(piece))
                return false;

            return true;
        }

        public void PlacePieceToMatrix(Piece piece)
        {
            ActivateVisualizationCubes(false);

            Transform[] cubes = piece.Cubes;
            for (int i = 0; i < cubes.Length; i++)
            {
                PlaceCubeToMatrix(cubes[i], _matrix);
            }
        }

        public void ShowPieceProjection(Piece piece)
        {
            ActivateVisualizationCubes(true);

            Transform[] cubes = piece.Cubes;
            for (int i = 0; i < cubes.Length; i++)
            {
                _placementVisCubes[i].position = cubes[i].position;
                PlaceCubeToMatrix(_placementVisCubes[i], _auxMatrix);
            }
        }

        /// <summary>
        /// This method will detect and clear all full rows present in the grid.
        /// </summary>
        public void ClearFullRows()
        {
            CoroutineHolder.Instance.StartCoroutine(ClearRowsCoroutine());
        }

        #endregion

        #region NON-MODIFIYNG METHODS 

        private void InitPlacementVisCubes()
        {
            for (int i = 0; i < MaxCubesPerPiece; i++)
            {
                GameObject cube = GameObject.Instantiate(_placementVisPrefab, _cubeHolder);
                cube.name = "Visualization cube";
                cube.SetActive(false);

                _placementVisCubes.Add(cube.transform);
            }
        }
        private void VisualizeGrid()
        {
            for (int y = 0; y < _dimensions.y; y++)
            {
                for (int x = 0; x < _dimensions.x; x++)
                {
                    for (int z = 0; z < _dimensions.z; z++)
                    {
                        Transform cell = GameObject.Instantiate(_cellVisPrefab, _cubeHolder).transform;
                        cell.localPosition = new Vector3(x, y, z) * PieceSpawner.PieceScale;
                        cell.localScale = Vector3.one * 0.85f * PieceSpawner.PieceScale;

                        _auxMatrix[y][x] = cell;
                    }
                }
            }
        }

        private Vector3Int GetMatrixCoordinates(Transform cube)
        {
            Vector3 localPos = cube.position - _cubeHolder.position;

            int xPos = Mathf.RoundToInt(localPos.x / PieceSpawner.PieceScale);
            int yPos = Mathf.RoundToInt(localPos.y / PieceSpawner.PieceScale);
            int zPos = Mathf.RoundToInt(localPos.z / PieceSpawner.PieceScale);

            return new Vector3Int(xPos, yPos, zPos);
        }

        private bool IsCellEmpty(int x, int y, int z, Transform[][] matrix)
        {
            return matrix[y][x] == null;
        }

        private bool IsCellEmpty(int x, int y, int z)
        {
            return IsCellEmpty(x, y, z, _matrix);
        }

        private bool IsCellEmpty(Vector3Int matrixPos)
        {
            return IsCellEmpty(matrixPos.x, matrixPos.y, matrixPos.z);
        }

        private bool IsCellEmpty(Vector3Int matrixPos, Transform[][] matrix)
        {
            return IsCellEmpty(matrixPos.x, matrixPos.y, matrixPos.z, matrix);
        }

        private bool IsRowEmpty(int row)
        {
            for (int x = 0; x < _dimensions.x; x++)
            {
                if (!IsCellEmpty(x, row, 0))
                    return false;
            }

            return true;
        }

        private bool IsPositionInBounds(int x, int y, int z)
        {
            bool xBounds = x >= 0 && x < _dimensions.x;
            bool yBounds = y >= 0 && y < _dimensions.y;
            bool zBounds = z >= 0 && z < _dimensions.z;

            return xBounds && yBounds && zBounds;
        }

        private bool IsPositionInBounds(Vector3Int matrixPos)
        {
            return IsPositionInBounds(matrixPos.x, matrixPos.y, matrixPos.z);
        }

        public bool IsCubeInsideMatrix(Transform cube)
        {
            Vector3Int cubeCoordinates = GetMatrixCoordinates(cube);
            return IsPositionInBounds(cubeCoordinates);
        }

        public bool IsCubeColliding(Transform cube)
        {
            Vector3Int cubeCoordinates = GetMatrixCoordinates(cube);
            return !IsCellEmpty(cubeCoordinates);
        }

        public bool IsCubeBottomConnected(Transform cube)
        {
            Vector3Int cubeCoordinates = GetMatrixCoordinates(cube);

            int xx = cubeCoordinates.x;
            int yy = cubeCoordinates.y - 1;
            int zz = cubeCoordinates.z;

            if (!IsPositionInBounds(xx, yy, zz))
                return true;

            if (!IsCellEmpty(xx, yy, zz))
                return true;

            return false;
        }

        public int Get4NeighbourCount(Transform cube)
        {
            return Get4NeighbourCount(cube, _matrix);
        }

        public int Get4NeighbourCount(Transform cube, Transform[][] matrix)
        {
            int count = 0;
            
            Vector3Int cubeCoordinates = GetMatrixCoordinates(cube);

            int x = 0;
            int y = 0;
            for (x = -1; x <= 1; x += 2)
            {
                int xx = cubeCoordinates.x + x;
                int yy = cubeCoordinates.y + y;
                int zz = cubeCoordinates.z + 0;

                if (x == 0 && y == 0)
                    continue;

                if (!IsPositionInBounds(xx, yy, zz))
                    continue;

                if (!IsCellEmpty(xx, yy, zz, matrix))
                    count++;
            }

            x = 0;
            y = 0;
            for (y = -1; y <= 1; y += 2)
            {
                int xx = cubeCoordinates.x + x;
                int yy = cubeCoordinates.y + y;
                int zz = cubeCoordinates.z + 0;

                if (x == 0 && y == 0)
                    continue;

                if (!IsPositionInBounds(xx, yy, zz))
                    continue;

                if (!IsCellEmpty(xx, yy, zz, matrix))
                    count++;
            }

            return count;
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

            ClearMatrixReferences(_auxMatrix);
            ShowPieceProjection(piece);     // Creates an image in _auxMatrix

            int neighbouring = 0;
            for (int i = 0; i < piece.Cubes.Length; i++)
            {
                neighbouring += Get4NeighbourCount(_placementVisCubes[i], _auxMatrix);
            }

            if (neighbouring < 6)
            {
                ClearMatrixReferences(_auxMatrix);
                return false;
            }

            return true;
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

        private bool DetectGameOver()
        {
            return !IsRowEmpty(_dimensions.y - 1);
        }

        private void ActivateVisualizationCubes(bool enable)
        {
            for (int i = 0; i < _placementVisCubes.Count; i++)
            {
                _placementVisCubes[i].gameObject.SetActive(enable);
            }
        }

        #endregion

        #region MODIFIYNG METHODS

        private void ClearMatrixReferences(Transform[][] matrix)
        {
            for (int y = 0; y < _dimensions.y; y++)
            {
                for (int x = 0; x < _dimensions.x; x++)
                {
                    for (int z = 0; z < _dimensions.z; z++)
                    {
                        matrix[y][x] = null;
                    }
                }
            }
        }

        public void PlaceCubeToMatrix(Transform cube, Transform[][] matrix)
        {
            cube.SetParent(_cubeHolder);

            Vector3Int cubeCoordinates = GetMatrixCoordinates(cube);
            matrix[cubeCoordinates.y][cubeCoordinates.x] = cube;

            cube.localPosition = (Vector3)cubeCoordinates * PieceSpawner.PieceScale;
            cube.rotation = Quaternion.identity;
        }

        private IEnumerator ClearRowsCoroutine()
        {
            int firstClearedRow = -1;
            int clearedRows = 0;
            for (int y = 0; y < _dimensions.y; y++)
            {
                if (DetectRowClear(y))
                {
                    if (firstClearedRow == -1)
                        firstClearedRow = y;

                    clearedRows++;
                }
            }

            if (clearedRows > 0)
            {
                for (int i = 0; i < clearedRows; i++)
                {
                    ClearRow(firstClearedRow + i);
                }

                ScoreTracker.Instance.RowClearScored(clearedRows);

                yield return new WaitForSeconds(RowClearTime);
                ShiftRowsAfterRowClear(firstClearedRow, clearedRows);
            }
        }

        private void ClearRow(int row)
        {
            for (int x = 0; x < _dimensions.x; x++)
            {
                PieceVisualComponent.AnimateClearedCube(_matrix[row][x]);

                //Destroy(_matrix[row][x].gameObject, 2);
                _matrix[row][x] = null;
            }
        }

        private void ShiftRowsAfterRowClear(int clearedRow, int shift)
        {
            for (int y = clearedRow + 1; y < _dimensions.y; y++)
            {
                for (int x = 0; x < _dimensions.x; x++)
                {
                    if (_matrix[y][x] == null)
                        continue;

                    _matrix[y][x].position -= shift * Vector3.up * PieceSpawner.PieceScale;   // position
                    _matrix[y - shift][x] = _matrix[y][x];                                      // reference
                    _matrix[y][x] = null;
                }
            }
        }

        /// <summary>
        /// Add penalty row to the bottom of the matrix. 
        /// Returns true if it caused game over.
        /// </summary>
        /// <returns></returns>
        private bool AddPenaltyRow()
        {
            if (DetectGameOver())
                return true;

            for (int y = (_dimensions.y - 2); y >= 0; y--)
            {
                for (int x = 0; x < _dimensions.x; x++)
                {
                    if (_matrix[y][x] == null)
                        continue;

                    _matrix[y][x].position += Vector3.up * PieceSpawner.PieceScale;   // position
                    _matrix[y + 1][x] = _matrix[y][x];                                  // reference
                    _matrix[y][x] = null;
                }
            }

            return false;
        }

        #endregion
    }
}
