using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnSeedsController : MonoBehaviour
{
    public static SpawnSeedsController Instance;
    
    [SerializeField] private Transform _playerPosition;
    
    [SerializeField] private float _radiusSpawn;
    [SerializeField] private float _minDistanceOfPlayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void GeneratePartsAround(List<GameObject> partsOfSpawn)
    {
        var amountParts = partsOfSpawn.Count;
        if (partsOfSpawn.Count > 0)
        {
            for (var i = 0; i < amountParts; i++)
            {
                var prefabSeed = partsOfSpawn[i];
                Vector3 offSet;

                do
                {
                    var offset2d = Random.insideUnitCircle * _radiusSpawn;
                    offSet = new Vector3(offset2d.x, 0, offset2d.y);
                    
                } while (offSet.magnitude < _minDistanceOfPlayer);

                var seedTemp = Instantiate(prefabSeed, transform.position + offSet, prefabSeed.transform.rotation);
            }
        }
        else
        {
            Debug.LogError("No hay objetos asignados");
        }
    }
}