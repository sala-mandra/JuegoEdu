using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsController : MonoBehaviour
{
    public static LevelsController Instance;

    [SerializeField] private SOLevelSpiral _soLevelSpiral;
    
    private void Awake()
    {
        Array.Resize(ref _soLevelSpiral.LevelsComplete, _soLevelSpiral.MaxLevel);
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(Instance);
        }
        
        DontDestroyOnLoad(Instance);
    }

    public void CompleteLevel()
    {
        var levelTemp = _soLevelSpiral.LevelsComplete[_soLevelSpiral.Level];
        if (_soLevelSpiral.Level <= _soLevelSpiral.MaxLevel && !levelTemp)
        {
            _soLevelSpiral.LevelsComplete[_soLevelSpiral.Level] = true;
            _soLevelSpiral.Level++;
        }
        else if (_soLevelSpiral.Level <= _soLevelSpiral.MaxLevel)
        {
            _soLevelSpiral.Level++;
        }
    }

    public void RestartGame()
    {
        _soLevelSpiral.Level = 0;
        _soLevelSpiral.LevelsComplete = new bool[_soLevelSpiral.MaxLevel];
        SceneManager.LoadScene("MenuAndLevel1");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
