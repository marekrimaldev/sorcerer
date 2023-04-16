using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTetris
{
    public class ThreadmillPieceProvider : PlayerPieceProvider
    {
        [SerializeField] private PieceThreadmill[] _threadmills;

        public override void AddPiece(Piece piece)
        {
            int idx = Random.Range(0, _threadmills.Length);
            _threadmills[idx].AddPiece(piece);
        }
    }
}
