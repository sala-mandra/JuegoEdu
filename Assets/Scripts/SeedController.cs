using System.Collections;
using UnityEngine;

public class SeedController : MonoBehaviour
{
    [SerializeField] private float _speedAnimation;
    [SerializeField] private float _minDistance = 0.5f;
 
    private Transform _positionPlayer;
    private bool _collected;
    private Coroutine _coroutineCollect;

    private void Awake()
    {
        _positionPlayer = GameObject.FindWithTag("Player").transform;
    }

    private void OnMouseDown()
    {
        if (_coroutineCollect == null)
        {
            _coroutineCollect = StartCoroutine(StartCollect());
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
}