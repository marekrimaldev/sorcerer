using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTetris
{
    public class CoroutineHolder : MonoBehaviour
    {
        private static CoroutineHolder _instance;
        public static CoroutineHolder Instance => _instance;

        private void Awake()
        {
            if(_instance != null && _instance != this)
            {
                Destroy(this);
            }
            else
            {
                _instance = this;
            }
        }
    }
}
