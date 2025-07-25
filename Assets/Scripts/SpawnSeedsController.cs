using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnSeedsController : MonoBehaviour
{
    public static SpawnSeedsController Instance;
    
    [SerializeField] private Transform _playerPosition;
    
    [SerializeField] private float _radiusSpawn;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void GeneratePartsAround(List<GameObject> partsOfSpawn)
    {
        var amountParts = GameController.Instance.AmountSpawn;
        if (partsOfSpawn.Count > 0)
        {
            for (var i = 0; i < amountParts; i++)
            {
                var prefabSeed = partsOfSpawn[0];
                var offset2d = Random.insideUnitCircle * _radiusSpawn;
                var offset = new Vector3(offset2d.x, 0, offset2d.y);

                var seedTemp = Instantiate(prefabSeed, transform.position + offset, prefabSeed.transform.rotation);
            }
        }
        else
        {
            Debug.LogError("No hay objetos asignados");
        }
    }
}