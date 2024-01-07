using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] public Slider slider;

    public void UpdateHealthBar(float value)
    {
        slider.value = value;
    }
}
