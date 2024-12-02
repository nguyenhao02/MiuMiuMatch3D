using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;

public class UIBoomObject : MonoBehaviour
{
    public GameObject parentObjects;
    public Transform shootPosion;
    public int minRot, maxRot;

    private List<LevelData> levelDatas;
    private int level;
    private bool canMove = true;

    void Start()
    {
        levelDatas = DataManager.Instance.levelDatas;
        level = DataManager.Instance.playerData.level;

        StartCoroutine(MoveGameObject());
        StartCoroutine(SpawnNomalObject());
    }

  private IEnumerator MoveGameObject()
    {
        while (true)
        {
            Quaternion newRotate = Quaternion.Euler(0, Random.Range(minRot, maxRot), 0);

            while (transform.rotation != newRotate)
            {
                if (!canMove)
                {
                    yield return null;
                    continue;
                }

                transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotate, Time.deltaTime * 30f);
                yield return null;
            } 
        }
    }

    private IEnumerator SpawnNomalObject()
    {
        while(true)
        {
            canMove = true;
            yield return new WaitForSeconds(6f); 
            canMove = false;
            GetComponent<Animator>().SetTrigger("isShoot");
            SoundManager.Instance.PlaySFX(SoundManager.Instance.explosion);

            yield return new WaitForSeconds(0.2f);
            int index = Random.Range(0, levelDatas[level].nomalObjects.Count);
            GameObject objSpwan = Instantiate(levelDatas[level].nomalObjects[index].prefabObject, shootPosion.position, Quaternion.identity, parentObjects.transform);
            objSpwan.GetComponent<Rigidbody>().AddForce(transform.rotation * Vector3.forward * 10000);
            yield return new WaitForSeconds(0.3f);
            
        }
    }
}
