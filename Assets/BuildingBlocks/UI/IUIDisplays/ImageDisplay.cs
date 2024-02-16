using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageDisplay : UIDisplay
{
    [SerializeField] private Image _imageObject;

    [Header("Colors")]
    [SerializeField] private Color _selectColor = Color.yellow;
    [SerializeField] private Color _unselectColor = Color.black;
    [SerializeField] private Color _unavailableSelectColor = Color.black;
    [SerializeField] private Color _unavailableUnselectColor = Color.black;

    public void DisplayImage(Sprite sprite)
    {
        _imageObject.sprite = sprite;
    }

    public override void StartHover(bool isSelectAvailable)
    {
        if (isSelectAvailable)
            _imageObject.color = _selectColor;
        else
            _imageObject.color = _unavailableSelectColor;
    }

    public override void StopHover(bool isSelectAvailable)
    {
        if (isSelectAvailable)
            _imageObject.color = _unselectColor;
        else
            _imageObject.color = _unavailableUnselectColor;
    }
}
