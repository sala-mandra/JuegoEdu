using UnityEngine;
using UnityEngine.UI;

public class SpiralController : MonoBehaviour
{
    public static SpiralController Instance;
    
    [SerializeField] private Button[] _buttonsLevels;
    [SerializeField] private GameObject[] _buttonsLevelsComplete;
    [SerializeField] private SOLevelSpiral _soLevelSpiral;

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
    }

    public void LevelComplete()
    {
        if (_soLevelSpiral.Level < _buttonsLevels.Length)
        {
            _soLevelSpiral.Level++;
            UpdateStateSpiral();
        }
    }
    
    private void UpdateStateSpiral()
    {
        for (var i = 0; i < _soLevelSpiral.Level; i++)
        {
            _buttonsLevels[i].interactable = true;
            _buttonsLevelsComplete[i].SetActive(true);
        }
    }
}