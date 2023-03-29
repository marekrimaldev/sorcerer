using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRTetris
{
    public class Piece : MonoBehaviour
    {
        [SerializeField] private Transform[] _cubes;

        public Transform[] Cubes => _cubes;

        public System.Action<Piece> OnPieceGrabbed;

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
        }

        public void PieceGrabbed()
        {
            OnPieceGrabbed?.Invoke(this);
        }
    }
}
