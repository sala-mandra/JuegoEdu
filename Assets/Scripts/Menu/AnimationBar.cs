using System.Collections;
using UnityEngine;

public class AnimationBar : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rateDown;
    [SerializeField] private float _rateDistance;

    private Vector3 _positionOrigin;
    private Vector3 _positionTarget;
    private Coroutine _coroutineAnimation;

    private void Awake()
    {
        _positionOrigin = transform.position;
        _positionTarget = _positionOrigin;
    }

    public void AnimationInBar(bool isIn)
    {
        if (isIn)
        {
            _positionTarget.y -= _rateDown;
        }
        else
        {
            _positionTarget.y += _rateDown;
        }
        
        if (_coroutineAnimation == null)
        {
            _coroutineAnimation = StartCoroutine(AnimationBarMove());
        }
    }

    private IEnumerator AnimationBarMove()
    {
        while (Vector2.Distance(transform.position, _positionTarget) > _rateDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, _positionTarget, _speed); 
            yield return null;
        }

        _coroutineAnimation = null;
    }
}