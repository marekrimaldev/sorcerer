using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRTetris
{
    public class VRPlayer : MonoBehaviour
    {
        private static VRPlayer _instance;
        public static VRPlayer Instance => _instance;

        public static System.Action<Piece> OnPieceDropped;

        private void Awake()
        {
            // SINGLETON
            if(_instance != null && _instance != this)
                Destroy(this);
            else
                _instance = this;
        }

        public void PiecePicked(SelectEnterEventArgs args)
        {
            IXRInteractable interactable = args.interactableObject;
            Piece piece = interactable.transform.GetComponentInParent<Piece>();

            if(piece != null)
            {
                piece.PieceGrabbed();
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
