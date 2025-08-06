using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public Phase CurrentPhase = Phase.Collecting;

    [Header("Part for work in logic")] 
    [SerializeField] private float _percentageLessScale = 0.7f;
    [SerializeField] List<ObjectsToSpawn> _partsToSpawn;
    [SerializeField] List<ObjectsToSpawn> _partsCollectedPlayers;
    [SerializeField] private GameObject[] _baseGuides;
    [SerializeField] private GameObject[] _plantedPlants;
    [SerializeField] List<ObjectsToSpawn> _zonesForSown;
    [SerializeField] private Color[] _colorsTurn;
    [SerializeField] private TypeObject[] _orderPlants;
    [SerializeField] private TextMeshProUGUI _textTurn;
    [SerializeField] private TextMeshProUGUI _textPhase;

    [FormerlySerializedAs("_nameObject")]
    [Header("Part of panel to show name")] 
    [SerializeField] private TextMeshProUGUI _textNameObject;
    [SerializeField] private TextMeshProUGUI _textNameTwoObject;
    [SerializeField] private TextMeshProUGUI _textDescription;
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

    public void ShowNameObject(string name, string nameTwo, string textDescription, Sprite background)
    {
        _textNameObject.text = name;
        _textNameTwoObject.text = nameTwo;
        _textDescription.text = textDescription;
        _panelToShowName.GetComponent<Image>().sprite = background;
        _audioSource.PlayOneShot(_effectAudio);
        StartCoroutine(ShowNameTemporaly());
    }

    private IEnumerator ShowNameTemporaly()
    {
        _panelToShowName.SetActive(true);
        yield return new WaitForSeconds(5f);
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
        StartCoroutine(StartAnimationPlants());
    }

    private IEnumerator StartAnimationPlants()
    {
        foreach (var plant in _orderPlants)
        {
            for (var i = 0; i < _plantedPlants.Length; i++)
            {
                var currentPlant = _plantedPlants[i].GetComponent<BaseGuideController>().GetTypePlant();
                if (currentPlant == plant)
                {
                    _plantedPlants[i].SetActive(true);
                }
                else
                {
                    continue;
                }

                yield return new WaitForSeconds(1);
            }
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

        var newRotation = objectToDrag.transform.localRotation;
        newRotation.x = 0;
        newRotation.y = 0;
        newRotation.z = 0;
        objectToDrag.transform.localRotation = newRotation;

        objectToDrag.transform.localScale *= _percentageLessScale;
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
        foreach (var zone in _zonesForSown[_currentTurn].PrefabsParts)
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