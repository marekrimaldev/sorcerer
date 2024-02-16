using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OnHitMesh", menuName = "On Hit Effects/Mesh", order = 1)]
public class OnHitMesh : OnHitEffect
{
    [SerializeField] private MeshVFXController _meshVFXControllerPrefab;
    [SerializeField] private float _duration = 1.5f;
    private MeshVFXController _meshVFXController;

    public override void ApplyEffect(Vector3 hitPoint, Transform hitTransform)
    {
        if (!ShouldApplyEffect(hitTransform))
            return;

        MeshFilter mf = hitTransform.GetComponent<MeshFilter>();
        if (mf != null)
        {
            _meshVFXController = Instantiate(_meshVFXControllerPrefab, hitPoint, Quaternion.identity) as MeshVFXController;
            _meshVFXController.SetMesh(mf.mesh);
            _meshVFXController.SetMeshTransform(hitTransform);

            _meshVFXController.DestroyVFXInSeconds(_duration);
        }
    }
}
