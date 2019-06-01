using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Resources;

namespace RPG.UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] Image healthSlider;
        [SerializeField] Health healthComponent;

        private void Start()
        {
            healthSlider.fillAmount = healthComponent.GetCurrentHealth() / healthComponent.GetTotalHealth();
        }

        private void Update()
        {
            healthSlider.fillAmount = healthComponent.GetCurrentHealth() / healthComponent.GetTotalHealth();
        }

    }
}

