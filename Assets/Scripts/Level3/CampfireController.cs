using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class CampfireController : MonoBehaviour
{
    [SerializeField] private List<QuestionsAndAnswers> _questions;
    [SerializeField] private ParticleSystem _fireInCampfire;
    [SerializeField] private GameObject _panelQuestion;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _effectFinalSound;
    [SerializeField] private GameObject _panelFinal;

    [Header("Objects of questions")] 
    [SerializeField] private TextMeshProUGUI _titleQuestion;
    [SerializeField] private TextMeshProUGUI _textQuestion;
    [SerializeField] private TextMeshProUGUI _textAnswer;
    [SerializeField] private GameObject[] _buttonsQuestions;

    private DragAndDrop _dragAndDrop;
    private FollowCamera _followCamera;
    private BoxCollider _colliderFire;
    private int _currentQuestion;

    private void Awake()
    {
        _colliderFire = GetComponent<BoxCollider>();
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
            _panelFinal.SetActive(true);
            _audioSource.PlayOneShot(_effectFinalSound);
        }
    }

    public void ListenAnswer()
    {
        var audioAnswerTemp = _questions[_currentQuestion].AudioAnswer;
        _audioSource.PlayOneShot(audioAnswerTemp);
    }

    public void EnableButtonNextQuestion()
    {
        _buttonsQuestions[_currentQuestion].SetActive(true);
    }

    public void LoadNewScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }

    private void IsCorrectPosition(GameObject goTemp)
    {
        _colliderFire.enabled = false;
        _followCamera.enabled = false;
        _dragAndDrop.enabled = false;
        transform.position = goTemp.transform.position;
        transform.localScale = goTemp.transform.localScale;
        transform.rotation = goTemp.transform.rotation;
        _fireInCampfire.gameObject.SetActive(true);
        _fireInCampfire.Play();
        SetQuestion();
        EnableButtonNextQuestion();
    }

    private void SetQuestion()
    {
        _titleQuestion.text = _questions[_currentQuestion].TitleQuestion;
        _textQuestion.text = _questions[_currentQuestion].Question;
        _textAnswer.text = _questions[_currentQuestion].Answer;
    }
    
    private void OnDisable()
    {
        _dragAndDrop.OnDropObject -= IsCorrectPosition;
    }
}