using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineElectric : MonoBehaviour
{ 
    private int segments = 10;     // Số đoạn của tia sét
    private float displacement = 0.5f; // Biên độ lêch
    private float updateInterval = 0.05f; // Tần suất cập nhật tia sét

    private Transform startPoint;  
    private Transform endPoint;   
    private Vector3[] points;

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments; 
        points = new Vector3[segments];

        StartCoroutine(PlayLine());    
    }


    public void Setline(GameObject startObj, GameObject endObj)
    {
        startPoint = startObj.transform;
        endPoint = endObj.transform;
    }

    private IEnumerator PlayLine()
    {
        while(true)
        {
            points[0] = startPoint.position;
            points[segments - 1] = endPoint.position;

            for (int i = 1; i < segments - 1; i++)
            {
                float lerpFactor = (float)i / (segments - 1);
                Vector3 point = Vector3.Lerp(startPoint.position, endPoint.position, lerpFactor);

                // Thêm độ lệch ngẫu nhiên để tạo hiệu ứng tia sét
                point.x += Random.Range(-displacement, displacement);
                point.y += Random.Range(-displacement, displacement);
                point.z += Random.Range(-displacement, displacement);

                points[i] = point;
            }

            lineRenderer.SetPositions(points);

            yield return new WaitForSeconds(updateInterval);
        }
    }
}
