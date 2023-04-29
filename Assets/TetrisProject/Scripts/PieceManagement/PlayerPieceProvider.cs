using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTetris
{
    /// <summary>
    /// Class providing generated pieces to the player
    /// </summary>
    public abstract class PlayerPieceProvider : MonoBehaviour
    {
        public abstract void AddPiece(Piece piece);
    }
}
