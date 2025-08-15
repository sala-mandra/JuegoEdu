using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectWantedController : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private DataWantedObject DataWantedObjectRef;
    [SerializeField] private ObjectFoundController _objectFoundController;

    public void OnPointerDown(PointerEventData eventData)
    {
        GameControllerLevel1.Instance.ShowPanelDescription(DataWantedObjectRef);
        GameControllerLevel1.Instance.PlayEffectSound();
        _objectFoundController.ObjectFound();
    }
}