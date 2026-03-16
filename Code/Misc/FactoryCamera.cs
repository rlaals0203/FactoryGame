using System;
using Code.Core.EventSystems;
using Code.Factory;
using Core.GameSystem;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class FactoryCamera : MonoBehaviour
{
    [SerializeField] private InputSO playerInput;
    [SerializeField] private float minFOV, maxFOV;
    [SerializeField] private float zoomAmount;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float deceleration = 5f;

    private Vector3 _currentVelocity = Vector3.zero;
    private Rigidbody _rigidbody;
    private Camera _mainCam;

    private float _targetOthor;
    private bool _canMove = false;

    private void Awake()
    {
        GameEventBus.AddListener<ChangeBuildModeEvent>(HandleChangeBuildMode);
        _rigidbody = GetComponent<Rigidbody>();
        _mainCam = Camera.main;

        Cursor.lockState = CursorLockMode.None;
    }
    
    private void OnDestroy()
    {
        GameEventBus.RemoveListener<ChangeBuildModeEvent>(HandleChangeBuildMode);
    }

    private void HandleChangeBuildMode(ChangeBuildModeEvent evt)
        => _canMove = evt.canBuild;

    private void FixedUpdate()
    {
        if (_canMove)
            MovementCamera();
    }

    private void MovementCamera()
    {
        Vector2 input = playerInput.Movement;
        Vector3 targetDir = Vector3.zero;

        if (input != Vector2.zero)
        {
            Vector3 camForward = _mainCam.transform.forward;
            Vector3 camRight = _mainCam.transform.right;
            camForward.y = 0;
            camRight.y = 0;

            camForward.Normalize();
            camRight.Normalize();

            targetDir = (camForward * input.y + camRight * input.x).normalized;
        }

        float speed = input != Vector2.zero ? moveSpeed : 0f;
        float lerpRate = input != Vector2.zero ? acceleration : deceleration;

        _currentVelocity = Vector3.Lerp(_currentVelocity, targetDir * speed, Time.deltaTime * lerpRate);
        _rigidbody.linearVelocity = _currentVelocity / Time.timeScale;
    }
}
