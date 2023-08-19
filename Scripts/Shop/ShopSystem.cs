using UnityEngine;

[CreateAssetMenu(menuName = "Shop/ShopSystem")]

public class ShopSystem : ScriptableObject
{
    [Header("Items")]
    [SerializeField] private ShopItem[] shopItems;

    public ShopItem[] GetShopItems()
    {
        return shopItems;
    }

    public bool TryPurchase(ShopItem selectedItem, CreditComponent purchaser)
    {
        return purchaser.Purchase(selectedItem.price, selectedItem.item);
    }
}
