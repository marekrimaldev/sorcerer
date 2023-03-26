using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VRTetris
{
    public class Piece : MonoBehaviour
    {
        [SerializeField] private Transform[] _cubes;

        public Transform[] Cubes => _cubes;

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
            // Disable ineractivity
        }
    }
}
