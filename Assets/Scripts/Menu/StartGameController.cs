using UnityEngine;
using UnityEngine.Video;

public class StartGameController : MonoBehaviour
{
    [SerializeField] private SOLevelSpiral _soLevelSpiral;
    [SerializeField] private GameObject _containerStartGame;
    [SerializeField] private GameObject _panelMenuSpiral;

    private void Start()
    {
        EnableCorrectsPanels();
    }

    private void EnableCorrectsPanels()
    {
        if (_soLevelSpiral.Level == 0)
        {
            _containerStartGame.SetActive(true);
            _panelMenuSpiral.SetActive(false);
        }
        else
        {
            _panelMenuSpiral.SetActive(true);
            _containerStartGame.SetActive(false);
        }
    }
}
