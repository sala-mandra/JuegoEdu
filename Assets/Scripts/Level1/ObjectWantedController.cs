using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectWantedController : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private AudioClip _clipNameObject;
    [SerializeField] private string _textNameObject;
    [SerializeField] private string _textDescription;
    [SerializeField] private Sprite _imageOfObject;

    public void OnPointerDown(PointerEventData eventData)
    {
        GameControllerLevel1.Instance.ShowPanelDescription(_textNameObject, _clipNameObject, _imageOfObject, _textDescription);
        GameControllerLevel1.Instance.PlayEffectSound();
    }
}
