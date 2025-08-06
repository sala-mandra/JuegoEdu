using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private float _distanceFromCamera = 2.5f;
    [SerializeField] private Vector3 _offset = Vector3.zero;
    [SerializeField] private bool _useRotation;
    [SerializeField] private Vector3 _rotationOffSet = Vector3.zero;
    
    private Transform _cameraTransform;

    private void Awake()
    {
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (_cameraTransform == null) return;
        
        var targetPosition = _cameraTransform.position + _cameraTransform.forward * _distanceFromCamera + _offset;
        transform.position = targetPosition;

        if (_useRotation)
        {
            var baseRotation = _cameraTransform.rotation;
            var offsetRotation = Quaternion.Euler(_rotationOffSet);
            transform.rotation = baseRotation * offsetRotation;
        }
    }
}