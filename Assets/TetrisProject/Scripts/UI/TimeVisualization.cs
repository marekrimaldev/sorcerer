using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace VRTetris
{
    public class TimeVisualization : MonoBehaviour
    {
        [SerializeField] private int _resolution;
        [SerializeField] private float _radius;
        [SerializeField] private LineRenderer _lr;

        private Coroutine _timerCoroutine;
        private List<Vector3> _points = new List<Vector3>();

        public Action OnTimesUp;

        public void StartTimer(float time)
        {
            if(_timerCoroutine != null)
            {
                Debug.LogWarning("Timer has not been stopped");
            }

            _timerCoroutine = StartCoroutine(CircleAnimationCoroutine(time));
        }

        public void StopTimer()
        {
            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
                _timerCoroutine = null;
            }
        }

        private void GenerateCircle(float tMax)
        {
            _points.Clear();

            float t = 0;
            float step = (2 * Mathf.PI) / _resolution;
            while (t < tMax)
            {
                float x = _radius * Mathf.Cos(t);
                float z = _radius * Mathf.Sin(t);
                Vector3 point = transform.position + new Vector3(x, 0, z);

                _points.Add(point);

                t += step;
            }

            _lr.positionCount = _points.Count;
            _lr.SetPositions(_points.ToArray());
        }

        private IEnumerator CircleAnimationCoroutine(float time)
        {
            float t0 = Time.time;
            float elapsedTime = 0;

            while (elapsedTime <= time)
            {
                elapsedTime = Time.time - t0;
                float t = elapsedTime / time;
                float tt = Mathf.Lerp(2*Mathf.PI, 0, t);
                GenerateCircle(tt);

                yield return null;
            }

            OnTimesUp?.Invoke();
        }
    }
}
