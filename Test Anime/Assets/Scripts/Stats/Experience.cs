using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;

        public event Action onExperienceGained; //a predefined delegate with no return value


        public object CaptureState()
        {
            return experiencePoints;
        }

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            onExperienceGained();
        }

        public void RestoreState(object state)
        {
            this.experiencePoints = (float) state;
        }

        public float GetCurrentExperience()
        {
            return experiencePoints;
        }
    }
}

