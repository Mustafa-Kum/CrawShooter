using System.Collections.Generic;
using UnityEngine;

public interface IPurchaseListener
{
    public bool HandlePurchase(Object newPurchase);
}

public class CreditComponent : MonoBehaviour, IRewardListener
{
    [Header("Components")]
    [Space]
    [SerializeField] private Component[] purchaseListeners;
    [SerializeField] private int credits;

    private List<IPurchaseListener> purchaseListenerInterface = new List<IPurchaseListener>();
    public int credit { get { return credits; } }

    public delegate void OnCreditChanged(int newCredit);
    public event OnCreditChanged onCreditChanged;

    private void Start()
    {
        CollectPurchaseListeners();
    }

    private void CollectPurchaseListeners()
    {
        foreach (Component listener in purchaseListeners)
        {
            IPurchaseListener listenerInterface = listener as IPurchaseListener;

            if (listenerInterface != null)
                purchaseListenerInterface.Add(listenerInterface);
        }
    }

    private void BroadCastPurchase(Object item)
    {
        foreach (IPurchaseListener purchaseListener in purchaseListenerInterface)
        {
            if (purchaseListener.HandlePurchase(item))
                return;
        }
    }

    public bool Purchase(int price, Object item)
    {
        if (credits < price)
            return false;

        credits -= price;
        onCreditChanged?.Invoke(credits);
        BroadCastPurchase(item);

        return true;
    }

    public void Reward(Reward reward)
    {
        credits += reward.creditReward;
        onCreditChanged?.Invoke(credits);
    }
}
