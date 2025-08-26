using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerLevel1 : MonoBehaviour
{
    public static GameControllerLevel1 Instance;

    [SerializeField] private SOLevelSpiral _soLevelSpiral;
    
    [Header("Objects for level 1 enable")]
    [SerializeField] private GameObject _panelForLevelOne;
    [SerializeField] private GameObject _panelMenuSpiral;

    [SerializeField] private Image[] _objectToFound;
    [SerializeField] private GameObject _panelDescription;
    [SerializeField] private TextMeshProUGUI _textNameObject;
    [SerializeField] private TextMeshProUGUI _textSecondNameObject;
    [SerializeField] private TextMeshProUGUI _textDescription;
    [SerializeField] private Image _imageBackgroundDescription;
    [SerializeField] private AudioClip _effectSound;
    [SerializeField] private AudioClip _effectFinalSound;
    [SerializeField] private AudioSource _audioSourceLevel1;
    [SerializeField] private int _TotalNumberObjects = 5;
    [SerializeField] private GameObject _panelFinalGame;

    private AudioClip _currentAudioNameObject;
    private List<IDDesiredObject> _foundObjectsList = new List<IDDesiredObject>();
    private int _idLevel = 1;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void RestartLevelOne()
    {
        foreach (var objectTemp in _objectToFound)
        {
            var colorTemp = objectTemp.color;
            colorTemp.a = 0.6f;
            objectTemp.color = colorTemp;
        }
        _foundObjectsList.Clear();
    }

    public void ShowPanelDescription(DataWantedObject dataObject)
    {
        _imageBackgroundDescription.sprite = dataObject.ImageOfObject;
        _currentAudioNameObject = dataObject.ClipNameObject;
        _textNameObject.text = dataObject.NameObject;
        _textSecondNameObject.text = dataObject.SecondNameObject;
        _textDescription.text = dataObject.DescriptionObject;
        _panelDescription.SetActive(true);
        CheckListObjectsFound(dataObject.Id);
    }

    public void LoadLevel(string nameLevel)
    {
        SceneManager.LoadScene(nameLevel);
    }

    public void PlayEffectSound()
    {
        _audioSourceLevel1.PlayOneShot(_effectSound);
    }

    public void PlaySoundNameObject()
    {
        _audioSourceLevel1.PlayOneShot(_currentAudioNameObject);
    }

    public void CountFindedObjects()
    {
        if (_foundObjectsList.Count >= _TotalNumberObjects)
        {
            Debug.Log("Encontro todos los objetos, se termino el juego");
            _panelFinalGame.SetActive(true);
            _audioSourceLevel1.PlayOneShot(_effectFinalSound);
            EndLevel();
        }
    }
    
    private void EndLevel()
    {
        LevelsController.Instance.CompleteLevel(_idLevel);
        SpiralController.Instance.EndLevelOne();
    }

    private void CheckListObjectsFound(IDDesiredObject id)
    {
        if (!_foundObjectsList.Contains(id))
        {
            _foundObjectsList.Add(id);
        }
    }
}