using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public int AmountSpawn = 5;
    
    [SerializeField] List<ObjectsToSpawn> _partsToSpawn;
    [SerializeField] List<ObjectsToSpawn> _partsCollectedPlayers;

    private int _currentTurn;
    private int _totalPlayers => _partsToSpawn.Count;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        StartTurn();
    }

    private void StartTurn()
    {
        if (_currentTurn < _totalPlayers)
        {
            Debug.Log($"Start turn {_currentTurn + 1}");
            SpawnSeedsController.Instance.GeneratePartsAround(_partsToSpawn[_currentTurn].PrefabsParts);
        }
        else
        {
            EndGame();
        }
    }

    public void AddCollectedPart(GameObject collectObj)
    {
        if (_currentTurn >= _partsCollectedPlayers.Count)
        {
            Debug.Log("Ya no hay turnos");
            return;
        }

        var collectedList = _partsCollectedPlayers[_currentTurn].PrefabsParts;
        if (collectedList.Count < AmountSpawn)
        {
            collectedList.Add(collectObj);
            Debug.Log($"Pieza de turno {_currentTurn + 1}: {collectedList.Count} / {AmountSpawn}");
            if (collectedList.Count >= AmountSpawn)
            {
                NextTurn();
            }
        }
    }

    private void NextTurn()
    {
        _currentTurn++;
        StartTurn();
    }
    
    private void EndGame()
    {
        Debug.Log("Ya ha terminado la primera ronda");
    }
}