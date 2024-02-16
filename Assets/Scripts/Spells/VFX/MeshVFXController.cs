using System.Collections;
using UnityEngine;

public class MeshVFXController : VFXController
{
    private Transform _meshTransform;

    private const string VFXPositionPostfix = "_position";
    private const string VFXRotationPostfix = "_angles";
    private const string VFXScalePostfix = "_scale";

    public void SetMesh(Mesh mesh)
    {
        _visualEffect.SetMesh("Mesh", mesh);
    }

    public void SetMeshTransform(Transform meshTransform)
    {
        _meshTransform = meshTransform;
        StartCoroutine(UpdateMeshTransform());
    }

    private IEnumerator UpdateMeshTransform()
    {
        while (this)
        {
            string propertyName = "MeshTransform";
            string position = propertyName + VFXPositionPostfix;
            string angles = propertyName + VFXRotationPostfix;
            string scale = propertyName + VFXScalePostfix;

            _visualEffect.SetVector3(position, (_meshTransform.position - _visualEffect.transform.position));
            _visualEffect.SetVector3(angles, _meshTransform.eulerAngles);
            _visualEffect.SetVector3(scale, _meshTransform.localScale);

            yield return null;
        }
    }
}
