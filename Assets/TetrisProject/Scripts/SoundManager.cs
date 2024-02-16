using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTetris
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundPlayer _clickSoundPlayer;

        private static SoundManager _instance;
        public static SoundManager Instance => _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this);
                return;
            }

            _instance = this;
        }
    }
}
