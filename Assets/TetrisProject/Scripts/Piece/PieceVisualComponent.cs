using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTetris
{
    [RequireComponent(typeof(Piece))]
    public class PieceVisualComponent : MonoBehaviour
    {
        [Header("Color")]
        [SerializeField] private float _dullMultiplier = .7f;
        [SerializeField] private float _baseMultiplier = .85f;
        [SerializeField] private float _brightMultiplier = 1f;
        [SerializeField] private float _emissionIntensity = .5f;
        [SerializeField] private Material _pieceMaterial;
        [SerializeField] private Color[] _colors;

        [Header("Lock In Efffect")]
        [SerializeField] private float _lockInFadeInDuration = .25f;
        [SerializeField] private float _lockInFadeOutDuration = .75f;
        [SerializeField] private VFXController _lockInVfx;

        [Header("Row Clear Effect")]
        [SerializeField] private float _throwForceStatic;
        [SerializeField] private float _torqueForceStatic;

        [Header("Destroy Effect")]
        [SerializeField] private float _minDestroyTime = 0f;
        [SerializeField] private float _maxDestroyTime = 0.5f;
        [SerializeField] private VFXController _destroyVfxStatic;

        // Static
        private static float ThrowForce;
        private static float TorqueForce;
        private static VFXController DestroyVfxStatic;
        private static float MinDestroyTime;
        private static float MaxDestroyTime;

        private Piece _piece;
        private Color _color;
        private float _colorMultiplier;
        private MeshRenderer[] _mrs;

        #region STATIC

        public static void AnimateClearedCube(Transform cube)
        {
            Rigidbody rb = cube.gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.AddForce(Random.onUnitSphere * ThrowForce);
            rb.AddTorque(Random.onUnitSphere * TorqueForce);

            float destroyDelay = Random.Range(MinDestroyTime, MaxDestroyTime);
            CoroutineHolder.Instance.StartCoroutine(DestroyCubeWithDelay(cube, destroyDelay));
        }

        private static IEnumerator DestroyCubeWithDelay(Transform cube, float delay)
        {
            yield return new WaitForSeconds(delay);
            DestroyCube(cube);
        }

        private static void DestroyCube(Transform cube)
        {
            VFXController vfx = Instantiate(DestroyVfxStatic, cube.position, Quaternion.identity);
            vfx.SetColor(cube.GetComponentInChildren<MeshRenderer>().material.color);
            //vfx.DestroyVFXInSeconds(2);

            Destroy(cube.gameObject);
        }

        #endregion

        private void Awake()
        {
            _piece = GetComponent<Piece>();
            _mrs = GetComponentsInChildren<MeshRenderer>();

            ThrowForce = _throwForceStatic;
            TorqueForce = _torqueForceStatic;
            DestroyVfxStatic = _destroyVfxStatic;
            MinDestroyTime = _minDestroyTime;
            MaxDestroyTime = _maxDestroyTime;
        }

        private void Start()
        {
            int idx = Random.Range(0, _colors.Length);
            SetMaterial(_pieceMaterial);
            SetColor(_colors[idx], _baseMultiplier);
        }

        public void OnPieceLockIn()
        {
            //for (int i = 0; i < _piece.Cubes.Length; i++)
            //{
            //    Instantiate(_lockInVfx, _piece.Cubes[i].position, _piece.Cubes[i].rotation);
            //    _lockInVfx.DestroyVFXInSeconds(2);
            //}

            StartCoroutine(PieceLockInAnimationCoroutine());
        }

        private void SetMaterial(Material mat)
        {
            for (int i = 0; i < _mrs.Length; i++)
            {
                _mrs[i].material = mat;
            }
        }

        private void SetColor(Color color, float colorMultiplier)
        {
            _color = color;
            _colorMultiplier = colorMultiplier;

            for (int i = 0; i < _mrs.Length; i++)
            {
                if (_mrs[i] == null)
                    break;  // Immediate row clear

                _mrs[i].material.color = _color * _colorMultiplier;
                _mrs[i].material.SetColor("_EmissionColor", _color * _emissionIntensity);
            }
        }

        private IEnumerator PieceLockInAnimationCoroutine()
        {
            yield return FadeColorBrightnessCoroutine(_colorMultiplier, _brightMultiplier, _lockInFadeInDuration);
            yield return FadeColorBrightnessCoroutine(_colorMultiplier, _dullMultiplier, _lockInFadeOutDuration);
        }

        private IEnumerator FadeColorBrightnessCoroutine(float b1, float b2, float duration)
        {
            float t = 0;
            while (t <= 1)
            {
                float b = Mathf.Lerp(b1, b2, t);
                SetColor(_color, b);

                yield return null;

                t += Time.deltaTime / duration;
            }
        }
    }
}
