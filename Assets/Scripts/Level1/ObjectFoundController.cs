using UnityEngine;
using UnityEngine.UI;

public class ObjectFoundController : MonoBehaviour
{
    private Image _imageComponent;

    private void Awake()
    {
        _imageComponent = GetComponent<Image>();
    }

    public void ObjectFound()
    {
        var colorTemp = _imageComponent.color;
        colorTemp.a = 1f;
        _imageComponent.color = colorTemp;
    }
}