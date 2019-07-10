using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Resource;
using System;

namespace RPG.UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] Image healthSlider;
        [SerializeField] Health healthComponent;
        [SerializeField] Text healthText;

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
            float current = healthComponent.GetCurrentHealth();
            float total = healthComponent.GetTotalHealth();
            healthSlider.fillAmount = current / total;
            healthText.text = "HP: " + Mathf.Round(current) + " / " + Mathf.Round(total);
        }

    }
}

