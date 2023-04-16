using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRTetris
{
    [RequireComponent(typeof(PieceVisualComponent))]
    public class Piece : MonoBehaviour
    {
        [SerializeField] private Transform[] _cubes;

        public Transform[] Cubes => _cubes;

        public System.Action<Piece> OnPieceGrabbed;
        [SerializeField] private UnityEvent OnPieceGrabbedUE;
        [SerializeField] private UnityEvent OnPieceLockedUE;

        private PieceVisualComponent _colorController;

        private void Awake()
        {
            _colorController = GetComponent<PieceVisualComponent>();
        }

        public void LockIn()
        {
            XRGrabInteractable interactable = GetComponentInChildren<XRGrabInteractable>();
            if (interactable != null)
                interactable.enabled = false;

            _colorController.OnPieceLockIn();

            OnPieceLockedUE?.Invoke();
        }

        public void PieceGrabbed()
        {
            OnPieceGrabbed?.Invoke(this);
            OnPieceGrabbedUE?.Invoke();
        }
    }
}
