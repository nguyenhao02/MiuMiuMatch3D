using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TaskController : MonoBehaviour
{
    public List<GameObject> taskCards;
    private List<TaskObject> taskObjescts;
    private UIWinGame uiWinGame;
    private TimeClock timeClock;
    private int level;

    void Awake()
    {
        uiWinGame = GetComponent<UIWinGame>();  
        timeClock = GetComponent<TimeClock>();
    }

    public void SetTaskCards()
    {
        level = DataManager.Instance.playerData.level;
        taskObjescts = DataManager.Instance.levelDatas[level].taskObjects;

        for(int i = 0; i < taskObjescts.Count; i++)
        {
            taskCards[i].SetActive(true);
            GameObject taskObjesct = Instantiate(taskObjescts[i].prefabObject, taskCards[i].transform.position, Quaternion.identity, taskCards[i].transform);
            taskObjesct.transform.localPosition = new Vector3(0, 0, -35);
            taskObjesct.GetComponent<Rigidbody>().isKinematic = true;
            taskObjesct.GetComponent<MeshCollider>().enabled = false;
            taskObjesct.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            taskObjesct.transform.localScale = new Vector3(75f, 75f, 75f);
            taskObjesct.transform.localRotation = Quaternion.identity;

            taskObjescts[i].count = taskObjescts[i].countSpawn;
            taskCards[i].GetComponentInChildren<TextMeshProUGUI>().text = taskObjescts[i].count.ToString();
        }

    }

    public void CheckPickObjectTask(GameObject obj)
    {
        for(int i = 0; i < taskObjescts.Count; i++)
        {
            if(CompareNameObject(taskObjescts[i].prefabObject, obj))
            {
                taskObjescts[i].count -= 1;
                // Hiệu ứng taskcard
                SoundManager.Instance.PlaySFX(SoundManager.Instance.objectTask);
                taskCards[i].GetComponentInChildren<ParticleSystem>().Play();
                taskCards[i].GetComponent<Animator>().SetTrigger("isZoom");
                UpdateCard(i);


                if(CheckWin())
                {
                    StartCoroutine(CheckWinGame());
                }

                if(taskObjescts[i].count == 0)
                {
                    StartCoroutine(RemoveTaskCard(i));
                }

                break;
            }

        }
    }
    private IEnumerator CheckWinGame()
    {
        //Chờ 1s kiểm tra lại WinGame tránh trường hợp ấn spell 0
        yield return new WaitForSeconds(1f);
        if(!CheckWin()) yield break;
        timeClock.PauseTime();

        yield return new WaitForSeconds(1f);
        uiWinGame.OpenUIWinGame();
    }

    private IEnumerator RemoveTaskCard(int index)
    {
        //Chờ 1s kiểm tra lại taskCard
        yield return new WaitForSeconds(1f);
        if(taskObjescts[index].count != 0) yield break;

        taskCards[index].GetComponent<Animator>().SetTrigger("isDisappear");
        yield return new WaitForSeconds(1.2f);
        taskCards[index].transform.parent.gameObject.SetActive(false);
    }

    public void CheckRemoveObjectTask(GameObject obj)
    {
        for(int i = 0; i < taskObjescts.Count; i++)
        {
            if(CompareNameObject(taskObjescts[i].prefabObject, obj))
            {
                taskObjescts[i].count += 1;
                UpdateCard(i);
                break;
            }

        }
    }


    private bool CompareNameObject (GameObject obj, GameObject objClone)
    {
        if(obj.name == objClone.name.Substring(0, objClone.name.Length - 7)) return true;
        return false;
    }

    public void UpdateCard(int index)
    {
        taskCards[index].GetComponentInChildren<TextMeshProUGUI>().text = taskObjescts[index].count.ToString();
    }

    private bool CheckWin()
    {
        for(int i = 0; i < taskObjescts.Count; i++)
        {
            if(taskObjescts[i].count > 0) return false;
        }
        return true;
    }
}
