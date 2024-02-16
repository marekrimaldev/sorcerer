using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : Menu
{
    [SerializeField] private GameObject _inGameMenuCanvas;
    private Canvas[] _otherSceneCanvases;

    private void Awake()
    {
        _otherSceneCanvases = FindObjectsOfType<Canvas>();
    }

    public void EnableSceneCanvases(bool enable)
    {
        if (!enable)
        {
            for (int i = 0; i < _otherSceneCanvases.Length; i++)
            {
                if (_otherSceneCanvases[i].gameObject == _inGameMenuCanvas)
                    continue;

                _otherSceneCanvases[i].gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < _otherSceneCanvases.Length; i++)
            {
                if (_otherSceneCanvases[i].gameObject == _inGameMenuCanvas)
                    continue;

                _otherSceneCanvases[i].gameObject.SetActive(true);
            }
        }
    }
}
