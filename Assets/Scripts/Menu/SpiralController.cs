using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpiralController : MonoBehaviour
{
    public static SpiralController Instance;
    
    [SerializeField] private Button[] _buttonsLevels;
    [SerializeField] private GameObject[] _goSpirals;
    [SerializeField] private SOLevelSpiral _soLevelSpiral;
    [SerializeField] private float _speedAnimator;
    [SerializeField] private GameObject _panelFinalGame;

    [Header("UI for characters")] 
    [SerializeField] private Button _nextDialogueButtonBoy;
    [SerializeField] private Button _nextDialogueButtonGirl;
    [SerializeField] private DialogueData[] _dialogues;

    private int _dialogueStep = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _soLevelSpiral.MaxLevel = _buttonsLevels.Length;
    }
    
    private void Start()
    {
        UpdateStateSpiral();
        StartDialogue();
        _nextDialogueButtonGirl.onClick.AddListener(OnNextClicked);
        _nextDialogueButtonBoy.onClick.AddListener(OnNextClicked);
    }

    private void StartDialogue()
    {
        _dialogueStep = 0;
        ShowCurrentDialogue();
    }

    private void ShowCurrentDialogue()
    {
        var data = _dialogues[_soLevelSpiral.Level];

        if (_dialogueStep == 0)
        {
            data.CharacterTwoLine.TextBoxCharacter.transform.parent.gameObject.SetActive(false);
            data.CharacterOneLine.TextBoxCharacter.text = data.CharacterOneLine.TextLine;
            data.CharacterOneLine.TextBoxCharacter.transform.parent.gameObject.SetActive(true);
            data.CharacterOneLine.ImageCharacter.sprite = data.CharacterOneLine.TalkingSprite;
            data.CharacterTwoLine.ImageCharacter.sprite = data.CharacterTwoLine.IdleSprite;
        }
        else if (_dialogueStep == 1)
        {
            data.CharacterOneLine.TextBoxCharacter.transform.parent.gameObject.SetActive(false);
            data.CharacterTwoLine.TextBoxCharacter.text = data.CharacterTwoLine.TextLine;
            data.CharacterTwoLine.TextBoxCharacter.transform.parent.gameObject.SetActive(true);
            data.CharacterTwoLine.ImageCharacter.sprite = data.CharacterTwoLine.TalkingSprite;
            data.CharacterOneLine.ImageCharacter.sprite = data.CharacterOneLine.IdleSprite;
        }
    }
    
    private void OnNextClicked()
    {
        _dialogueStep++;
        if (_dialogueStep > 1)
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentDialogue();
        }
    }
    
    private void EndDialogue()
    {
        //_soLevelSpiral.Level++;

        if (_soLevelSpiral.Level <= _soLevelSpiral.MaxLevel)
        {
            if (_soLevelSpiral.Level == _soLevelSpiral.MaxLevel)
            {
                FinalEvent();
            }
            else
            {
                _buttonsLevels[_soLevelSpiral.Level].interactable = true;
            }
        }
    }
    
    private void FinalEvent()
    {
        var finalData = _dialogues[_dialogues.Length - 1];

        // Cambiar sprites finales si están definidos
        // if (finalData.character1FinalSprite != null)
        //     character1Image.sprite = finalData.character1FinalSprite;
        // if (finalData.character2FinalSprite != null)
        //     character2Image.sprite = finalData.character2FinalSprite;

        _panelFinalGame.SetActive(true);
    }

    public void LevelComplete()
    {
        if (_soLevelSpiral.Level < _buttonsLevels.Length)
        {
            _soLevelSpiral.Level++;
            transform.parent.gameObject.SetActive(true);
            UpdateStateSpiral();
            StartDialogue();
        }
    }
    
    private void UpdateStateSpiral()
    {
        for (var i = 0; i < _soLevelSpiral.Level; i++)
        {
            _buttonsLevels[i].interactable = true;
            _goSpirals[i].SetActive(true);
        }

        if (_soLevelSpiral.Level > 0)
        {
            var currentImage = _goSpirals[_soLevelSpiral.Level - 1].GetComponent<Image>();
            StartCoroutine(AnimationFilledSpiral(currentImage));
        }
    }

    private IEnumerator AnimationFilledSpiral(Image spiral)
    {
        var maxValue = 1f;
        var currentValueFill = spiral.fillAmount;
        while (currentValueFill < maxValue)
        {
            currentValueFill += _speedAnimator;
            spiral.fillAmount = currentValueFill;
            yield return null;
        }
    }
}