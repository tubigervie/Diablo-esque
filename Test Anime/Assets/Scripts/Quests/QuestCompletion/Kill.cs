using RPG.Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Questing
{
    [System.Serializable]
    [RequireComponent(typeof(Health))]
    public class Kill : QuestCompletion
    {

        private void OnEnable()
        {
            var health = GetComponent<Health>();
            health.onDie += CompleteQuest;
        }

        private void OnDisable()
        {
            var health = GetComponent<Health>();
            health.onDie -= CompleteQuest;
        }

    }
}

