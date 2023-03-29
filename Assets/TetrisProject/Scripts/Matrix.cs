using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTetris
{
    public class Matrix : MonoBehaviour
    {
        private Vector3Int _dimensions;
        private Transform[][] _matrix;

        private const float RowClearTime = 1f;

        public void InitMatrix(Vector3Int dimensions)
        {
            _dimensions = dimensions;

            _matrix = new Transform[_dimensions.y][];
            for (int i = 0; i < _dimensions.y; i++)
            {
                _matrix[i] = new Transform[_dimensions.x];
            }
        }

        public void PlaceCubeToMatrix(Transform cube)
        {
            cube.SetParent(transform);
            Vector3Int cubeCoordinates = GetMatrixCoordinates(cube);
            _matrix[cubeCoordinates.y][cubeCoordinates.x] = cube;
            cube.localPosition = Vector3.Scale(cubeCoordinates, cube.lossyScale);
            cube.rotation = Quaternion.identity;
        }

        public void PlacePieceToMatrix(Piece piece)
        {
            Transform[] cubes = piece.Cubes;
            for (int i = 0; i < cubes.Length; i++)
            {
                PlaceCubeToMatrix(cubes[i]);
            }
        }

        public void DrawMatrix(GameObject cellPrefab)
        {
            for (int y = 0; y < _dimensions.y; y++)
            {
                for (int x = 0; x < _dimensions.x; x++)
                {
                    for (int z = 0; z < _dimensions.z; z++)
                    {
                        Transform cell = Instantiate(cellPrefab, transform).transform;
                        cell.localPosition = new Vector3(x, y, z) * PieceGenerator.PieceScale;
                        cell.localScale = Vector3.one * 0.85f * PieceGenerator.PieceScale;

                        _matrix[y][x] = cell;
                    }
                }
            }
        }

        /// <summary>
        /// This method will not destroy any objects
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

        public void DetectRowClears()
        {
            StartCoroutine(ClearRowsCoroutine());
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

            if(clearedRows > 0)
            {
                for (int i = 0; i < clearedRows; i++)
                {
                    ClearRow(firstClearedRow + i);
                    ScoreTracker.Instance.RowClearScored();
                }

                yield return new WaitForSeconds(RowClearTime);
                ShiftRowsAfterClearRow(firstClearedRow, clearedRows);
            }
        }

        public Vector3Int GetMatrixCoordinates(Transform cube)
        {
            Vector3 localPos = cube.position - transform.position;

            int xPos = Mathf.RoundToInt(localPos.x / cube.lossyScale.x);
            int yPos = Mathf.RoundToInt(localPos.y / cube.lossyScale.y);
            int zPos = Mathf.RoundToInt(localPos.z / cube.lossyScale.z);

            return new Vector3Int(xPos, yPos, zPos);
        }

        public bool IsCellEmpty(int x, int y, int z)
        {
            return _matrix[y][x] == null;
        }

        public bool IsCellEmpty(Vector3Int matrixPos)
        {
            return IsCellEmpty(matrixPos.x, matrixPos.y, matrixPos.z);
        }

        public bool IsPositionInBounds(int x, int y, int z)
        {
            bool xBounds = x >= 0 && x < _dimensions.x;
            bool yBounds = y >= 0 && y < _dimensions.y;
            bool zBounds = z >= 0 && z < _dimensions.z;

            return xBounds && yBounds && zBounds;
        }

        public bool IsPositionInBounds(Vector3Int matrixPos)
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

        public int Get8NeighbourCount(Transform cube)
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

        public bool DetectRowClear(int row)
        {
            for (int x = 0; x < _dimensions.x; x++)
            {
                if (IsCellEmpty(x, row, 0))
                    return false;
            }

            return true;
        }

        private void ShiftRowsAfterClearRow(int clearedRow, int shift)
        {
            Debug.Log("ShiftRowsAfterClearRow");

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

        private void ClearRow(int row)
        {
            for (int x = 0; x < _dimensions.x; x++)
            {
                // Probably some effect here

                Destroy(_matrix[row][x].gameObject);
                _matrix[row][x] = null;
            }
        }
    }
}
