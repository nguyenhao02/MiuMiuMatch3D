using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level Data")]
public class LevelData : ScriptableObject
{
    public int time;
    public List<TaskObject> taskObjects;
    public List<NomalObject> nomalObjects;
    public List<SpecialObject> specialObjects;
    public List<TwinObject> twinObjects;
    
}
