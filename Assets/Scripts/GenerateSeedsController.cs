using System;
using UnityEngine;

public class GenerateSeedsController : MonoBehaviour
{
    [SerializeField] private Transform _playerPosition;
    [SerializeField] private GameObject[] _partsOfStructure;

    private void Start()
    {
        GeneratePartsAround();
    }

    private void GeneratePartsAround()
    {
        
    }
}
