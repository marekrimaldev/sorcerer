using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMenuBuilder : MenuBuilder
{
    [SerializeField] private int _elementsPerRow = 5;
    [SerializeField] private MenuItem[] _menuItems;

    [Header("Formating")]
    [SerializeField] private bool _useFormatiing = true;
    [SerializeField] private float _spacing = 50;
    [SerializeField] private float _uiElementSize = 100;

    public override ITraversable[] GetTraverzables()
    {
        if (_useFormatiing)
            FormateGrid();

        AssignAdjacency();
        return _menuItems;
    }

    private void FormateGrid()
    {
        float stepX = (_uiElementSize + _spacing) * _menuItems[0].GetComponent<RectTransform>().rect.width;
        float stepY = (_uiElementSize + _spacing) * _menuItems[0].GetComponent<RectTransform>().rect.height;

        int shouldShift = _elementsPerRow % 2 == 0 ? 1 : 0;   // Shift the start pos if there is even number of levels per row
        Vector2 startPos = Vector2.zero - (_elementsPerRow / 2) * stepX * Vector2.right + shouldShift * stepX * Vector2.right;

        for (int i = 0; i < _menuItems.Length; i++)
        {
            int x = i % _elementsPerRow;
            int y = i / _elementsPerRow;
            Vector2 pos = startPos + x * stepX * Vector2.right - y * stepY * Vector2.up;

            _menuItems[i].transform.localScale = new Vector2(_uiElementSize, _uiElementSize);
            _menuItems[i].transform.localPosition = pos;
        }
    }

    private void AssignAdjacency()
    {
        for (int i = 0; i < _menuItems.Length; i++)
        {
            int rowOffset = (i / _elementsPerRow) * _elementsPerRow;
            _menuItems[i].LeftSuccessor = _menuItems[rowOffset + (i + _elementsPerRow - 1) % _elementsPerRow];
            _menuItems[i].RightSuccessor = _menuItems[rowOffset + (i + _elementsPerRow + 1) % _elementsPerRow];
            _menuItems[i].UpSuccessor = _menuItems[(i + _menuItems.Length - _elementsPerRow) % _menuItems.Length];
            _menuItems[i].DownSuccessor = _menuItems[(i + _menuItems.Length + _elementsPerRow) % _menuItems.Length];
        }
    }
}
