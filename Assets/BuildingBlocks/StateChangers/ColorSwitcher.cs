using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwitcher : MonoBehaviour
{
    [SerializeField] private Color _color01;
    [SerializeField] private Color _color02;
    [SerializeField] private SpriteRenderer _sr;

    private void Start()
    {
        _sr.color = _color01;
    }

    public void SwitchColor()
    {
        if (_sr.color == _color01)
        {
            _sr.color = _color02;
        }
        else
        {
            _sr.color = _color01;
        }
    }
}
