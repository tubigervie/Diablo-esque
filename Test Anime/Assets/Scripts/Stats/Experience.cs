using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        public bool alreadyLoaded = false;

        [SerializeField] float experiencePoints = 0;

        public event Action onExperienceGained; //a predefined delegate with no return value

        public event Action onExperienceLoaded;

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
            if(alreadyLoaded)
                onExperienceLoaded();
        }

        public float GetCurrentExperience()
        {
            return experiencePoints;
        }
    }
}

