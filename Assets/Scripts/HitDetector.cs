using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetector : MonoBehaviour
{
    [SerializeField] private Color _color;
    [SerializeField] private VFXController _vfxControllerPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Spell>() == null)
            return;

        VFXController vfx = Instantiate(_vfxControllerPrefab, other.transform.position, Quaternion.identity);
        vfx.SetColor(_color);
        vfx.DestroyVFXInSeconds(3);

        Rigidbody rb = other.gameObject.GetComponentInParent<Rigidbody>();
        if (rb != null)
        {
            vfx.transform.forward = -(transform.right * rb.velocity.x);
        }
    }
}
