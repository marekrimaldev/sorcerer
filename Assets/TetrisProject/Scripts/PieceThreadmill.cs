using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VRTetris
{
    public class PieceThreadmill : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _endPoint;

        private Vector3 _threadmillDirection;

        private List<Piece> _pieces = new List<Piece>();

        public System.Action<Piece> OnPieceCollision;

        private void Awake()
        {
            _threadmillDirection = (_endPoint.position - _startPoint.position).normalized;
        }

        public void AddPiece(Piece piece)
        {
            Debug.Log("Adding piece");
            piece.transform.position = _startPoint.position;
            _pieces.Add(piece);

            piece.OnPieceGrabbed += RemovePiece;
        }

        private void RemovePiece(Piece piece)
        {
            Debug.Log("Removing piece");
            _pieces.Remove(piece);

            piece.OnPieceGrabbed -= RemovePiece;
        }

        private void Update()
        {
            TranslatePieces();
        }

        private void TranslatePieces()
        {
            int piecesAtEnd = 0;
            for (int i = 0; i < _pieces.Count; i++)
            {
                if (Vector3.Distance(_pieces[i].transform.position, _endPoint.position) < .1f)
                {
                    piecesAtEnd++;
                    continue;
                }

                _pieces[i].transform.position += _speed * _threadmillDirection * Time.deltaTime;
            }

            if(piecesAtEnd > 1)
            {
                Debug.Log("PIECE COLLISION");
            }
        }
    }
}
