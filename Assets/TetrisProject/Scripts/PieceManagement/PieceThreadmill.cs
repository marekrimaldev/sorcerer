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

        public static System.Action OnPieceCollision;

        private void Awake()
        {
            _threadmillDirection = (_endPoint.position - _startPoint.position).normalized;
        }

        public void AddPiece(Piece piece)
        {
            piece.transform.position = _startPoint.position;
            _pieces.Add(piece);

            piece.OnPieceGrabbed += RemovePiece;
        }

        private void RemovePiece(Piece piece)
        {
            _pieces.Remove(piece);

            piece.OnPieceGrabbed -= RemovePiece;
        }

        private void Update()
        {
            TranslatePieces();
        }

        private void TranslatePieces()
        {
            List<Piece> piecesAtEnd = new List<Piece>();

            for (int i = 0; i < _pieces.Count; i++)
            {
                if (Vector3.Distance(_pieces[i].transform.position, _endPoint.position) < .1f)
                {
                    piecesAtEnd.Add(_pieces[i]);
                    continue;
                }

                _pieces[i].transform.position += _speed * _threadmillDirection * Time.deltaTime;
            }


            if(piecesAtEnd.Count > 1)
            {
                OnPieceCollision?.Invoke();

                for (int i = 0; i < piecesAtEnd.Count; i++)
                {
                    _pieces.Remove(piecesAtEnd[i]);
                    Destroy(piecesAtEnd[i].gameObject);
                }
            }
        }
    }
}
