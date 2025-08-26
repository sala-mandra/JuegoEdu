using UnityEngine;

public class LevelsController : MonoBehaviour
{
    public static LevelsController Instance;

    [SerializeField] private SOLevelSpiral _soLevelSpiral;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void CompleteLevel(int idLevel)
    {
        if (!_soLevelSpiral.LevelsComplete.Contains(idLevel))
        {
            _soLevelSpiral.LevelsComplete.Add(idLevel);
            _soLevelSpiral.Level++;
        }
    }
}