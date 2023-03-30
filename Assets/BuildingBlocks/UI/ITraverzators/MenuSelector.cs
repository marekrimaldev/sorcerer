using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MenuSelector : MonoBehaviour, ITraversator
{
    [SerializeField] private bool _startOnFirstSelectable = false;

    [SerializeField] private KeyCode _moveLeft = KeyCode.LeftArrow;
    [SerializeField] private KeyCode _moveRight = KeyCode.RightArrow;
    [SerializeField] private KeyCode _moveUp = KeyCode.UpArrow;
    [SerializeField] private KeyCode _moveDown = KeyCode.DownArrow;

    [SerializeField] private KeyCode _select = KeyCode.Return;

    private ITraversable _firstTraversable;
    private ITraversable _currTraversable;

    public void SetTraversable(ITraversable traversable)
    {
        Debug.Log("Traversable = " + traversable);

        _firstTraversable = traversable;
        _currTraversable = traversable;
        _currTraversable?.OnHoverStart();
    }

    public void ResetPosition()
    {
        _currTraversable = _firstTraversable;
        _currTraversable?.OnHoverStart();
    }

    private void OnEnable()
    {
        if (_startOnFirstSelectable)
            _currTraversable = _firstTraversable;

        _currTraversable?.OnHoverStart();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            _currTraversable.OnHoverStop();

            if (Input.GetKeyDown(_moveLeft))
                _currTraversable = _currTraversable.LeftSuccessor;
            else if (Input.GetKeyDown(_moveRight))
                _currTraversable = _currTraversable.RightSuccessor;
            else if(Input.GetKeyDown(_moveUp))
                _currTraversable = _currTraversable.UpSuccessor;
            else if(Input.GetKeyDown(_moveDown))
                _currTraversable = _currTraversable.DownSuccessor;

            _currTraversable.OnHoverStart();

            if(Input.GetKeyDown(_select))
            {
                _currTraversable.Select();
            }
        }
    }
}
