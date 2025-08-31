using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnswerController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int _indexOption;
    [SerializeField] private Image _baseImageBackground;
    [SerializeField] private Sprite _correctSprite;
    [SerializeField] private Sprite _inCorrectSprite;
    [SerializeField] private Sprite _normalSprite;

    public void OnPointerClick(PointerEventData eventData)
    {
        MultipleAnswersController.Instance.AnswerSelected(_indexOption);
        MultipleAnswersController.Instance.DisableAnswer();
    }

    public void RestarAnswer()
    {
        _baseImageBackground.sprite = _normalSprite;
    }

    public void CorrectAnswer()
    {
        _baseImageBackground.sprite = _correctSprite;
    }
    
    public void InCorrectAnswer()
    {
        _baseImageBackground.sprite = _inCorrectSprite;
    }
}