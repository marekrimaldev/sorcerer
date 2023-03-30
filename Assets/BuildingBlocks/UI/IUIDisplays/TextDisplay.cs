using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDisplay : UIDisplay
{
    [SerializeField] private TMP_Text _textObject;

    [Header("Colors")]
    [SerializeField] private Color _selectColor = Color.black;
    [SerializeField] private Color _unselectColor = Color.white;
    [SerializeField] private Color _unavailableColor = Color.gray;

    public void DisplayText(string text)
    {
        _textObject.text = text;
    }

    public override void StartHover(bool isSelectAvailable)
    {
        if (isSelectAvailable)
            _textObject.color = _selectColor;
        else
            _textObject.color = _unavailableColor;
    }

    public override void StopHover(bool isSelectAvailable)
    {
        if (isSelectAvailable)
            _textObject.color = _unselectColor;
        else
            _textObject.color = _unavailableColor;
    }
}
