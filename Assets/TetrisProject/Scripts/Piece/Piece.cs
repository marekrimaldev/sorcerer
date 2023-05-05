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
        public System.Action<Piece> OnPieceLocked;
        [SerializeField] private UnityEvent OnPieceGrabbedUE;
        [SerializeField] private UnityEvent OnPieceLockedUE;

        private PieceVisualComponent _visualComponent;

        private void Awake()
        {
            _visualComponent = GetComponent<PieceVisualComponent>();
        }

        public void SetInteractability(bool val)
        {
            XRGrabInteractable interactable = GetComponentInChildren<XRGrabInteractable>();
            if (interactable != null)
                interactable.enabled = val;
        }

        public void LockIn()
        {
            SetInteractability(false);

            _visualComponent.OnPieceLockIn();

            OnPieceLocked?.Invoke(this);
            OnPieceLockedUE?.Invoke();
        }

        public void Grab()
        {
            OnPieceGrabbed?.Invoke(this);
            OnPieceGrabbedUE?.Invoke();
        }
    }
}
