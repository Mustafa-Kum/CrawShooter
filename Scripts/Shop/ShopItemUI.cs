using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [Header("Components")]
    [Space]
    [SerializeField] private Image icon;
    [SerializeField] private Image grayOut;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button button;
    [SerializeField] private Color notEnoughCreditColor;
    [SerializeField] private Color enoughCreditColor;

    private ShopItem item;

    public delegate void OnItemSelected(ShopItemUI selectedItem);
    public event OnItemSelected onItemSelected;

    private void Start()
    {
        button.onClick.AddListener(ItemSelected);
    }

    private void ItemSelected()
    {
        onItemSelected?.Invoke(this);
    }

    public void Init(ShopItem item, int currentCredit)
    {
        this.item = item;

        icon.sprite = item.icon;
        titleText.text = item.title;
        priceText.text = "$" + item.price.ToString();
        descriptionText.text = item.description;

        Refresh(currentCredit);
    }

    public void Refresh(int currentCredit)
    {
        if (currentCredit < item.price)
        {
            grayOut.enabled = true;
            priceText.color = notEnoughCreditColor;
        }
        else
        {
            grayOut.enabled = false;
            priceText.color = enoughCreditColor;
        }
    }

    internal ShopItem GetItem()
    {
        return item;
    }
}
