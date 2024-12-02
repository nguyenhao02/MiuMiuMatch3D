using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NomalObject
{
    public GameObject prefabObject;
    public int countSpawn;
}

[System.Serializable]
public class TaskObject
{
    public GameObject prefabObject;
    public int countSpawn;
    public int count;

}

[System.Serializable]
public class SpecialObject
{
    public GameObject prefabObject;
    public int minCountSpawn, maxCountSpawn;
    public float rateSpawn;

}

[System.Serializable]
public class TwinObject
{
    public GameObject prefabObject;
    public GameObject obj1;
    public GameObject obj2;

}
