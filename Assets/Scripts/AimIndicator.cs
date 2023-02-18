using UnityEngine;

public class AimIndicator : MonoBehaviour
{
    [SerializeField] private bool _showAim = true;
    [SerializeField] protected Transform _spawnPoint;
    [SerializeField] private LineRenderer _aimRenderer;
    [SerializeField] private float _aimLineLength = 15;

    public Transform SpawnPoint => _spawnPoint;

    protected virtual void Update()
    {
        if (_showAim)
            UpdateAimRenderer();
    }

    private void UpdateAimRenderer()
    {
        _aimRenderer.positionCount = 2;
        _aimRenderer.SetPosition(0, _spawnPoint.position);
        _aimRenderer.SetPosition(1, _spawnPoint.position + _spawnPoint.forward * _aimLineLength);
    }
}
