using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] _sounds;
    private AudioSource _as;

    private void Awake()
    {
        _as = GetComponent<AudioSource>();

        if(_as == null)
            CreateAudioSource();
    }

    private void CreateAudioSource()
    {
        _as = gameObject.AddComponent<AudioSource>();
        _as.playOnAwake = false;
    }

    public void PlaySound(int soundIdx)
    {
        if (soundIdx >= _sounds.Length)
            return;

        if (_as == null)
            CreateAudioSource();

        _as.PlayOneShot(_sounds[soundIdx]);
    }

    /// <summary>
    /// This method creates an persistent object with AudioSource attached and play sound on it.
    /// Use this method if you want your sounds to persist a scene change.
    /// Object is made persistend by calling DontDestroyOnLoad() 
    /// </summary>
    public void PlaySoundPersistent(int soundIdx)
    {
        if (soundIdx >= _sounds.Length)
            return;

        GameObject go = new GameObject("Persistent sound player");
        go.AddComponent<AudioSource>();
        DontDestroyOnLoad(go);
        go.GetComponent<AudioSource>().volume = 0.5f;
        go.GetComponent<AudioSource>().PlayOneShot(_sounds[soundIdx]);
        Destroy(go, _sounds[soundIdx].length + 1);
    }
}
