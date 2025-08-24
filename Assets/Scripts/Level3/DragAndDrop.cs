using UnityEngine;
using System;

public class DragAndDrop : MonoBehaviour
{
    public Action<GameObject> OnDropObject;
    
    [SerializeField] private LayerMask _layerMask;
    
    private Vector2 _currentScreenPos;
    private Camera _mainCamera;
    private FollowCamera _followCamera;
    private bool _overTarget;
    private Transform _transformTarget;

    private void Awake()
    {
        _followCamera = GetComponent<FollowCamera>();
        _mainCamera = Camera.main;
    }

    private void OnMouseDown()
    {
        _followCamera.enabled = false;
    }

    private void OnMouseUp()
    {
        _followCamera.enabled = true;
        if (_overTarget)
        {
            _followCamera.enabled = false;
            OnDropObject?.Invoke(_transformTarget.gameObject);
            _transformTarget.gameObject.SetActive(false);
        }
    }

    private void OnMouseDrag()
    {
        _currentScreenPos = Input.mousePosition;
        FollowPointer(_currentScreenPos);
        RaycastToTarget(_currentScreenPos);
    }
    
    private void FollowPointer(Vector2 screenPos)
    {
        var worldPos = _mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, _mainCamera.WorldToScreenPoint(transform.position).z));
        transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);
    }
    
    private void RaycastToTarget(Vector2 screenPos)
    {
        var ray = _mainCamera.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100, _layerMask))
        {
            if (hit.transform)
            {
                _transformTarget = hit.transform;
                _overTarget = true;
            }
            else
            {
                _overTarget = false;
            }
        }
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
    }
}