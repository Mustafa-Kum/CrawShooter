using System.Collections.Generic;
using UnityEngine;

public class InventoryComponent : MonoBehaviour, IPurchaseListener
{
    [Header("Components")]
    [Space]
    [SerializeField] private Weapon[] initialWeaponPrefabs;
    [SerializeField] private Transform[] weaponSlots;
    [SerializeField] private Transform defaultWeaponSlot;

    private List<Weapon> weapons;
    private int currentWeaponIndex = -1;

    private void Start()
    {
        InitWeapons();
    }

    private void InitWeapons()
    {
        weapons = new List<Weapon>();

        foreach (Weapon weapon in initialWeaponPrefabs)
        {
            GiveNewWeapon(weapon);
        }

        NextWeapon();
    }

    private void GiveNewWeapon(Weapon weapon)
    {
        Transform weaponSlot = defaultWeaponSlot;

        foreach (Transform slot in weaponSlots)
        {
            if (slot.gameObject.CompareTag(weapon.GetAttackSlotTag()))
            {
                weaponSlot = slot;
                break;
            }
        }

        Weapon newWeapon = Instantiate(weapon, weaponSlot);
        newWeapon.Init(gameObject);

        weapons.Insert(0, newWeapon);
    }

    public void NextWeapon()
    {
        int nextWeaponIndex = currentWeaponIndex + 1;

        if (nextWeaponIndex >= weapons.Count)
        {
            nextWeaponIndex = 0;
        }

        EquipWeapon(nextWeaponIndex);
    }

    private void EquipWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= weapons.Count)
            return;

        if (currentWeaponIndex >= 0 && currentWeaponIndex < weapons.Count)
            weapons[currentWeaponIndex].Unequip();

        weapons[weaponIndex].Equip();
        currentWeaponIndex = weaponIndex;
    }

    internal Weapon GetActiveWeapon()
    {
        return weapons[currentWeaponIndex];
    }

    public bool HandlePurchase(Object newPurchase)
    {
        GameObject itemAsGameObject = newPurchase as GameObject;

        if (itemAsGameObject == null)
            return false;

        Weapon itemAsWeapon = itemAsGameObject.GetComponent<Weapon>();

        if (itemAsWeapon == null)
            return false;

        GiveNewWeapon(itemAsWeapon);
        EquipWeapon(0);

        return true;
    }
}
