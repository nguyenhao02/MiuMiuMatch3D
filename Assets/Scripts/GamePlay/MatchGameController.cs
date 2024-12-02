using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MatchGameController : MonoBehaviour
{
    public List<GameObject> selectBoxs;
    public List<GameObject> selectObjects = new List<GameObject>();
    public List<GameObject> inBoxObjects = new List<GameObject>();

    public GameObject parentObject;

    public GameObject spell1Collider;
    public GameObject spell2Point;
    public GameObject spell2LineElectric;
    public GameObject spell3Point;
    public GameObject particleSpell3;
    public GameObject clockSpell3;

    public GameObject fxBulletBlack, fxExplosionBlack, fxBulletBlue, fxExplosionBlue, fxExplosionRed, fxBulletRocket;

    private GameObject selectObject;
    private GameObject preSelectObject = null;
    private Vector3 selectObjectPos;
    private Vector3 preSelectObjectPos;

    private float moveSpeed = 10f;
    private bool canPickUp = false;

    private TaskController taskController;
    private SetMap setMap;
    private UIController uIController;
    private TimeClock timeClock;

    void Awake()
    {
        taskController = GetComponent<TaskController>();
        setMap = GetComponent<SetMap>();
        uIController = GetComponent<UIController>();
        timeClock = GetComponent<TimeClock>();
    }

    void Update()
    {
        PickUp();    
    }

    void FixedUpdate()
    {
        SortBoxs();
    }

    public void PlayPickUp()
    {
        canPickUp = true;
    }
    public void PausePickUp()
    {
        canPickUp = false;
    }

    private void PickUp()
    {
        if(!canPickUp) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);  
            Ray ray =  Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;
            Outline outline;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.CompareTag("Object"))
                {
                    selectObject = hit.collider.gameObject;
                    selectObjectPos = selectObject.transform.position;

                    if (preSelectObject != selectObject)
                    {
                        SoundManager.Instance.Vibration(); 
                        SoundManager.Instance.PlaySFX(SoundManager.Instance.objectHighlight);
                        
                        if (preSelectObject != null)
                        {
                            preSelectObject.GetComponent<Rigidbody>().isKinematic = false;
                            outline = preSelectObject.GetComponent<Outline>();
                            if(outline != null) outline.enabled = false;
                            else preSelectObject.GetComponent<OutlineAll>().enabled = false;

                            preSelectObject.transform.position = preSelectObjectPos;
                        }

                        preSelectObjectPos = selectObjectPos;
                        selectObject.GetComponent<Rigidbody>().isKinematic = true;
                        outline = selectObject.GetComponent<Outline>();
                        if(outline != null) outline.enabled = true;
                        else selectObject.GetComponent<OutlineAll>().enabled = true;

                        selectObjectPos.y += 0.5f;
                        selectObject.transform.position = selectObjectPos;

                        

                        preSelectObject = selectObject;
                    }
                }
                else
                {
                    if (preSelectObject != null)
                    {
                        preSelectObject.GetComponent<Rigidbody>().isKinematic = false;
                        outline = preSelectObject.GetComponent<Outline>();
                        if(outline != null) outline.enabled = false;
                        else preSelectObject.GetComponent<OutlineAll>().enabled = false;

                        preSelectObject.transform.position = preSelectObjectPos;
                        preSelectObject = null;
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (preSelectObject == null) return;

                outline = preSelectObject.GetComponent<Outline>();
                if(outline != null) outline.enabled = false;
                else preSelectObject.GetComponent<OutlineAll>().enabled = false;

                if(selectObjects.Count == 7) 
                {
                    preSelectObject.GetComponent<Rigidbody>().isKinematic = false;
                    return;
                }

                //Check TwinObject
                TwinObject twinObject = setMap.CheckTwinObject(preSelectObject);
                if(twinObject != null)
                {
                    if(selectObjects.Count > 5)
                    {
                        preSelectObject.GetComponent<Rigidbody>().isKinematic = false;
                        return;
                    }
                    preSelectObject.GetComponent<Rigidbody>().isKinematic = true;
                    preSelectObject.GetComponent<CapsuleCollider>().enabled = false;
                    StartCoroutine(PickTwinObject(twinObject, preSelectObject));
                    preSelectObject = null;
                    return;
                }

                preSelectObject.GetComponent<Rigidbody>().isKinematic = true;
                preSelectObject.GetComponent<MeshCollider>().enabled = false;
                
                //Check Special Object
                int intCheck = setMap.CheckSpecialObject(preSelectObject);
                if(intCheck >= 0)
                {
                    PickSpecialObject(intCheck, preSelectObject);
                    //Destroy(preSelectObject);
                    Debug.Log(intCheck);
                    preSelectObject = null;
                    return;
                }

                

                // Chọn vị trí chèn
                int index = IndexInsert(preSelectObject, selectObjects) ;
                selectObjects.Insert(index, preSelectObject);

                // Kiểm tra vật phẩm nhiệm vụ
                taskController.CheckPickObjectTask(preSelectObject);

                // Di chuyển về Box
                StartCoroutine(MoveToBox(preSelectObject));

                preSelectObject = null;
            
            }
        }
    }

    private void SortBoxs()
    {
        for(int i = 0; i < selectObjects.Count; i++)
        {
            if(!inBoxObjects.Contains(selectObjects[i])) continue;

            GameObject obj = selectObjects[i];
            GameObject box = selectBoxs[i];
            obj.transform.position = Vector3.Lerp(obj.transform.position, box.transform.position, Time.deltaTime * moveSpeed );
            obj.transform.SetParent(box.transform);
            
        }
    }  

    private IEnumerator MoveToBox(GameObject obj)
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.objectSelect);
        obj.transform.SetParent(selectBoxs[0].transform);
        Vector3 newPos = new Vector3 (obj.transform.position.x + (obj.transform.position.x > 0 ? -1f : 1f), 15f, obj.transform.position.z + (obj.transform.position.z > 4.5 ? -0.5f : 1f));
        Quaternion newRotation = Quaternion.Euler(90, 0, 0); 
        Vector3 objScale = obj.transform.localScale;

        while (Vector3.Distance(obj.transform.position, newPos) > 0.3f)
        {
            // Di chuyển lên
            obj.transform.position = Vector3.Lerp(obj.transform.position, newPos, Time.deltaTime * moveSpeed);
            // Xoay
            obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, newRotation, Time.deltaTime * moveSpeed); 
            // Zoom in
            obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, objScale * 1.4f, Time.deltaTime * moveSpeed);
            yield return null;
        }

        obj.transform.position = newPos;
        obj.transform.rotation = newRotation;
        obj.transform.localScale = objScale * 1.4f;

        objScale = new Vector3(66f,66f,66f);

        // di chuyển về box ứng với selectObject
        GameObject box = selectBoxs[selectObjects.IndexOf(obj)];
        Vector3 targetPosition = box.transform.position;  
        while (Vector3.Distance(obj.transform.position, targetPosition) > 0.3f)
        {
            box = selectBoxs[selectObjects.IndexOf(obj)];
            targetPosition = box.transform.position;
            // Di chuyển về box
            obj.transform.position = Vector3.Lerp(obj.transform.position, targetPosition, Time.deltaTime * moveSpeed);
            // Zoom out
            obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, objScale, Time.deltaTime * moveSpeed);
            yield return null;
        }
        obj.transform.SetParent(box.transform);
        obj.transform.position = targetPosition;
        obj.transform.localScale = objScale;

        //BingGo
        int index  = IndexInsert(obj, inBoxObjects);
        inBoxObjects.Insert(index, obj); 
        BingGo(index); 

        box.GetComponent<Animator>().SetTrigger("UpDown");
       
    }

    private int IndexInsert(GameObject obj, List<GameObject> objs)
    {
        int i = objs.Count;
        while(i > 0)
        {
            if(obj.name == objs[i-1].name) 
            {
                return i;
            }

            i--;
        }
        return objs.Count;
    }

    private void BingGo(int index)
    {
        if(index < 2) 
        {
            if(inBoxObjects.Count == 7) 
            {
                //FullBox & EndGame
                uIController.OpenUILoseGameFullBox();
            }
            
            return;
        }
        

        if ((inBoxObjects[index].name == inBoxObjects[index - 1].name) && (inBoxObjects[index].name == inBoxObjects[index - 2].name))
        {
            Debug.Log("BingGo");

            selectBoxs[index -1].GetComponentInChildren<ParticleSystem>().Play();
            StartCoroutine(BingGoEffect(inBoxObjects[index], inBoxObjects[index - 1], inBoxObjects[index - 2]));

            // selectObjects.Remove(inBoxObjects[index]);
            // selectObjects.Remove(inBoxObjects[index - 1]);
            // selectObjects.Remove(inBoxObjects[index - 2]);

            inBoxObjects.RemoveAt(index);
            inBoxObjects.RemoveAt(index - 1);
            inBoxObjects.RemoveAt(index - 2);
        }
        else 
        {
            if(inBoxObjects.Count == 7) 
            {
                //FullBox & EndGame
                uIController.OpenUILoseGameFullBox();
            }
        }
    }

    private IEnumerator BingGoEffect(GameObject obj_1, GameObject obj_2, GameObject obj_3)
    {
        yield return new WaitForSeconds(0.2f);

        SoundManager.Instance.PlaySFX(SoundManager.Instance.binggo);

        Vector3 goPos = obj_2.transform.position;
        goPos.z += 1.7f;
        Vector3 obj_2Scale = obj_2.transform.localScale;
       
        while(Vector3.Distance(obj_2.transform.position, goPos) > 0.1f)
        {
            obj_1.transform.position = Vector3.Lerp(obj_1.transform.position, goPos, Time.deltaTime * moveSpeed);
            obj_2.transform.position = Vector3.Lerp(obj_2.transform.position, goPos, Time.deltaTime * moveSpeed);
            obj_2.transform.localScale = Vector3.Lerp(obj_2.transform.localScale, obj_2Scale * 1.4f, Time.deltaTime * moveSpeed);
            obj_3.transform.position = Vector3.Lerp(obj_3.transform.position, goPos, Time.deltaTime * moveSpeed);
            yield return null;
        }
  
        Destroy(obj_1);
        Destroy(obj_2);
        Destroy(obj_3);

        selectObjects.Remove(obj_1);
        selectObjects.Remove(obj_2);
        selectObjects.Remove(obj_3);
        yield return null;
    }

    public bool Spell0()
    {
        if(inBoxObjects.Count == 0) return false;

        for(int i = 1; i <= 3; i++)
        {
            if(inBoxObjects.Count < 1) break;

            SoundManager.Instance.PlaySFX(SoundManager.Instance.spell0);

            GameObject obj = inBoxObjects[inBoxObjects.Count - 1];

            obj.transform.SetParent(parentObject.transform);
            //Tạo 1 bản sao tương tự
            GameObject newObj = Instantiate(setMap.CopyObject(obj), obj.transform.position, obj.transform.localRotation, parentObject.transform);
            Vector3 newObjScale = newObj.transform.localScale;
            newObj.transform.localScale = obj.transform.localScale;
            // Đẩy ra khỏi Box && hiệu ứng
            selectBoxs[inBoxObjects.Count - 1].GetComponent<Animator>().SetTrigger("isSpring");
            StartCoroutine(PushAndZoom(newObj, newObjScale));
            // Kiểm tra vật phẩm nhiệm vụ
            taskController.CheckRemoveObjectTask(newObj);
             
            selectObjects.Remove(obj);
            inBoxObjects.Remove(obj);
            Destroy(obj);
        }
        return true;
    }

    private IEnumerator PushAndZoom(GameObject obj, Vector3 desScale)
    {
        obj.GetComponent<Rigidbody>().AddForce(Vector3.forward * Random.Range(5000f, 10000f));

        float timeZoom = 0.5f; 
        float time = 0f;
        Vector3 initialScale = obj.transform.localScale;

        while (time < timeZoom)
        {
            obj.transform.localScale = Vector3.Lerp(initialScale, desScale, time / timeZoom);
            time += Time.deltaTime; 
            yield return null; 
        }
        
        obj.transform.localScale = desScale;
    }

    public void Spell1()
    {
        canPickUp = false;

        spell1Collider.SetActive(true);
        StartCoroutine(Spell1Wating());
    }

    private IEnumerator Spell1Wating()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.spell1);
        yield return new WaitForSeconds(3f);
        spell1Collider.SetActive(false);
        canPickUp = true;
    }

    public bool Spell2()
    {
        List<Transform> children = new List<Transform>();
        
        foreach (Transform child in parentObject.transform)
        {
            // if(setMap.CheckSpecialObject(child.gameObject) >=0 ) continue;
            // if(setMap.CheckTaskObject(child.gameObject) >= 0) continue;

            // children.Add(child);
            if(setMap.CheckTaskObject(child.gameObject) >= 0) children.Add(child);
        }

        // Sử dụng từ điển để lưu tên và số lượng phần tử con có cùng tên
        Dictionary<string, List<Transform>> nameGroups = new Dictionary<string, List<Transform>>();

        // Lặp qua từng GameObject con và nhóm chúng theo tên
        foreach (Transform child in children)
        {
            if (!nameGroups.ContainsKey(child.name))
            {
                nameGroups[child.name] = new List<Transform>();
            }
            nameGroups[child.name].Add(child);
        }

        // Tìm và xóa nhóm đầu tiên có ít nhất 3 phần tử cùng tên
        foreach (var group in nameGroups)
        {
            if (group.Value.Count >= 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    group.Value[i].gameObject.transform.SetParent(null);
                    StartCoroutine(Spell2Effect(group.Value[i].gameObject));
                    // Kiểm tra vật phẩm nhiệm vụ
                    taskController.CheckPickObjectTask(group.Value[i].gameObject);
                }
                SoundManager.Instance.PlaySFX(SoundManager.Instance.spell2);
                return true; 
            }
        }
        return false;
    }

    private IEnumerator Spell2Effect(GameObject obj)
    {
        obj.GetComponent<Outline>().enabled = true;
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.GetComponent<MeshCollider>().enabled = false;

        yield return new WaitForSeconds(0.2f);
        // tao tia set
        GameObject lineElectric = Instantiate(spell2LineElectric, gameObject.transform.position, Quaternion.identity);

        Vector3 newPos = new Vector3 (obj.transform.position.x + (obj.transform.position.x > 0 ? -1f : 1f), 25f, obj.transform.position.z + (obj.transform.position.z > 4.5 ? -0.5f : 1f));
        Quaternion newRotation = Quaternion.Euler(90, 0, 0); 
        Vector3 objScale = obj.transform.localScale;

        while (Vector3.Distance(obj.transform.position, newPos) > 0.3f)
        {
            // Cập nhật vị trí cho Line electric
            lineElectric.GetComponent<LineElectric>().Setline(spell2Point, obj);

            obj.transform.position = Vector3.Lerp(obj.transform.position, newPos, Time.deltaTime * 5f);
            obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, newRotation, Time.deltaTime * 5f); 
            obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, objScale * 1.2f, Time.deltaTime * 5f);
            yield return null;
        }

        obj.transform.position = newPos;
        obj.transform.rotation = newRotation;
        obj.transform.localScale = objScale * 1.4f;


        Destroy(obj);
        Destroy(lineElectric);
    }

    public void Spell3()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.spell3);
        StartCoroutine(Spell3Effect(spell3Point, clockSpell3));
    }
    private IEnumerator Spell3Effect(GameObject objStart, GameObject objEnd)
    {
        GameObject snowParticle = Instantiate(particleSpell3, objStart.transform.position, Quaternion.identity);
        Vector3 newPos = objEnd.transform.position;
        while (Vector3.Distance(snowParticle.transform.position, newPos) > 0.3f)
        {
            snowParticle.transform.position = Vector3.MoveTowards(snowParticle.transform.position, newPos, Time.deltaTime * 15f);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        Destroy(snowParticle);
    }
    
    public IEnumerator ClearBox()
    {
        for(int i = inBoxObjects.Count -1; i >= 0 ; i--)
        {
            GameObject obj = inBoxObjects[i];
            obj.transform.SetParent(parentObject.transform);

            //Tạo 1 bản sao tương tự
            GameObject newObj = Instantiate(setMap.CopyObject(obj), obj.transform.position, obj.transform.localRotation, parentObject.transform);
            Vector3 newObjScale = newObj.transform.localScale;
            newObj.transform.localScale = obj.transform.localScale;
            // Đẩy ra khỏi Box && hiệu ứng
            selectBoxs[inBoxObjects.Count - 1].GetComponent<Animator>().SetTrigger("isSpring");
            StartCoroutine(PushAndZoom(newObj, newObjScale));
            // Kiểm tra vật phẩm nhiệm vụ
            taskController.CheckRemoveObjectTask(newObj);
             
            selectObjects.Remove(obj);
            inBoxObjects.Remove(obj);
            Destroy(obj);

            SoundManager.Instance.PlaySFX(SoundManager.Instance.spell0);
            yield return  new WaitForSeconds(0.1f);
        }
    }

    private void PickSpecialObject(int index, GameObject obj)
    {
        if(index == 0) 
        {
            StartCoroutine(MoveSpecialObject0(obj));
            timeClock.AddTimeClock(15);
            return;
        }

        if(index == 1) 
        {
            StartCoroutine(MoveSpecialObject1(obj));
            timeClock.MinusTimeClock(15);
            return;
        }
        if(index == 2) 
        {
            StartCoroutine(MoveSpecialObject2(obj));
        }
    }

    private IEnumerator MoveSpecialObject0(GameObject obj)
    {
        GameObject fxBulletBlue1 = Instantiate(fxBulletBlue, obj.transform.position, Quaternion.identity);
        fxBulletBlue1.GetComponent<ArcMovement>().SetData(clockSpell3.transform, 1f, 2);
        GameObject fxBulletBlue2 = Instantiate(fxBulletBlue, obj.transform.position, Quaternion.identity);
        fxBulletBlue2.GetComponent<ArcMovement>().SetData(clockSpell3.transform, 1f, -2);
        GameObject fxBulletBlue3 = Instantiate(fxBulletBlue, obj.transform.position, Quaternion.identity);
        fxBulletBlue3.GetComponent<ArcMovement>().SetData(clockSpell3.transform, 1f, 0);

        yield return StartCoroutine(MoveSpecialObject(obj));

        
        fxBulletBlue1.GetComponent<ArcMovement>().StartArcMovement();
        fxBulletBlue2.GetComponent<ArcMovement>().StartArcMovement();
        fxBulletBlue3.GetComponent<ArcMovement>().StartArcMovement();

        GameObject explosionBlue = Instantiate(fxExplosionBlue, obj.transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySFX(SoundManager.Instance.explosion);
        yield return new WaitForSeconds(1f);
        Destroy(explosionBlue);
    }

    private IEnumerator MoveSpecialObject1(GameObject obj)
    {
        GameObject fxBulletblack1 = Instantiate(fxBulletBlack, obj.transform.position, Quaternion.identity);
        fxBulletblack1.GetComponent<ArcMovement>().SetData(clockSpell3.transform, 1f, 2);
        GameObject fxBulletblack2 = Instantiate(fxBulletBlack, obj.transform.position, Quaternion.identity);
        fxBulletblack2.GetComponent<ArcMovement>().SetData(clockSpell3.transform, 1f, -2);
        GameObject fxBulletblack3 = Instantiate(fxBulletBlack, obj.transform.position, Quaternion.identity);
        fxBulletblack3.GetComponent<ArcMovement>().SetData(clockSpell3.transform, 1f, 0);
        
        yield return StartCoroutine(MoveSpecialObject(obj));

        fxBulletblack1.GetComponent<ArcMovement>().StartArcMovement();
        fxBulletblack2.GetComponent<ArcMovement>().StartArcMovement();
        fxBulletblack3.GetComponent<ArcMovement>().StartArcMovement();

        GameObject explosionBlack = Instantiate(fxExplosionBlack, obj.transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySFX(SoundManager.Instance.explosion);
        yield return new WaitForSeconds(1f);
        Destroy(explosionBlack);

    }

    private IEnumerator MoveSpecialObject2(GameObject obj)
    {
        yield return StartCoroutine(MoveSpecialObject(obj));
        GameObject explosionRed = Instantiate(fxExplosionRed, obj.transform.position, Quaternion.identity);
        
        List<Transform> children = new List<Transform>();
        
        foreach (Transform child in parentObject.transform)
        {
            if(setMap.CheckNomalObject(child.gameObject) >=0 ) 
            children.Add(child);
        }

        // Sử dụng từ điển để lưu tên và số lượng phần tử con có cùng tên
        Dictionary<string, List<Transform>> nameGroups = new Dictionary<string, List<Transform>>();

        // Lặp qua từng GameObject con và nhóm chúng theo tên
        foreach (Transform child in children)
        {
            if (!nameGroups.ContainsKey(child.name))
            {
                nameGroups[child.name] = new List<Transform>();
            }
            nameGroups[child.name].Add(child);
        }

        // Tìm và xóa nhóm đầu tiên có ít nhất 3 phần tử cùng tên
        foreach (var group in nameGroups)
        {
            if (group.Value.Count >= 3)
            {
                SoundManager.Instance.PlaySFX(SoundManager.Instance.rocket);
                float timeMove = 0.7f;
                for (int i = 0; i < 3; i++)
                {
                    group.Value[i].gameObject.transform.SetParent(null);
                    GameObject fxRocketSpawn = Instantiate(fxBulletRocket, obj.transform.position, Quaternion.identity);
                    fxRocketSpawn.GetComponent<ArcMovement>().SetData(group.Value[i].gameObject.transform, timeMove, Random.Range(-3f, 3f));
                    fxRocketSpawn.GetComponent<ArcMovement>().StartArcMovement();
                    StartCoroutine(RocketEffect(group.Value[i].gameObject, timeMove));
                    timeMove += 0.2f;              
                }
                
                break;
            }
        }

        
        yield return new WaitForSeconds(1f);
        Destroy(explosionRed);
    }

    private IEnumerator RocketEffect(GameObject obj, float time)
    {
        obj.GetComponent<Outline>().enabled = true;
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.GetComponent<MeshCollider>().enabled = false;

        yield return new WaitForSeconds(time + 0.2f);
        SoundManager.Instance.PlaySFX(SoundManager.Instance.rocketHit);
        Destroy(obj);
    }

    private IEnumerator MoveSpecialObject(GameObject obj)
    {
        obj.GetComponent<Outline>().enabled = true;
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.GetComponent<MeshCollider>().enabled = false;

        // Thay đổi vị trí, hướng quay và tỉ lệ của đối tượng
        Vector3 newPos = obj.transform.position;
        newPos.y += 10f;
        obj.transform.position = newPos;
        
        obj.transform.rotation = Quaternion.Euler(90, 0, 0); 
        obj.transform.localScale = obj.transform.localScale * 1.2f;

        float elapsedTime = 0f, duration = 0.6f;
        while (elapsedTime < duration)
        {
            int randomYRotation = Random.Range(-10, 10);
            obj.transform.rotation = Quaternion.Euler(90, randomYRotation, 0);

            elapsedTime += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }

        Destroy(obj);
    }

    private IEnumerator PickTwinObject(TwinObject twinObj, GameObject obj)
    {
        
        yield return StartCoroutine(TwinObjMoveUp(obj));
        obj.GetComponent<Animator>().SetTrigger("isBroken");
        SoundManager.Instance.PlaySFX(SoundManager.Instance.pop);

        GameObject obj1 = obj.transform.GetChild(0).gameObject;
        GameObject obj1Spawn = Instantiate(twinObj.obj1, obj1.transform.position, obj1.transform.rotation, parentObject.transform);
        obj1Spawn.transform.localScale = obj1Spawn.transform.localScale * 1.4f;
        Destroy(obj1);
        obj1Spawn.GetComponent<Rigidbody>().isKinematic = true;
        obj1Spawn.GetComponent<MeshCollider>().enabled = false; 
        // Kiểm tra vật phẩm nhiệm vụ
        taskController.CheckPickObjectTask(obj1Spawn);

        GameObject obj2 = obj.transform.GetChild(1).gameObject;
        GameObject obj2Spawn = Instantiate(twinObj.obj2, obj2.transform.position, obj2.transform.rotation, parentObject.transform);
        obj2Spawn.transform.localScale = obj2Spawn.transform.localScale * 1.4f;
        Destroy(obj2);
        obj2Spawn.GetComponent<Rigidbody>().isKinematic = true;
        obj2Spawn.GetComponent<MeshCollider>().enabled = false;
        // Kiểm tra vật phẩm nhiệm vụ
        taskController.CheckPickObjectTask(obj2Spawn);
        

        int index = IndexInsert(obj1Spawn, selectObjects) ;
        selectObjects.Insert(index, obj1Spawn);
        StartCoroutine(TwinObjMoveToBox(obj1Spawn, -2.5f));

        yield return null;
       
        index = IndexInsert(obj2Spawn, selectObjects) ;
        selectObjects.Insert(index, obj2Spawn);
        StartCoroutine(TwinObjMoveToBox(obj2Spawn, 2.5f));

        yield return new WaitForSeconds(2f);
        Destroy(obj);
    }

    private IEnumerator TwinObjMoveUp(GameObject obj)
    {
        Vector3 newPos = new Vector3 (0f, 15f, 1f);
        Quaternion newRotation = Quaternion.Euler(0, 0, 0); 
        Vector3 objScale = obj.transform.localScale;

        while (Vector3.Distance(obj.transform.position, newPos) > 0.3f)
        {
            // Di chuyển lên
            obj.transform.position = Vector3.Lerp(obj.transform.position, newPos, Time.deltaTime * moveSpeed);
            // Xoay
            obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, newRotation, Time.deltaTime * moveSpeed); 
            // Zoom in
            obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, objScale * 1.4f, Time.deltaTime * moveSpeed);
            yield return null;
        }

        obj.transform.position = newPos;
        obj.transform.rotation = newRotation;
        obj.transform.localScale = objScale * 1.4f;
    }

    private IEnumerator TwinObjMoveToBox(GameObject obj, float posX)
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.objectSelect);

        obj.transform.SetParent(selectBoxs[0].transform);
        Vector3 newPos = new Vector3 (posX, 15f, 4f);

        while (Vector3.Distance(obj.transform.position, newPos) > 0.3f)
        {
            // Di chuyển lên
            obj.transform.position = Vector3.Lerp(obj.transform.position, newPos, Time.deltaTime * moveSpeed);
            yield return null;
        }
        obj.transform.position = newPos;

        Vector3 objScale = new Vector3(66f,66f,66f);

        // di chuyển về box ứng với selectObject
        GameObject box = selectBoxs[selectObjects.IndexOf(obj)];
        Vector3 targetPosition = box.transform.position;  
        while (Vector3.Distance(obj.transform.position, targetPosition) > 0.3f)
        {
            box = selectBoxs[selectObjects.IndexOf(obj)];
            targetPosition = box.transform.position;
            // Di chuyển về box
            obj.transform.position = Vector3.Lerp(obj.transform.position, targetPosition, Time.deltaTime * moveSpeed);
            // Zoom out
            obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, objScale, Time.deltaTime * moveSpeed);
            // Animation box
            yield return null;
        }
        obj.transform.position = targetPosition;
        obj.transform.localScale = objScale;
        obj.transform.SetParent(box.transform);

        //BingGo
        int index  = IndexInsert(obj, inBoxObjects);
        inBoxObjects.Insert(index, obj); 
        BingGo(index); 

        box.GetComponent<Animator>().SetTrigger("UpDown");
    }
    
}

