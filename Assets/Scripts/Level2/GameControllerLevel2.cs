using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerLevel2 : MonoBehaviour
{
    public static GameControllerLevel2 Instance;
    public Phase CurrentPhase = Phase.Collecting;

    [Header("Part for work in logic")] 
    [SerializeField] private SOLevelSpiral _soLevelSpiral;

    [SerializeField] private float _percentageLessScale = 0.7f;
    [SerializeField] List<ObjectsToSpawn> _partsToSpawn;
    [SerializeField] List<ObjectsToSpawn> _partsCollectedPlayers;
    [SerializeField] private GameObject[] _baseGuides;
    [SerializeField] private GameObject[] _plantedPlants;
    [SerializeField] List<ObjectsToSpawn> _zonesForSown;
    [SerializeField] private TypeObject[] _orderPlantsGrowth;
    [SerializeField] private TypeObject[] _orderPlantsSownOne;
    [SerializeField] private TypeObject[] _orderPlantsSownTwo;
    [SerializeField] private GameObject[] _imageTextPhase;
    [SerializeField] private List<ObjectsToSpawn> _imageTextTurn;

    [Header("Part of panel to show name")] 
    [SerializeField] private TextMeshProUGUI _textNameObject;
    [SerializeField] private TextMeshProUGUI _textNameTwoObject;
    [SerializeField] private TextMeshProUGUI _textDescription;
    [SerializeField] private GameObject _panelToShowName;
    [SerializeField] private AudioClip _effectAudio;
    [SerializeField] private AudioClip _effectFinalAudio;
    [SerializeField] private AudioSource _audioSource;

    [Header("Panels to intro")] 
    [SerializeField] private GameObject _panelFinal;

    private int _currentTurn;
    private int _totalPlayers => _partsToSpawn.Count;
    private int _currentCollected;
    private int _currentBaseGuide;
    private Coroutine _coroutineShowName;
    private bool _showTextTurn;
    private int _idLevel = 2; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void StartGame()
    {
        StartCoroutine(StartPhase());
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
        _showTextTurn = true;
        _panelToShowName.SetActive(true);
        yield return new WaitForSeconds(10f);
        _panelToShowName.SetActive(false);
        _showTextTurn = false;
        _coroutineShowName = null;
    }

    public void AddCollectedPart(GameObject collectObj)
    {
        if (CurrentPhase != Phase.Collecting)
        {
            Debug.LogWarning("Esta no es la fase actual");
            return;
        }

        var amountSpawn = _partsToSpawn[_currentTurn].ObjectsToUse.Count;
        var list = _partsCollectedPlayers[_currentTurn].ObjectsToUse;
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
            _partsCollectedPlayers[_currentTurn].ObjectsToUse = listTemp.OrderBy(p => Array.IndexOf(_orderPlantsSownOne,
                p.GetComponent<SeedController>().TypeObjectDrag)).ToList();
        }
        else
        {
            _partsCollectedPlayers[_currentTurn].ObjectsToUse =listTemp.OrderBy(p => Array.IndexOf(_orderPlantsSownTwo,
                p.GetComponent<SeedController>().TypeObjectDrag)).ToList();
        }
    }

    private void NextCollectingTurn()
    {
        _currentTurn++;
        StartCoroutine(StartPhase());
    }

    public void StartCoroutineEnableNextDrag()
    {
        StartCoroutine(EnableNextDragObject());
    }
    
    private IEnumerator EnableNextDragObject()
    {
        if (CurrentPhase != Phase.Arranging)
            yield break;

        if (_partsCollectedPlayers[_currentTurn].ObjectsToUse.Count == 0)
        {
            NextArrangingTurn();
            yield break;
        }
        
        var requiredPart = _partsCollectedPlayers[_currentTurn].ObjectsToUse[_currentCollected];
        SetNewTransform(requiredPart);        
        requiredPart.SetActive(true);
        
        _partsCollectedPlayers[_currentTurn].ObjectsToUse.RemoveAt(0);
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
        yield return new WaitForSeconds(2.5f);
        EndGame();
    }

    private IEnumerator StartPhase()
    {
        yield return new WaitUntil(() => !_showTextTurn);
        switch (CurrentPhase)
        {
            case Phase.Collecting:
                yield return StartCoroutine(StartCollectingTurn());
                break;
            case Phase.Arranging:
                yield return StartCoroutine(StartArrangingTurn());
                break;
            case Phase.Finished:
                Debug.Log("Termino interacciones");
                break;
        }
    }

    private IEnumerator StartCollectingTurn()
    {
        if (_currentTurn < _totalPlayers)
        {
            
            yield return StartCoroutine(StartPhaseAnimation(_imageTextPhase[GetPhaseIndex()]));
            SpawnSeedsController.Instance.GeneratePartsAround(_partsToSpawn[_currentTurn].ObjectsToUse);
        }
        else
        {
            _currentTurn = 0;
            CurrentPhase = Phase.Arranging;
            yield return StartCoroutine(StartPhase());
        }
    }

    private IEnumerator StartArrangingTurn()
    {
        if (_currentTurn < _totalPlayers)
        {
            yield return StartCoroutine(StartPhaseAnimation(_imageTextPhase[GetPhaseIndex()]));
            yield return StartCoroutine(EnableNextDragObject());
        }
        else
        {
            _currentTurn = 0;
            CurrentPhase = Phase.Finished;
            yield return StartCoroutine(StartPhase());
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

    private IEnumerator StartPhaseAnimation(GameObject imagePhase)
    {
        if (_currentTurn == 0)
        {
            imagePhase.SetActive(true);
            yield return new WaitForSeconds(3);
            imagePhase.SetActive(false);
        }
        var turnTemp = _imageTextTurn[GetPhaseIndex()].ObjectsToUse[_currentTurn];
        yield return StartCoroutine(StartAnimationTextTurn(turnTemp));
    }

    private IEnumerator StartAnimationTextTurn(GameObject currentTurnImage)
    {
        currentTurnImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(10f);
        currentTurnImage.gameObject.SetActive(false);
    }
    
    private IEnumerator EnableGuidesArranging()
    {
        yield return new WaitForSeconds(1f);
        if (_currentBaseGuide >= 0)
        {
            _zonesForSown[_currentTurn].ObjectsToUse[_currentBaseGuide].SetActive(true);
            if (_currentBaseGuide < _zonesForSown[_currentTurn].ObjectsToUse.Count - 1)
            {
                _currentBaseGuide++;
            }
            else
            {
                _currentBaseGuide = 0;
            }
        }
    }
    
    private void NextArrangingTurn()
    {
        _currentTurn++;
        StartCoroutine(StartPhase());
    }
    
    private void EndGame()
    {
        Debug.Log("Ya ha terminado el juego");
        LevelsController.Instance.CompleteLevel(_idLevel);
        _panelFinal.SetActive(true);
        _audioSource.PlayOneShot(_effectFinalAudio);
    }
    
    private int GetPhaseIndex()
    {
        return CurrentPhase == Phase.Collecting ? 0 :
            CurrentPhase == Phase.Arranging ? 1 : -1;
    }

    public void ReturnToHome()
    {
        SceneManager.LoadScene("MenuAndLevel1");
    }
}