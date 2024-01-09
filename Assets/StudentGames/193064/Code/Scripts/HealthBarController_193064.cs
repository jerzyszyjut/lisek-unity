using UnityEngine;
using UnityEngine.UI;

namespace _193064
{
    public class HealthBarController : MonoBehaviour
    {
        [SerializeField] public Slider slider;

        public void UpdateHealthBar(float value)
        {
            slider.value = value;
        }
    }
}