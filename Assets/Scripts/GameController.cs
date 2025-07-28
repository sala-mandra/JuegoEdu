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
    
    private enum Phase
    {
        Collecting,
        Arranging,
        Finished
    }

    private Phase _currentPhase = Phase.Collecting;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        StartPhase();
    }

    public void AddCollectedPart(GameObject collectObj)
    {
        if (_currentPhase != Phase.Collecting)
        {
            Debug.LogWarning("Esta no es la fase actual");
            return;
        }

        var list = _partsCollectedPlayers[_currentTurn].PrefabsParts;
        if (list.Count < AmountSpawn)
        {
            list.Add(collectObj);
            Debug.Log($"Pieza recolectada (turno {_currentTurn + 1}): {list.Count}/{AmountSpawn}");

            if (list.Count >= AmountSpawn)
            {
                NextCollectingTurn();
            }
        }
    }

    private void NextCollectingTurn()
    {
        _currentTurn++;
        StartPhase();
    }

    private void StartPhase()
    {
        switch (_currentPhase)
        {
            case Phase.Collecting:
                StartCollectingTurn();
                break;
            case Phase.Arranging:
                StartArrangingTurn();
                break;
            case Phase.Finished:
                Debug.Log("Juego terminado");
                break;
        }
    }

    private void StartCollectingTurn()
    {
        if (_currentTurn < _totalPlayers)
        {
            Debug.Log($"[Collect] turno {_currentTurn + 1}");
            SpawnSeedsController.Instance.GeneratePartsAround(_partsToSpawn[_currentTurn].PrefabsParts);
        }
        else
        {
            Debug.Log("Primera ronda completa");
            _currentTurn = 0;
            _currentPhase = Phase.Arranging;
            StartPhase();
        }
    }

    private void StartArrangingTurn()
    {
        if (_currentTurn < _totalPlayers)
        {
            Debug.Log($"[Armado] turno {_currentTurn + 1}");
            
        }
    }
//------------------------------------------------------------------------------------------------------------------------------
    public void StartGame()
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
    
    // public void AddCollectedPart(GameObject collectObj)
    // {
    //     if (_currentTurn >= _partsCollectedPlayers.Count)
    //     {
    //         Debug.Log("Ya no hay turnos");
    //         return;
    //     }
    //
    //     var collectedList = _partsCollectedPlayers[_currentTurn].PrefabsParts;
    //     if (collectedList.Count < AmountSpawn)
    //     {
    //         collectedList.Add(collectObj);
    //         Debug.Log($"Pieza de turno {_currentTurn + 1}: {collectedList.Count} / {AmountSpawn}");
    //         if (collectedList.Count >= AmountSpawn)
    //         {
    //             NextTurn();
    //         }
    //     }
    // }
    
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