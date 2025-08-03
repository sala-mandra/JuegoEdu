using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public Phase CurrentPhase = Phase.Collecting;
    
    [Header("Part for work in logic")]
    [SerializeField] List<ObjectsToSpawn> _partsToSpawn;
    [SerializeField] List<ObjectsToSpawn> _partsCollectedPlayers;
    [SerializeField] private GameObject[] _baseGuides;
    [SerializeField] private GameObject[] _plantedPlants;
    [SerializeField] private GameObject[] _zoneSown;
    [SerializeField] private Color[] _colorsTurn;
    [SerializeField] private TextMeshProUGUI _textTurn;
    [SerializeField] private TextMeshProUGUI _textPhase;

    [FormerlySerializedAs("_nameObject")]
    [Header("Part of panel to show name")] 
    [SerializeField] private TextMeshProUGUI _textNameObject;
    [SerializeField] private GameObject _panelToShowName;
    [SerializeField] private AudioClip _effectAudio;
    [SerializeField] private AudioSource _audioSource;
    
    private int _currentTurn;
    private int _totalPlayers => _partsToSpawn.Count;
    private int _currentCollected;

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

    public void ShowNameObject(string name)
    {
        _textNameObject.text = name;
        _audioSource.PlayOneShot(_effectAudio);
        StartCoroutine(ShowNameTemporaly());
    }

    private IEnumerator ShowNameTemporaly()
    {
        _panelToShowName.SetActive(true);
        yield return new WaitForSeconds(2f);
        _panelToShowName.SetActive(false);
    }

    public void AddCollectedPart(GameObject collectObj)
    {
        if (CurrentPhase != Phase.Collecting)
        {
            Debug.LogWarning("Esta no es la fase actual");
            return;
        }

        var amountSpawn = _partsToSpawn[_currentTurn].PrefabsParts.Count;
        var list = _partsCollectedPlayers[_currentTurn].PrefabsParts;
        if (list.Count < amountSpawn)
        {
            list.Add(collectObj);
            var currenType = collectObj.GetComponent<SeedController>().TypeObjectDrag;
            if (currenType == TypeObject.Shovel)
            {
                AddFirstOrLastPosInList(collectObj, list, true);
            }
            else
            {
                AddFirstOrLastPosInList(collectObj, list, false);
            }
            
            if (list.Count >= amountSpawn)
            {
                NextCollectingTurn();
            }
        }
    }

    private void AddFirstOrLastPosInList(GameObject objectCollect, List<GameObject> listToAdd, bool first)
    {
        if (first)
        {
            if (listToAdd[0] != null)
            {
                var objectTempInZeroPos = listToAdd[0];
                var indexTemp = listToAdd.IndexOf(objectCollect);
                listToAdd[0] = objectCollect;
                listToAdd[indexTemp] = objectTempInZeroPos;
            }
        }
        else
        {
            var waterObject = listToAdd
                .FirstOrDefault(obj => obj != null &&
                    obj.GetComponent<SeedController>() != null &&
                    obj.GetComponent<SeedController>().TypeObjectDrag == TypeObject.Water);

            if (waterObject != null)
            {
                int waterIndex = listToAdd.IndexOf(waterObject);
                if (waterIndex != listToAdd.Count - 1)
                {
                    listToAdd.RemoveAt(waterIndex);
                    listToAdd.Add(waterObject);
                }
            }
        }
    }

    private void NextCollectingTurn()
    {
        _currentTurn++;
        StartPhase();
    }
    
    public void EnableNextDragObject()
    {
        if (CurrentPhase != Phase.Arranging)
            return;

        if (_partsCollectedPlayers[_currentTurn].PrefabsParts.Count == 0)
        {
            NextArrangingTurn();
            return;
        }
        
        var requiredPart = _partsCollectedPlayers[_currentTurn].PrefabsParts[_currentCollected];
        SetNewTransform(requiredPart);        
        requiredPart.SetActive(true);
        
        _partsCollectedPlayers[_currentTurn].PrefabsParts.RemoveAt(0);
    }

    public void EnableAnimationPlantedPlants()
    {
        foreach (var plant in _plantedPlants)
        {
            plant.SetActive(true);
        }
    }

    private void StartPhase()
    {
        switch (CurrentPhase)
        {
            case Phase.Collecting:
                StartCollectingTurn();
                break;
            case Phase.Arranging:
                StartArrangingTurn();
                break;
            case Phase.Finished:
                EndGame();
                break;
        }
        StartCoroutine(StartAnimationTextTurn());
    }

    private void StartCollectingTurn()
    {
        if (_currentTurn < _totalPlayers)
        {
            _textPhase.text = "Fase de recolección";
            _textTurn.color = _colorsTurn[_currentTurn];
            _textTurn.text = "Turno " + (_currentTurn + 1);
            SpawnSeedsController.Instance.GeneratePartsAround(_partsToSpawn[_currentTurn].PrefabsParts);
        }
        else
        {
            _currentTurn = 0;
            CurrentPhase = Phase.Arranging;
            StartPhase();
        }
    }

    private void StartArrangingTurn()
    {
        if (_currentTurn < _totalPlayers)
        {
            _textPhase.text = "Fase de armado";
            _textTurn.color = _colorsTurn[_currentTurn];
            _textTurn.text = "Turno " + (_currentTurn + 1);
            _baseGuides[_currentTurn].SetActive(true);
            StartCoroutine(EnableGuidesArranging());
        }
        else
        {
            _currentTurn = 0;
            CurrentPhase = Phase.Finished;
            StartPhase();
        }
    }

    private void SetNewTransform(GameObject objectToDrag)
    {
        objectToDrag.GetComponent<FollowCamera>().enabled = true;
        var posTemp = objectToDrag.transform.localPosition;
        posTemp.x = 0;
        posTemp.y = 0;
        posTemp.z = 0;
        objectToDrag.transform.localPosition = posTemp;

        var newRotation = objectToDrag.transform.localRotation;
        newRotation.x = 0;
        newRotation.y = 0;
        newRotation.z = 0;
        objectToDrag.transform.localRotation = newRotation;

        var newScale = objectToDrag.transform.localScale;
        newScale.x -= 0.8f;
        newScale.y -= 0.8f;
        newScale.z -= 0.8f;
        objectToDrag.transform.localScale = newScale;
    }

    private IEnumerator StartAnimationTextTurn()
    {
        _textTurn.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        _textTurn.gameObject.SetActive(false);
    }
    
    private IEnumerator EnableGuidesArranging()
    {
        yield return new WaitForSeconds(2f);
        foreach (var zone in _zoneSown)
        {
            zone.SetActive(true);
        }
        EnableNextDragObject();
    }
    
    private void NextArrangingTurn()
    {
        _currentTurn++;
        StartPhase();
    }
    
    private void EndGame()
    {
        Debug.Log("Ya ha terminado el juego");
    }
}