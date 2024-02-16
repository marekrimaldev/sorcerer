using UnityEngine;

public class OnHitEffectManager : MonoBehaviour
{
    [SerializeField] private OnHitEffect[] _onHitEffects;

    public void ApplyEffects(Vector3 hitPoint, Transform hitTransform)
    {
        for (int i = 0; i < _onHitEffects.Length; i++)
        {
            _onHitEffects[i].ApplyEffect(hitPoint, hitTransform);
        }
    }
}
