using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTetris
{
    public class Matrix : MonoBehaviour
    {
        private Vector3Int _dimensions;
        private Transform[][] _matrix;

        public void InitMatrix(Vector3Int dimensions)
        {
            _dimensions = dimensions;

            _matrix = new Transform[_dimensions.y][];
            for (int i = 0; i < _dimensions.y; i++)
            {
                _matrix[i] = new Transform[_dimensions.x];
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

        public void DetectRowClears()
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

        private Vector3Int GetMatrixCoordinates(Transform cube)
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

        private bool IsPositionInBounds(Vector3Int matrixPos)
        {
            return IsPositionInBounds(matrixPos.x, matrixPos.y, matrixPos.z);
        }

        private bool IsCubeInsideMatrix(Transform cube)
        {
            Vector3Int cubeMatrixPos = GetMatrixCoordinates(cube);
            Debug.Log(cubeMatrixPos);
            return IsPositionInBounds(cubeMatrixPos);
        }

        private bool IsCubeColliding(Transform cube)
        {
            Vector3Int cubeMatrixPos = GetMatrixCoordinates(cube);
            return !IsCellEmpty(cubeMatrixPos);
        }

        private bool IsCube4Connected(Transform cube)
        {
            Vector3Int cubeMatrixPos = GetMatrixCoordinates(cube);

            for (int x = -1; x <= 1; x += 2)
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
            Vector3Int cubeMatrixPos = GetMatrixCoordinates(cube);
            _matrix[cubeMatrixPos.x][cubeMatrixPos.y] = cube;
            cube.localPosition = Vector3.Scale(cubeMatrixPos, cube.localScale) + cube.localScale * 0.5f;
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
    }
}
