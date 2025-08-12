using System.Collections;
using UnityEngine;

public class AnimationGrowthPlants : MonoBehaviour
{
    [SerializeField] private float _speedAnimation = 0.02f;
    [SerializeField] private float _rateDownPlant;
    [SerializeField] private float _rateDistanceUpPlant;
    
    private Vector3 _originPos;

    private void Awake()
    {
        _originPos = transform.localPosition;
        SetDownPosition();
    }

    [ContextMenu("Start")]
    public void AnimationGrowth()
    {
        StartCoroutine(StartAnimationPlantGrowth());
    }

    private void SetDownPosition()
    {
        var pos = transform.localPosition;
        pos.y = _rateDownPlant;
        transform.localPosition = pos;
    }

    private IEnumerator StartAnimationPlantGrowth()
    {
        while (Vector3.Distance(_originPos, transform.localPosition) > _rateDistanceUpPlant)
        {
            yield return null;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _originPos, _speedAnimation);
        }
    }
}