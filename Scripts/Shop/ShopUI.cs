using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("Components")]
    [Space]
    [SerializeField] private ShopSystem shopSystem;
    [SerializeField] private ShopItemUI shopItemUIPrefab;
    [SerializeField] private RectTransform shopList;
    [SerializeField] private CreditComponent creditComponent;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Button backButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI creditText;

    private List<ShopItemUI> shopItems = new List<ShopItemUI>();
    private ShopItemUI selectedItem;

    private void Start()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        backButton.onClick.AddListener(uiManager.SwitchToGamePlayUI);
        buyButton.onClick.AddListener(TryPurchaseItem);

        creditComponent.onCreditChanged += UpdateCredit;

        UpdateCredit(creditComponent.credit);
        InitShopItems();
    }

    private void TryPurchaseItem()
    {
        if (!selectedItem || !shopSystem.TryPurchase(selectedItem.GetItem(), creditComponent))
            return;

        RemoveItem(selectedItem);
    }

    private void RemoveItem(ShopItemUI itemToRemove)
    {
        shopItems.Remove(itemToRemove);
        Destroy(itemToRemove.gameObject);
    }

    private void UpdateCredit(int newCredit)
    {
        creditText.SetText(newCredit.ToString());
        RefreshItems();
    }

    private void RefreshItems()
    {
        foreach (ShopItemUI shopItemUI in shopItems)
        {
            shopItemUI.Refresh(creditComponent.credit);
        }
    }

    private void InitShopItems()
    {
        ShopItem[] shopItems = shopSystem.GetShopItems();

        foreach (ShopItem item in shopItems)
        {
            AddShopItem(item);
        }
    }

    private void AddShopItem(ShopItem item)
    {
        ShopItemUI newItemUI = Instantiate(shopItemUIPrefab, shopList);
        newItemUI.Init(item, creditComponent.credit);
        newItemUI.onItemSelected += ItemSelected;

        shopItems.Add(newItemUI);
    }

    private void ItemSelected(ShopItemUI item)
    {
        selectedItem = item;
    }
}
