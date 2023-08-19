using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityHolder : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Components")]
    [Space]
    [SerializeField] private AbilityComponent abilityComponent;
    [SerializeField] private RectTransform root;
    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;
    [SerializeField] private AbilityUI abilityUIPrefab;

    [Header("IconHighlight")]
    [Space]
    [SerializeField] private float highlightSize = 1.5f;
    [SerializeField] private float scaleSpeed = 20f;
    [SerializeField] private float scaleRange = 200f;

    private Vector3 goalScale = Vector3.one;
    private PointerEventData touchData;
    private AbilityUI highlightedAbility;
    private List<AbilityUI> abilityUIs = new List<AbilityUI>();

    private void Awake()
    {
        abilityComponent.onNewAbilityAdded += AddAbility;
    }

    void Update()
    {
        HandleTouchInteraction();
    }

    private void HandleTouchInteraction()
    {
        if (touchData != null)
        {
            GetUIUnderPointer(touchData, out highlightedAbility);
            ArrangeScale(touchData);
        }

        transform.localScale = Vector3.Lerp(transform.localScale, goalScale, Time.deltaTime * scaleSpeed);
    }

    private void ArrangeScale(PointerEventData touchData)
    {
        if (scaleRange == 0)
            return;

        float touchVerticalPosition = touchData.position.y;

        foreach (AbilityUI abilityUI in abilityUIs)
        {
            float abilityUIVerticalPosition = abilityUI.transform.position.y;
            float distance = Mathf.Abs(touchVerticalPosition - abilityUIVerticalPosition);

            if (distance > scaleRange)
            {
                abilityUI.SetScaleValue(0);
                continue;
            }

            float scaleValue = (scaleRange - distance) / scaleRange;
            abilityUI.SetScaleValue(scaleValue);
        }
    }

    private void AddAbility(Ability newAbility)
    {
        AbilityUI newAbilityUI = Instantiate(abilityUIPrefab, root);
        newAbilityUI.Init(newAbility);
        abilityUIs.Add(newAbilityUI);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        touchData = eventData;
        goalScale = Vector3.one * highlightSize;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (highlightedAbility)
        {
            highlightedAbility.ActivateAbility();
        }
        
        touchData = null;
        ResetScale();
        goalScale = Vector3.one;
    }

    private void ResetScale()
    {
        foreach (AbilityUI abilityUI in abilityUIs)
        {
            abilityUI.SetScaleValue(0);
        }
    }

    private bool GetUIUnderPointer(PointerEventData eventData, out AbilityUI abilityUI)
    {
        List<RaycastResult> findAbility = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, findAbility);

        abilityUI = null;

        foreach (RaycastResult result in findAbility)
        {
            abilityUI = result.gameObject.GetComponentInParent<AbilityUI>();

            if (abilityUI != null)
                return true;
        }

        return false;
    }
}
