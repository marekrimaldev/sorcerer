using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTetris
{
    public class SimplePieceProvider : PlayerPieceProvider
    {
        [SerializeField] private Transform _spawn;

        public override void AddPiece(Piece piece)
        {
            piece.transform.position = _spawn.position;
        }
    }
}
