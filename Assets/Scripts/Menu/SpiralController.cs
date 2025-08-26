using System.Collections;
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
        if (_soLevelSpiral.Level > 0)
        {
            RestoreSpiralsState();
        }
        StartDialogue();
        _nextDialogueButtonGirl.onClick.AddListener(OnNextClicked);
        _nextDialogueButtonBoy.onClick.AddListener(OnNextClicked);
    }
    
    private void RestoreSpiralsState()
    {
        for (var i = 0; i < _soLevelSpiral.Level - 1; i++)
        {
            _goSpirals[i].SetActive(true);
            var spiralImage = _goSpirals[i].GetComponent<Image>();
            spiralImage.fillAmount = 1f;
        }
        
        for (var i = 0; i < _soLevelSpiral.Level; i++)
        {
            _buttonsLevels[i].interactable = true;
        }
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
            data.CharacterTwoLine.ImageCharacter.sprite = data.CharacterOneLine.IdleSprite;
        }
        else if (_dialogueStep == 1)
        {
            data.CharacterOneLine.TextBoxCharacter.transform.parent.gameObject.SetActive(false);
            data.CharacterTwoLine.TextBoxCharacter.text = data.CharacterTwoLine.TextLine;
            data.CharacterTwoLine.TextBoxCharacter.transform.parent.gameObject.SetActive(true);
            data.CharacterTwoLine.ImageCharacter.sprite = data.CharacterTwoLine.TalkingSprite;
            data.CharacterOneLine.ImageCharacter.sprite = data.CharacterTwoLine.IdleSprite;
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
        if (_soLevelSpiral.Level > 0 && _soLevelSpiral.Level <= _soLevelSpiral.MaxLevel)
        {
            var completedIndex = _soLevelSpiral.Level - 1;
            if (completedIndex >= 0 && completedIndex < _goSpirals.Length)
            {
                var spiralImage = _goSpirals[completedIndex].GetComponent<Image>();
                StartCoroutine(AnimationFilledSpiral(spiralImage));
            }
        }
        else if (_soLevelSpiral.Level == 0)
        {
            _buttonsLevels[_soLevelSpiral.Level].interactable = true;
        }
    }

    public void EndLevelOne()
    {
        StartDialogue();
    }
    
    private IEnumerator AnimationFilledSpiral(Image spiral)
    {
        var maxValue = 1f;
        var currentValueFill = spiral.fillAmount;
        while (currentValueFill < maxValue)
        {
            currentValueFill += _speedAnimator * Time.deltaTime;
            spiral.fillAmount = Mathf.Min(currentValueFill, maxValue);
            yield return null;
        }

        if (_soLevelSpiral.Level < _buttonsLevels.Length)
        {
            _buttonsLevels[_soLevelSpiral.Level].interactable = true;
        }
        
        if (_soLevelSpiral.Level == _soLevelSpiral.MaxLevel)
        {
            yield return new WaitForSeconds(1.5f);
            FinalEvent();
        }
    }
    
    private void FinalEvent()
    {
        _panelFinalGame.SetActive(true);
    }
}