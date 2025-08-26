using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOLevelSpiral", menuName = "Scriptable Objects/SOLevelSpiral")]
public class SOLevelSpiral : ScriptableObject
{
    public int Level;
    public int MaxLevel;
    public List<int> LevelsComplete = new List<int>();
}
