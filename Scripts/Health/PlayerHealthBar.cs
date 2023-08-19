using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Image amtImage;

    internal void UpdateHealth(float health, float delta, float maxHealth)
    {
        amtImage.fillAmount = health / maxHealth;
    }
}
