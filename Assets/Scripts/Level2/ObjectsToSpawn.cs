using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class ObjectsToSpawn
{
    [FormerlySerializedAs("ObjectsToUSe")] [FormerlySerializedAs("PrefabsParts")] public List<GameObject> ObjectsToUse;
}