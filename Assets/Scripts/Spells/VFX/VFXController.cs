using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// This class purpose is to make comunication between Spell and its VFX easier and more abstract.
/// </summary>
public class VFXController : MonoBehaviour
{
    [SerializeField] private VisualEffect _visualEffect;
}
