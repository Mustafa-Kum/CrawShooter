using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaBar : MonoBehaviour
{
    [SerializeField] private Image amtImage;

    internal void UpdateStamina(float health, float delta, float maxHealth)
    {
        float fillAmount = health / maxHealth;
        float currentFillAmount = amtImage.fillAmount;
        float smoothFillAmount = Mathf.Lerp(currentFillAmount, fillAmount, Time.deltaTime * 2);
        amtImage.fillAmount = smoothFillAmount;
    }
}
