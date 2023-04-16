using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTetris
{
    public class CubeDestroyer : MonoBehaviour
    {
        [SerializeField] private float _minDestroyTime = 0f;
        [SerializeField] private float _maxDestroyTime = 0.5f;

        //private void OnTriggerEnter(Collider other)
        //{
        //    float destroyTime = Random.Range(_minDestroyTime, _maxDestroyTime);
        //    StartCoroutine(DestroyWithDelay(other.transform, destroyTime));
        //}

        //private IEnumerator DestroyWithDelay(Transform t, float delay)
        //{
        //    yield return new WaitForSeconds(delay);
        //    PieceVisualComponent.DestroyCube(t);
        //}
    }
}
