using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Components")]
    [Space]
    [SerializeField] private RectTransform thumbStickTransform;
    [SerializeField] private RectTransform backgroundTransform;
    [SerializeField] private RectTransform centerTransform;

    public delegate void OnStickInputValueUpdated(Vector2 inputValue);
    public delegate void OnStickTabbed();

    public event OnStickInputValueUpdated onStickInputValueUpdated;
    public event OnStickTabbed onStickTabbed;

    private bool bWasDragging;

    public void OnDrag(PointerEventData eventData)
    {
        LimitingButtonPosition(eventData);

        bWasDragging = true;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        // Position of Touching for Stick.

        backgroundTransform.position = eventData.position;
        thumbStickTransform.position = eventData.position;

        bWasDragging = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Reseting Position of Stick.

        backgroundTransform.position = centerTransform.position;
        thumbStickTransform.position = backgroundTransform.position;

        onStickInputValueUpdated?.Invoke(Vector2.zero);

        if (!bWasDragging)
        {
            onStickTabbed?.Invoke();
        }
    }

    private void LimitingButtonPosition(PointerEventData eventData)
    {
        Vector2 touchPos = eventData.position;
        Vector2 centerPos = backgroundTransform.position;
        Vector2 localOffSet = Vector2.ClampMagnitude(touchPos - centerPos, backgroundTransform.sizeDelta.x / 5);
        Vector2 inputValue = localOffSet / (backgroundTransform.sizeDelta.x / 5);

        thumbStickTransform.position = centerPos + localOffSet;

        onStickInputValueUpdated?.Invoke(inputValue);
    }

}
