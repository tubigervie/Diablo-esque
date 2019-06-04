using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Resource
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] float experiencePoints = 0;

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
        }
    }
}

