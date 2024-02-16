using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject[] _subMenus;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowMainMenu();
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        _mainMenu.SetActive(true);

        for (int i = 0; i < _subMenus.Length; i++)
        {
            _subMenus[i].SetActive(false);
        }
    }
}
