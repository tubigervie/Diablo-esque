using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Resource;

namespace RPG.UI
{
    public class EnemyHealthUI : MonoBehaviour
    {
        [SerializeField] Text enemyName;
        [SerializeField] Image healthSlider;
        [SerializeField] GameObject healthUI;
        public Health target;

        public bool IsActive()
        {
            return healthUI.activeInHierarchy;
        }

        public void OnEnabled(Health combatTarget)
        {
            target = combatTarget;
            enemyName.text = combatTarget.gameObject.name;
            healthSlider.fillAmount = combatTarget.GetCurrentHealth() / combatTarget.GetTotalHealth();
            healthUI.SetActive(true);
        }

        public void Disable()
        {
            target = null;
            healthUI.SetActive(false);
        }

        private void Update()
        {
            if (healthUI.activeInHierarchy && target != null)
            {
                healthSlider.fillAmount = target.GetCurrentHealth() / target.GetTotalHealth();
                if (target.IsDead() || Vector3.Distance(transform.position, target.transform.position) > 20)
                {
                    Disable();
                }
            }
        }


    }
}

