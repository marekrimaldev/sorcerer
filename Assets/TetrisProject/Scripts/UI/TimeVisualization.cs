using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTetris
{
    public class TimeVisualization : MonoBehaviour
    {
        [SerializeField] private int _resolution;
        [SerializeField] private float _radius;
        [SerializeField] private LineRenderer _lr;
        [SerializeField] private float _time = 3;

        private List<Vector3> _points = new List<Vector3>();

        private void Awake()
        {
            GenerateCircle();
            AnimateCircle();
        }

        private void GenerateCircle()
        {
            float t = 0;
            float step = (2 * Mathf.PI) / _resolution;
            for (int i = 0; i <= _resolution; i++)
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

        private void AnimateCircle()
        {
            StartCoroutine(CircleAnimationCoroutine());
        }

        private IEnumerator CircleAnimationCoroutine()
        {
            yield return new WaitForSeconds(3);

            Debug.Log(Time.time);

            float wsTime = _time / _resolution;
            WaitForSecondsRealtime ws = new WaitForSecondsRealtime(wsTime);
            while (true)
            {
                if (_points.Count < 1)
                    break;

                _points.RemoveAt(0);
                _lr.positionCount = _points.Count;
                _lr.SetPositions(_points.ToArray());

                yield return ws;
            }

            Debug.Log(Time.time);
        }
    }
}
