using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CampfireController : MonoBehaviour
{
    [SerializeField] private List<QuestionsAndAnswers> _questions;
    [SerializeField] private ParticleSystem _fireInCampfire;
    [SerializeField] private GameObject _panelQuestion;

    [Header("Objects of questions")] 
    [SerializeField] private TextMeshProUGUI _titleQuestion;
    [SerializeField] private TextMeshProUGUI _titleAnswer;

    private DragAndDrop _dragAndDrop;
    private FollowCamera _followCamera;
    private int _currentQuestion;

    private void Awake()
    {
        _dragAndDrop = GetComponent<DragAndDrop>();
        _followCamera = GetComponent<FollowCamera>();
    }

    private void Start()
    {
        _dragAndDrop.OnDropObject += IsCorrectPosition;
    }

    public void NextQuestion()
    {
        if (_currentQuestion < _questions.Count - 1)
        {
            _currentQuestion++;
            SetQuestion();
        }
        else
        {
            Debug.Log("Termino el juego");
        }
    }

    private void IsCorrectPosition(GameObject goTemp)
    {
        _followCamera.enabled = false;
        transform.position = goTemp.transform.position;
        transform.localScale = goTemp.transform.localScale;
        _fireInCampfire.Play();
        SetQuestion();
        _panelQuestion.SetActive(true);
    }

    private void SetQuestion()
    {
        _titleQuestion.text = _questions[_currentQuestion].Question;
        _titleAnswer.text = _questions[_currentQuestion].Answer;
    }
    
    private void OnDisable()
    {
        _dragAndDrop.OnDropObject -= IsCorrectPosition;
    }
}