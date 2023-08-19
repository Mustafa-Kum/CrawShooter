using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCreditBar : MonoBehaviour
{
    [Header("Components")]
    [Space]
    [SerializeField] private Button shopButton;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private CreditComponent creditComponent;
    [SerializeField] private TextMeshProUGUI creditText;

    private void Start()
    {
        shopButton.onClick.AddListener(ShowShop);
        creditComponent.onCreditChanged += UpdateCredit;
        UpdateCredit(creditComponent.credit);
    }

    private void UpdateCredit(int newCredit)
    {
        creditText.SetText(newCredit.ToString());
    }

    private void ShowShop()
    {
        uiManager.SwitchToShop();
    }
}
