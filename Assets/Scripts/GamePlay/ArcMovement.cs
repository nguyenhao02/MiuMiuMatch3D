using UnityEngine;
using System.Collections;

public class ArcMovement : MonoBehaviour
{
    public Transform targetPosition; // Vị trí B
    public float duration ; // Thời gian để đi từ A đến B
    public float arcHeight; // Độ cao của đường cong

    public void SetData(Transform target, float timeMove, float arc)
    {
        targetPosition = target;
        duration = timeMove;
        arcHeight = arc;
    }

    public void StartArcMovement()
    {
        StartCoroutine(MoveInArc(transform.position, targetPosition.position));
    }

    private IEnumerator MoveInArc(Vector3 startPoint, Vector3 endPoint)
    {
        yield return new WaitForSeconds(0.2f);

        transform.localRotation = Quaternion.Euler(90, 0, 0);
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            // Tỷ lệ phần trăm hoàn thành
            float t = elapsedTime / duration;

            // Lerp trên trục X và Z để di chuyển từ A đến B
            Vector3 currentPosition = Vector3.Lerp(startPoint, endPoint, t);

            // Tính chiều cao của vòng cung bằng cách thêm giá trị sin(t) vào vị trí Y
            float height = Mathf.Sin(Mathf.PI * t) * arcHeight;
            currentPosition.x += height;

            // Xác định hướng di chuyển
            Vector3 direction = currentPosition - transform.position;
            if (direction != Vector3.zero) // Tránh lỗi khi direction là (0, 0, 0)
            {
                // Lấy quaternion xoay theo hướng di chuyển
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // Giữ giá trị trục X là 90 độ
                transform.localRotation = Quaternion.Euler(90, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
            }

            transform.position = currentPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPoint;
        Destroy(gameObject);
    }

}
