using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRTetris
{
    public class VRPlayer : MonoBehaviour
    {
        public static System.Action<Piece> OnPieceDropped;

        public void PieceDropped(SelectExitEventArgs args)
        {
            IXRInteractable interactable = args.interactableObject;

            Piece piece = interactable.transform.GetComponent<Piece>();
            if(piece != null)
                OnPieceDropped?.Invoke(piece);
        }
    }
}
