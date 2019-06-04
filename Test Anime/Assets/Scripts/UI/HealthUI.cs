using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Resource;

namespace RPG.UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] Image healthSlider;
        [SerializeField] Health healthComponent;

        private void Awake()
        {
            healthComponent = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

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

