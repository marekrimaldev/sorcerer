using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRTetris
{
    public class Piece : MonoBehaviour
    {
        [SerializeField] private Transform[] _cubes;

        public Transform[] Cubes => _cubes;

        public System.Action<Piece> OnPieceGrabbed;
        [SerializeField] private UnityEvent OnPieceGrabbedUE;
        [SerializeField] private UnityEvent OnPieceLockedUE;

        public void SetColor(Color color)
        {
            MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = color;
            }
        }

        public void LockIn()
        {
            XRGrabInteractable interactable = GetComponentInChildren<XRGrabInteractable>();
            if (interactable != null)
                interactable.enabled = false;

            OnPieceLockedUE?.Invoke();
        }

        public void PieceGrabbed()
        {
            OnPieceGrabbed?.Invoke(this);
            OnPieceGrabbedUE?.Invoke();
        }
    }
}
