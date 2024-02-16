using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class MenuItem : MonoBehaviour, ITraversable
{
    [SerializeField] private bool _isSelectAvailable = true;
    [SerializeField] private UIDisplay[] _uiDisplays;
    private List<ISelectable> _aditionalSelectables = new List<ISelectable>();

    [Header("Events")]
    [SerializeField] private UnityEvent OnSelect;
    [SerializeField] private UnityEvent OnUnavailable;
    [SerializeField] private UnityEvent OnHoverEnter;
    [SerializeField] private UnityEvent OnHoverExit;

    public ITraversable LeftSuccessor { get; set; }
    public ITraversable RightSuccessor { get; set; }
    public ITraversable UpSuccessor { get; set; }
    public ITraversable DownSuccessor { get; set; }

    private void Awake()
    {
        SetDisplaysHoverStop();
    }

    public void OnHoverStart()
    {
        OnHoverEnter?.Invoke();
        SetDisplaysHoverStart();
    }

    public void OnHoverStop()
    {
        if (!gameObject.activeSelf)
            return;

        OnHoverExit?.Invoke();
        SetDisplaysHoverStop();
    }

    public void Select()
    {
        if (!gameObject.activeSelf)
            return;

        if (_isSelectAvailable)
        {
            OnSelect?.Invoke();
            SelectSelectables();
        }
        else
        {
            OnUnavailable?.Invoke();
        }
    }

    /// <summary>
    /// Additional selectables can be added using this method.
    /// Selecting this selectable also calls Select() on all added selectables
    /// Using this method it is possible to dynamicaly add more behaviour at runtime
    /// </summary>
    /// <param name="selectable"></param>
    public void AddSelectable(ISelectable selectable)
    {
        _aditionalSelectables.Add(selectable);
    }

    public void SetAvailable(bool isAvailable)
    {
        _isSelectAvailable = isAvailable;
        SetDisplaysHoverStop();
    }

    private void SetDisplaysHoverStart()
    {
        for (int i = 0; i < _uiDisplays.Length; i++)
        {
            _uiDisplays[i].StartHover(_isSelectAvailable);
        }
    }

    private void SetDisplaysHoverStop()
    {
        for (int i = 0; i < _uiDisplays.Length; i++)
        {
            _uiDisplays[i].StopHover(_isSelectAvailable);
        }
    }

    private void SelectSelectables()
    {
        for (int i = 0; i < _aditionalSelectables.Count; i++)
        {
            _aditionalSelectables[i].Select();
        }
    }
}
