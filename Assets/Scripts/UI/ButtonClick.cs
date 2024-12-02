using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEvents : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 originScale;

    void Start()
    {
        originScale = gameObject.transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.click);
        SoundManager.Instance.Vibration();
        gameObject.transform.localScale *= 0.97f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        gameObject.transform.localScale = originScale;
    }

}
