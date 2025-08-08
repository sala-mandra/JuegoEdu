using System.Collections;
using UnityEngine;

public class SeedController : MonoBehaviour
{
    public TypeObject TypeObjectDrag;

    [SerializeField] private string _nameObject;
    [SerializeField] private string _nameTwoObject;
    [SerializeField] private string _descriptionObject;
    [SerializeField] private Sprite _backgroundObject;
    [SerializeField] private float _speedAnimation;
    [SerializeField] private float _minDistance = 0.5f;
    [SerializeField] private LayerMask _layerBaseGuide;
 
    private Transform _positionPlayer;
    private bool _collected;
    private Coroutine _coroutineCollect;

    private bool _isDragging;
    private Camera _mainCamera;
    private Coroutine _coroutineMoveToTarget;
    private bool _inTarget;
    private bool _overTarget;
    private BaseGuideController _currentBaseGuideSelected;
    private Vector2 currentScreenPos;
    
    private void Start()
    {
        _mainCamera = Camera.main;
        _positionPlayer = GameObject.FindWithTag("Player").transform;
    }

    private void OnMouseDown()
    {
        if (GameController.Instance.CurrentPhase == Phase.Collecting)
        {
            GameController.Instance.ShowNameObject(_nameObject, _nameTwoObject, _descriptionObject, _backgroundObject);
            if (_coroutineCollect == null)
            {
                _coroutineCollect = StartCoroutine(StartCollect());
            }
        }
        else if (GameController.Instance.CurrentPhase == Phase.Arranging)
        {
            _isDragging = true;
            GetComponent<FollowCamera>().enabled = false;
        }
    }

    private void OnMouseDrag()
    {
        if (GameController.Instance.CurrentPhase == Phase.Arranging)
        {
            if (_isDragging)
            {
                currentScreenPos = Input.mousePosition;
                FollowPointer(currentScreenPos);
                RaycastToTarget(currentScreenPos);
            }
        }
    }

    private void OnMouseUp()
    {
        _isDragging = false;
        if (GameController.Instance.CurrentPhase == Phase.Arranging)
        {
            GetComponent<FollowCamera>().enabled = true;
        }
        if (_overTarget)
        {
            if (_currentBaseGuideSelected.CheckObjectDrop(TypeObjectDrag))
            {
                var transformBaseTemp = _currentBaseGuideSelected.transform;
                GetComponent<FollowCamera>().enabled = false;
                StartCoroutine(MoveToTarget(transformBaseTemp));
                _overTarget = false;
                GameController.Instance.EnableNextDragObject();
            }
        }
    }

    private IEnumerator StartCollect()
    {
        while (!_collected)
        {
            transform.position = Vector3.MoveTowards(transform.position, _positionPlayer.position, _speedAnimation);
            var distance = Vector3.Distance(transform.position, _positionPlayer.position);
            if (distance <= _minDistance)
            {
                _collected = true;
            }
            yield return null;
        }
        
        GameController.Instance.AddCollectedPart(gameObject); 
        _coroutineCollect = null;
        gameObject.SetActive(false);
    }

    private void FollowPointer(Vector2 screenPos)
    {
        var worldPos = _mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, _mainCamera.WorldToScreenPoint(transform.position).z));
        transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);
    }
    
    private void RaycastToTarget(Vector2 screenPos)
    {
        var ray = _mainCamera.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100, _layerBaseGuide))
        {
            if (hit.transform.GetComponent<BaseGuideController>())
            {
                Debug.Log("Sobre un Target: " + hit.collider.name);
                _currentBaseGuideSelected = hit.transform.GetComponent<BaseGuideController>();
                _overTarget = true;
            }
            else
            {
                _overTarget = false;
                _currentBaseGuideSelected = null;
            }
        }
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
    }

    private IEnumerator MoveToTarget(Transform targetPosition)
    {
        while (!_inTarget)
        {
            if (Vector3.Distance(transform.position, targetPosition.position) <= 0.5f)
            {
                _inTarget = true;
                targetPosition.gameObject.SetActive(false);
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, _speedAnimation);
            yield return null;
        }
        if (TypeObjectDrag == TypeObject.Water)
        {
            GameController.Instance.EnableAnimationPlantedPlants();
        }
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}