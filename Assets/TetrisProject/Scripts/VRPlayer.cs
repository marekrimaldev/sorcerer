using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRTetris
{
    public class VRPlayer : MonoBehaviourSingleton<VRPlayer>
    {
        public static System.Action<Piece> OnPieceDropped;

        public void PiecePicked(SelectEnterEventArgs args)
        {
            IXRInteractable interactable = args.interactableObject;
            Piece piece = interactable.transform.GetComponentInParent<Piece>();

            if(piece != null)
            {
                piece.Grab();
            }
        }

        public void PieceDropped(SelectExitEventArgs args)
        {
            IXRInteractable interactable = args.interactableObject;

            Piece piece = interactable.transform.GetComponent<Piece>();
            if(piece != null)
                OnPieceDropped?.Invoke(piece);
        }
    }
}
