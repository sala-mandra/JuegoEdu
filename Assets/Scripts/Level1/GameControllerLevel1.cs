using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameControllerLevel1 : MonoBehaviour
{
    public static GameControllerLevel1 Instance;

    [Header("Objects for level 1 enable")] 
    [SerializeField] private GameObject _panelForLevelOne;
    [SerializeField] private GameObject _panelMenuSpiral;
    
    [SerializeField] private GameObject _panelDescription;
    [SerializeField] private TextMeshProUGUI _textNameObject;
    [SerializeField] private TextMeshProUGUI _textSecondNameObject;
    [SerializeField] private TextMeshProUGUI _textDescription;
    [SerializeField] private Image _imageBackgroundDescription;
    [SerializeField] private AudioClip _effectSound;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private int _TotalNumberObjects = 5;
    [SerializeField] private GameObject _panelFinalGame;

    private AudioClip _currentAudioNameObject;
    private int _amountObjectsInScene;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ShowPanelDescription(string textToName, string textSecondName, AudioClip audioName, Sprite imageObject, string description)
    {
        _imageBackgroundDescription.sprite = imageObject;
        _currentAudioNameObject = audioName;
        _textNameObject.text = textToName;
        _textSecondNameObject.text = textSecondName;
        _textDescription.text = description;
        _panelDescription.SetActive(true);
        _amountObjectsInScene++;
    }

    public void LoadLevel(string nameLevel)
    {
        SceneManager.LoadScene(nameLevel);
    }

    public void PlayEffectSound()
    {
        _audioSource.PlayOneShot(_effectSound);
    }

    public void PlaySoundNameObject()
    {
        _audioSource.PlayOneShot(_currentAudioNameObject);
    }

    public void CountFindedObjects()
    {
        if (_amountObjectsInScene >= _TotalNumberObjects)
        {
            Debug.Log("Encontro todos los objetos, se termino el juego");
            _panelFinalGame.SetActive(true);
        }
    }
    
    public void CompleteLevel()
    {
        SpiralController.Instance.LevelComplete();
        _panelForLevelOne.SetActive(false);
        _panelMenuSpiral.SetActive(true);
    }
}