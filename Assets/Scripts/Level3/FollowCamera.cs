using System;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private float _distanceFromCamera = 2.5f;
    [SerializeField] private bool _useRotation;
    [SerializeField] private bool _useScale;
    [SerializeField] private Vector3 _offset = Vector3.zero;
    [SerializeField] private Vector3 _rotationOffSet = Vector3.zero;
    [SerializeField] private Vector3 _scaleOffSet = Vector3.one;
    
    private Transform _cameraTransform;

    private void Awake()
    {
        _cameraTransform = Camera.main.transform;
    }

    private void Start()
    {
        SetNewScale();
    }

    [ContextMenu("SetTransform")]
    public void SetNewScale()
    {
        if (_useScale)
        {
            transform.localScale = Vector3.Scale(Vector3.one, _scaleOffSet);
        }
    }

    private void Update()
    {
        if (_cameraTransform == null) return;
        
        var targetPosition = _cameraTransform.position 
                             + _cameraTransform.forward * _distanceFromCamera 
                             + _cameraTransform.right * _offset.x 
                             + _cameraTransform.up * _offset.y
                             + _cameraTransform.forward * _offset.z;
        transform.position = targetPosition;
        
        if (_useRotation)
        {
            var baseRotation = _cameraTransform.rotation;
            var offsetRotation = Quaternion.Euler(_rotationOffSet);
            transform.rotation = baseRotation * offsetRotation;
        }
    }
}