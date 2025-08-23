using System;
using UnityEngine;

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
    }
}
