using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    private Transform _attachPoint;

    public void Init(Transform attachPoint)
    {
        _attachPoint = attachPoint;
    }

    public void SetHealthSliderValue(float health, float delta, float maxHealth)
    {
        healthSlider.value = health / maxHealth;
    }

    internal void onOwnerDead(GameObject killer)
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        Vector3 attachScreenPoint = Camera.main.WorldToScreenPoint(_attachPoint.position);

        transform.position = attachScreenPoint;
    }
}
