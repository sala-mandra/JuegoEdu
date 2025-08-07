using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerLevel1 : MonoBehaviour
{
    public static GameControllerLevel1 Instance;

    [Header("Objects for level 1 enable")] 
    [SerializeField] private GameObject _panelForLevelOne;
    [SerializeField] private GameObject _panelMenuSpiral;
    
    [SerializeField] private GameObject _panelDescription;
    [SerializeField] private TextMeshProUGUI _textNameObject;
    [SerializeField] private TextMeshProUGUI _textDescription;
    [SerializeField] private Image _imageOfObjectSelected;
    [SerializeField] private AudioClip _effectSound;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private int _TotalNumberObjects = 5;

    private AudioClip _currentAudioNameObject;
    private int _amountObjectsInScene;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ShowPanelDescription(string textToName, AudioClip audioName, Sprite imageObject, string description)
    {
        _imageOfObjectSelected.sprite = imageObject;
        _currentAudioNameObject = audioName;
        _textNameObject.text = textToName;
        _textDescription.text = description;
        _panelDescription.SetActive(true);
        CountFindedObjects();
    }

    public void PlayEffectSound()
    {
        _audioSource.PlayOneShot(_effectSound);
    }

    public void PlaySoundNameObject()
    {
        _audioSource.PlayOneShot(_currentAudioNameObject);
    }

    private void CountFindedObjects()
    {
        if (_amountObjectsInScene >= _TotalNumberObjects)
        {
            Debug.Log("Encontro todos los objetos, se termino el juego");
            CompleteLevel();
        }
    }
    
    private void CompleteLevel()
    {
        SpiralController.Instance.LevelComplete();
        _panelForLevelOne.SetActive(false);
        _panelMenuSpiral.SetActive(true);
    }
}