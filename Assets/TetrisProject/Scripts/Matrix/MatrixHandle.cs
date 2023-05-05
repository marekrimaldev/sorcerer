using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class MatrixHandle : MonoBehaviour
{
    [SerializeField] private Transform _leftHandTransform;
    [SerializeField] private Transform _rightHandTransform;
    [SerializeField] private InputActionReference _leftHandGrabAction;
    [SerializeField] private InputActionReference _rightHandGrabAction;

    private Coroutine _leftHandGrabCoroutine;
    private Coroutine _rightHandGrabCoroutine;

    bool _leftHandPressed = false;
    bool _rightHandPressed = false;

    private bool _leftHandFree = true;
    private bool _rightHandFree = true;

    private const float Threshold = 0.1f;

    private void Update()
    {
        if (_leftHandFree)
            HandleLeftHand();
        else
            StopCoroutineSafe(_leftHandGrabCoroutine);

        if (_rightHandFree)
            HandleRightHand();
        else
            StopCoroutineSafe(_rightHandGrabCoroutine);
    }

    private void HandleLeftHand()
    {
        float leftHandVal = _leftHandGrabAction.action.ReadValue<float>();

        if (!_leftHandPressed && leftHandVal > Threshold)
        {
            StopCoroutineSafe(_rightHandGrabCoroutine);

            _leftHandPressed = true;
            _leftHandGrabCoroutine = StartCoroutine(TrackHandGrabCoroutine(_leftHandTransform));
        }
        else if (_leftHandPressed && leftHandVal < Threshold)
        {
            _leftHandPressed = false;
            StopCoroutineSafe(_leftHandGrabCoroutine);
        }
    }

    private void HandleRightHand()
    {
        float rightHandVal = _rightHandGrabAction.action.ReadValue<float>();

        if (!_rightHandPressed && rightHandVal > Threshold)
        {
            StopCoroutineSafe(_leftHandGrabCoroutine);

            _rightHandPressed = true;
            _rightHandGrabCoroutine = StartCoroutine(TrackHandGrabCoroutine(_rightHandTransform));
        }
        else if (_rightHandPressed && rightHandVal < Threshold)
        {
            _rightHandPressed = false;
            StopCoroutineSafe(_rightHandGrabCoroutine);
        }
    }

    private IEnumerator TrackHandGrabCoroutine(Transform handTransform)
    {
        yield return new WaitForSeconds(.05f);

        Vector3 handStartPos = handTransform.position;
        Vector3 gridStartPos = transform.position;

        while (true)
        {
            float yDiff = handTransform.position.y - handStartPos.y;
            transform.position = gridStartPos + Vector3.up * yDiff;

            yield return null;
        }
    }

    private void StopCoroutineSafe(Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    public void LeftHandGrabStart(SelectEnterEventArgs args)
    {
        _leftHandFree = false;
    }

    public void LeftHandGrabEnd(SelectExitEventArgs args)
    {
        _leftHandFree = true;
    }

    public void RightHandGrabStart(SelectEnterEventArgs args)
    {
        _rightHandFree = false;
    }

    public void RightHandGrabEnd(SelectExitEventArgs args)
    {
        _rightHandFree = true;
    }
}
