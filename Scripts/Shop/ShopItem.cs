using UnityEngine;

[CreateAssetMenu(menuName = "Shop/ShopItem")]

public class ShopItem : ScriptableObject
{
    public Object item;
    public Sprite icon;
    public string title;
    public int price;
    [TextArea(10, 10)]
    public string description;
}
