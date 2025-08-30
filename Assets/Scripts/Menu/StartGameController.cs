using UnityEngine;

public class StartGameController : MonoBehaviour
{
    [SerializeField] private SOLevelSpiral _soLevelSpiral;
    [SerializeField] private GameObject _containerStartGame;
    [SerializeField] private GameObject _panelMenuSpiral;
    [SerializeField] private GameObject _audioSourceMapa;
    [SerializeField] private GameObject _panelStartGame;

    private void Start()
    {
        EnableCorrectsPanels();
    }

    private void EnableCorrectsPanels()
    {
        if (_soLevelSpiral.Level == 0)
        {
            _containerStartGame.SetActive(true);
            _panelStartGame.SetActive(true);
            _panelMenuSpiral.SetActive(false);
        }
        else
        {
            _audioSourceMapa.SetActive(true);
            _panelMenuSpiral.SetActive(true);
            _containerStartGame.SetActive(false);
        }
    }
}
