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
        private Transform _cubeHolder;  // Used as a parent to all cubes in the grid

        private const float RowClearTime = .5f;

        #region PUBLIC INTERFACE

        public Matrix(Vector3 origin, Vector3Int dimensions)
        {
            _origin = origin;
            _dimensions = dimensions;

            _cubeHolder = new GameObject("Cube Holder").transform;
            _cubeHolder.transform.position = _origin;

            _matrix = new Transform[_dimensions.y][];
            for (int i = 0; i < _dimensions.y; i++)
            {
                _matrix[i] = new Transform[_dimensions.x];
            }
        }

        public void PlacePieceToMatrix(Piece piece)
        {
            Transform[] cubes = piece.Cubes;
            for (int i = 0; i < cubes.Length; i++)
            {
                PlaceCubeToMatrix(cubes[i]);
            }
        }

        /// <summary>
        /// Use this method to visualize the grid.
        /// It fills all the cells with the provided prefab.
        /// </summary>
        /// <param name="cellPrefab"></param>
        public void FillMatrix(GameObject cellPrefab)
        {
            for (int y = 0; y < _dimensions.y; y++)
            {
                for (int x = 0; x < _dimensions.x; x++)
                {
                    for (int z = 0; z < _dimensions.z; z++)
                    {
                        Transform cell = GameObject.Instantiate(cellPrefab, _cubeHolder).transform;
                        cell.localPosition = new Vector3(x, y, z) * PieceGenerator.PieceScale;
                        cell.localScale = Vector3.one * 0.85f * PieceGenerator.PieceScale;

                        _matrix[y][x] = cell;
                    }
                }
            }
        }

        /// <summary>
        /// This method will not destroy any objects. It just removes references.
        /// </summary>
        public void ClearMatrix()
        {
            for (int y = 0; y < _dimensions.y; y++)
            {
                for (int x = 0; x < _dimensions.x; x++)
                {
                    for (int z = 0; z < _dimensions.z; z++)
                    {
                        _matrix[y][x] = null;
                    }
                }
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

        private Vector3Int GetMatrixCoordinates(Transform cube)
        {
            Vector3 localPos = cube.position - _cubeHolder.position;

            int xPos = Mathf.RoundToInt(localPos.x / PieceGenerator.PieceScale);
            int yPos = Mathf.RoundToInt(localPos.y / PieceGenerator.PieceScale);
            int zPos = Mathf.RoundToInt(localPos.z / PieceGenerator.PieceScale);

            return new Vector3Int(xPos, yPos, zPos);
        }

        private bool IsCellEmpty(int x, int y, int z)
        {
            return _matrix[y][x] == null;
        }

        private bool IsCellEmpty(Vector3Int matrixPos)
        {
            return IsCellEmpty(matrixPos.x, matrixPos.y, matrixPos.z);
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

                if (!IsCellEmpty(xx, yy, zz))
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

                if (!IsCellEmpty(xx, yy, zz))
                    count++;
            }

            return count;
        }

        private int Get8NeighbourCount(Transform cube)
        {
            int count = 0;

            Vector3Int cubeCoordinates = GetMatrixCoordinates(cube);

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int xx = cubeCoordinates.x + x;
                    int yy = cubeCoordinates.y + y;
                    int zz = cubeCoordinates.z + 0;

                    if (x == 0 && y == 0)
                        continue;

                    if (!IsPositionInBounds(xx, yy, zz))
                        continue;

                    if (!IsCellEmpty(xx, yy, zz))
                        count++;
                }
            }

            return count;
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

        #endregion

        #region MODIFIYNG METHODS

        public void PlaceCubeToMatrix(Transform cube)
        {
            cube.SetParent(_cubeHolder);

            Vector3Int cubeCoordinates = GetMatrixCoordinates(cube);
            _matrix[cubeCoordinates.y][cubeCoordinates.x] = cube;

            cube.localPosition = (Vector3)cubeCoordinates * PieceGenerator.PieceScale;
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

                    _matrix[y][x].position -= shift * Vector3.up * PieceGenerator.PieceScale;   // position
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

                    _matrix[y][x].position += Vector3.up * PieceGenerator.PieceScale;   // position
                    _matrix[y + 1][x] = _matrix[y][x];                                  // reference
                    _matrix[y][x] = null;
                }
            }

            return false;
        }

        #endregion
    }
}
