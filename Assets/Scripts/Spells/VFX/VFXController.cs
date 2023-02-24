using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// This class purpose is to make comunication between Spell and its VFX easier and more abstract.
/// </summary>
public class VFXController : MonoBehaviour
{
    [SerializeField] protected VisualEffect _visualEffect;

    public void EnableVFX(bool val)
    {
        _visualEffect.gameObject.SetActive(val);
    }
}
