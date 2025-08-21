using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DialogueData
{
    public DialogueLine CharacterOneLine;
    public DialogueLine CharacterTwoLine;
}

[Serializable]
public class DialogueLine
{
    public string TextLine;
    public Sprite TalkingSprite;
    public Sprite IdleSprite;
    public TextMeshProUGUI TextBoxCharacter;
    public Image ImageCharacter;
}