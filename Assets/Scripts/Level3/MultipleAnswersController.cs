using System.Collections;
using TMPro;
using UnityEngine;

public class MultipleAnswersController : MonoBehaviour
{
    public static MultipleAnswersController Instance;
    public int IndexQuestion { get; set; }

    [SerializeField] private CampfireController _campfireController;
    [SerializeField] private TextMeshProUGUI[] _textsOptionAnswer;
    [SerializeField] private Questions[] _questions;
    [SerializeField] private GameObject _panelAnswerLong;
    [SerializeField] private GameObject _panelMultipleAnswer;
    [SerializeField] private float _timeForWaitNextQuestion = 3f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SetAnswers()
    {
        var answersTemp = _questions[IndexQuestion];

        for (var i = 0; i < _textsOptionAnswer.Length; i++)
        {
            _textsOptionAnswer[i].GetComponentInParent<AnswerController>().RestarAnswer();
            _textsOptionAnswer[i].GetComponentInParent<AnswerController>().enabled = true;
            _textsOptionAnswer[i].text = answersTemp.Options[i];
        }
    }
    
    public void AnswerSelected(int selectedOption)
    {
        var q = _questions[IndexQuestion];

        for (var i = 0; i < _textsOptionAnswer.Length; i++)
        {
            var currentOption = _textsOptionAnswer[i].GetComponentInParent<AnswerController>();
            
            if (i == q.CorrectAnswerIndex)
                currentOption.CorrectAnswer();
            else if (selectedOption == i)
                currentOption.InCorrectAnswer();
        }

        StartCoroutine(TimeForViewAnswerResult());
        Debug.Log($"Respuest seleccionada: {selectedOption}");
    }

    public void DisableAnswer()
    {
        for (var i = 0; i < _textsOptionAnswer.Length; i++)
        {
            _textsOptionAnswer[i].GetComponentInParent<AnswerController>().enabled = false;
        }
    }
    
    private IEnumerator TimeForViewAnswerResult()
    {
        yield return new WaitForSeconds(_timeForWaitNextQuestion);
        _panelMultipleAnswer.SetActive(false);
        _panelAnswerLong.SetActive(true);
        _campfireController.ListenAnswer();
    }
}