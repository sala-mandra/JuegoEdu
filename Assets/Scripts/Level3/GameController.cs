using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public Phase CurrentPhase = Phase.Collecting;

    [Header("Part for work in logic")] 
    [SerializeField] private SOLevelSpiral _soLevelSpiral;
    [SerializeField] private float _percentageLessScale = 0.7f;
    [SerializeField] List<ObjectsToSpawn> _partsToSpawn;
    [SerializeField] List<ObjectsToSpawn> _partsCollectedPlayers;
    [SerializeField] private GameObject[] _baseGuides;
    [SerializeField] private GameObject[] _plantedPlants;
    [SerializeField] List<ObjectsToSpawn> _zonesForSown;
    [SerializeField] private Color[] _colorsTurn;
    [SerializeField] private TypeObject[] _orderPlantsGrowth;
    [SerializeField] private TypeObject[] _orderPlantsSownOne;
    [SerializeField] private TypeObject[] _orderPlantsSownTwo;
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
    private int _currentBaseGuide;
    private Coroutine _coroutineShowName;
    

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
        if (_coroutineShowName != null)
        {
            StopCoroutine(_coroutineShowName);
        }
        _coroutineShowName = StartCoroutine(ShowNameTemporaly());
    }

    private IEnumerator ShowNameTemporaly()
    {
        _panelToShowName.SetActive(true);
        yield return new WaitForSeconds(10f);
        _panelToShowName.SetActive(false);
        _coroutineShowName = null;
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
            OrderListsToSow(list);
            if (list.Count >= amountSpawn)
            {
                NextCollectingTurn();
            }
        }
    }

    private void OrderListsToSow(List<GameObject> listTemp)
    {
        if (_currentTurn == 0)
        {
            _partsCollectedPlayers[_currentTurn].PrefabsParts = listTemp.OrderBy(p => Array.IndexOf(_orderPlantsSownOne,
                p.GetComponent<SeedController>().TypeObjectDrag)).ToList();
        }
        else
        {
            _partsCollectedPlayers[_currentTurn].PrefabsParts =listTemp.OrderBy(p => Array.IndexOf(_orderPlantsSownTwo,
                p.GetComponent<SeedController>().TypeObjectDrag)).ToList();
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
        StartCoroutine(EnableGuidesArranging());
    }

    public void EnableAnimationPlantedPlants()
    {
        StartCoroutine(StartAnimationPlants());
    }

    private IEnumerator StartAnimationPlants()
    {
        foreach (var plant in _orderPlantsGrowth)
        {
            for (var i = 0; i < _plantedPlants.Length; i++)
            {
                var currentPlant = _plantedPlants[i].GetComponent<BaseGuideController>().GetTypePlant();
                if (currentPlant == plant)
                {
                    _plantedPlants[i].SetActive(true);
                    _plantedPlants[i].GetComponent<AnimationGrowthPlants>().AnimationGrowth();
                }
                else
                {
                    continue;
                }

                yield return new WaitForSeconds(1);
            }
        }
        yield return new WaitForSeconds(2);
        EndGame();
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
                Debug.Log("Termino interacciones");
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
            //_baseGuides[_currentTurn].SetActive(true);
            EnableNextDragObject();
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
        yield return new WaitForSeconds(1f);
        if (_currentBaseGuide >= 0)
        {
            _zonesForSown[_currentTurn].PrefabsParts[_currentBaseGuide].SetActive(true);
            if (_currentBaseGuide < _zonesForSown[_currentTurn].PrefabsParts.Count - 1)
            {
                _currentBaseGuide++;
            }
            else
            {
                _currentBaseGuide = 0;
            }
        }
        // else
        // {
        //     _currentBaseGuide++;
        // }
    }
    
    private void NextArrangingTurn()
    {
        _currentTurn++;
        StartPhase();
    }
    
    private void EndGame()
    {
        Debug.Log("Ya ha terminado el juego");
        if (_soLevelSpiral.Level < _soLevelSpiral.MaxLevel)
        {
            _soLevelSpiral.Level++;
        }

        SceneManager.LoadScene("Levels");
    }
}