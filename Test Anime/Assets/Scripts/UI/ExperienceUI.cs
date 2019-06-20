using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceUI : MonoBehaviour
{
    BaseStats baseStats;
    Experience experience;
    [SerializeField] Image experienceSlider;
    [SerializeField] Text experiencePoints;

    private void Awake()
    {
        experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
    }

    private void Update()
    {
        float XPtoLevelUp = baseStats.GetStat(Stat.ExperienceToLevelUp);
        experiencePoints.text ="Lvl " + baseStats.GetLevel() + ": " + experience.GetCurrentExperience() + "/" + XPtoLevelUp;
        experienceSlider.fillAmount = experience.GetCurrentExperience() / XPtoLevelUp;
    }
}
