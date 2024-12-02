using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMap : MonoBehaviour
{
    public GameObject parentObject;
    private List<LevelData> levelDatas;
    private int level;

    private TaskController taskController;

    void Awake()
    {
        taskController = GetComponent<TaskController>();
    }
    public void LoadLevel()
    {
        levelDatas = DataManager.Instance.levelDatas;
        level = DataManager.Instance.playerData.level;

        SpawnTaskObject(levelDatas[level].taskObjects);
        SpawnNomalObject(levelDatas[level].nomalObjects);
        SpawnSpecialObject(levelDatas[level].specialObjects);
        SpawnTwinlObject(levelDatas[level].twinObjects);

    }

    private void SpawnTaskObject(List<TaskObject> taskObjects)
    {
        foreach (TaskObject taskObject in taskObjects)
        {
            for (int i = 0; i < taskObject.countSpawn; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-3, 3), Random.Range(7, 15), Random.Range(-5, 3));
                Instantiate(taskObject.prefabObject, spawnPosition, Quaternion.identity, parentObject.transform);
            }
        }
    }

    private void SpawnNomalObject(List<NomalObject> nomalObjects)
    {
        foreach (NomalObject nomalObject in nomalObjects)
        {
            for (int i = 0; i < nomalObject.countSpawn; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-3, 3), Random.Range(7, 15), Random.Range(-5, 3));
                Instantiate(nomalObject.prefabObject, spawnPosition, Quaternion.identity, parentObject.transform);
            }
        }
    }

    private void SpawnSpecialObject(List<SpecialObject> specialObjects)
    {
        for(int i = 0; i < specialObjects.Count; i++)
        {
            float chance = Random.Range(0f, 1f);
            float rateSpawn = specialObjects[i].rateSpawn;
            int countSpawn = Random.Range(specialObjects[i].minCountSpawn, specialObjects[i].maxCountSpawn + 1);

            if(i == 0 || i == 2) 
            {
                int plays = DataManager.Instance.playerData.plays;
                rateSpawn *= plays;
                countSpawn = Random.Range(specialObjects[i].minCountSpawn + plays - 1, specialObjects[i].maxCountSpawn + plays);
            }
            
            if(chance <= rateSpawn)
            {
                for (int j = 0; j < countSpawn; j++)
                {
                    Vector3 spawnPosition = new Vector3(Random.Range(-3, 3), Random.Range(7, 15), Random.Range(-5, 3));
                    Instantiate(specialObjects[i].prefabObject, spawnPosition, Quaternion.identity, parentObject.transform) ;
                }
            }
        }
    }

    private void SpawnTwinlObject(List<TwinObject> twinObjects)
    {
        foreach(TwinObject twinObject in twinObjects)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-3, 3), Random.Range(7, 15), Random.Range(-5, 3));
            Instantiate(twinObject.prefabObject, spawnPosition, Quaternion.identity, parentObject.transform);

            //Spawn thêm từng obj con cho đủ số lượng  / 3
            Instantiate(twinObject.obj1, spawnPosition, Quaternion.identity, parentObject.transform);
            Instantiate(twinObject.obj1, spawnPosition, Quaternion.identity, parentObject.transform);
            Instantiate(twinObject.obj2, spawnPosition, Quaternion.identity, parentObject.transform);
            Instantiate(twinObject.obj2, spawnPosition, Quaternion.identity, parentObject.transform);

            //Check taskobject để + taskCount;
            foreach (TaskObject taskObject in levelDatas[level].taskObjects)
            {
                int i = 0;
                if(taskObject.prefabObject.name == twinObject.obj1.name)
                {
                    taskObject.count += 3;
                    taskController.UpdateCard(i);
                    break;
                }

                if(taskObject.prefabObject.name == twinObject.obj2.name)
                {
                    taskObject.count += 3;
                    taskController.UpdateCard(i);
                    break;
                }
                i++;
            }
        }
    }


    public GameObject CopyObject(GameObject obj)
    {
        foreach (TaskObject taskObject in levelDatas[level].taskObjects)
        {
            if(taskObject.prefabObject.name == obj.name.Substring(0, obj.name.Length - 7))
            return taskObject.prefabObject;
        }
        foreach (NomalObject nomalObject in levelDatas[level].nomalObjects)
        {
            if(nomalObject.prefabObject.name == obj.name.Substring(0, obj.name.Length - 7))
            return nomalObject.prefabObject;
        }
        return null;
    }

    public int CheckNomalObject(GameObject obj)
    {
        int i = 0;
        foreach (NomalObject nomalObject in levelDatas[level].nomalObjects)
        {
            if(nomalObject.prefabObject.name == obj.name.Substring(0, obj.name.Length - 7))
            return i;
            i++;
        }
        return -1;
    }


    public int CheckTaskObject(GameObject obj)
    {
        int i = 0;
        foreach (TaskObject taskObject in levelDatas[level].taskObjects)
        {
            if(taskObject.prefabObject.name == obj.name.Substring(0, obj.name.Length - 7)) 
            return i;
            i++;
        }
        return -1;
    }

    public int CheckSpecialObject(GameObject obj)
    {
        int i = 0;
        foreach (SpecialObject specialObject in levelDatas[level].specialObjects)
        {
            if(specialObject.prefabObject.name == obj.name.Substring(0, obj.name.Length - 7))
            return i;
            i++;
        }
        return -1;
    }

    public TwinObject CheckTwinObject(GameObject obj)
    {
        foreach (TwinObject twinObject in levelDatas[level].twinObjects)
        {
            if(twinObject.prefabObject.name == obj.name.Substring(0, obj.name.Length - 7))
            return twinObject;
        }
        return null;
    }
}
