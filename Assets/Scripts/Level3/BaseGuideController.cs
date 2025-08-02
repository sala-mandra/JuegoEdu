using UnityEngine;

public class BaseGuideController : MonoBehaviour
{
    [SerializeField] private TypeObject _objectBase;
    [SerializeField] private GameObject _objectToActive;

    public bool CheckObjectDrop(TypeObject objectDropType)
    {
        if (_objectBase == objectDropType)
        {
            transform.gameObject.SetActive(false);
            if (_objectToActive)
            {
                _objectToActive.SetActive(true);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}