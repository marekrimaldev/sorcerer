using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// This class purpose is to make comunication between Spell and its VFX easier and more abstract.
/// </summary>
public class VFXController : MonoBehaviour
{
    [SerializeField] protected VisualEffect _visualEffect;
    [Tooltip("Put negative number if you wish to not auto destroy the effect")]

    public void EnableVFX(bool val)
    {
        _visualEffect.gameObject.SetActive(val);
    }

    public void SetColor(Color color)
    {
        _visualEffect.SetVector4("Color", color);
    }

    /// <summary>
    /// Use this method to destroy the VFX in seconds.
    /// </summary>
    public void DestroyVFXInSeconds(float seconds)
    {
        DestroyVFXAfterDuration(seconds);
    }

    /// <summary>
    /// Use this method to destroy the VFX immediately.
    /// </summary>
    public void DestroyVFXImmediate()
    {
        Destroy(gameObject);
    }

    private void DestroyVFXAfterDuration(float destroyTime)
    {
        Light[] lights = GetComponentsInChildren<Light>();
        for (int i = 0; i < lights.Length; i++)
        {
            StartCoroutine(FadeOutLightCoroutine(lights[0], destroyTime));
        }

        Invoke(nameof(DestroyVFXImmediate), destroyTime);
    }

    private IEnumerator FadeOutLightCoroutine(Light light, float duration)
    {
        float t = 0;
        float startIntensity = light.intensity;
        float startRange = light.range;
        while (t <= 1)
        {
            light.intensity = Mathf.Lerp(startIntensity, 0, t);
            light.range = Mathf.Lerp(startRange, 0, t);
            yield return null;

            t += Time.deltaTime / duration;
        }
    }
}
