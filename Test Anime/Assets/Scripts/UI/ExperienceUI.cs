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

    private void OnEnable()
    {
        experience.onExperienceGained += UpdateExperience;
        experience.onExperienceLoaded += UpdateExperience;
        baseStats.onLevelUp += UpdateExperience;
    }

    private void OnDisable()
    {
        experience.onExperienceGained -= UpdateExperience;
        experience.onExperienceLoaded -= UpdateExperience;
        baseStats.onLevelUp -= UpdateExperience;
    }

    public void UpdateExperience()
    {
        float XPtoLevelUp = baseStats.GetStat(Stat.ExperienceToLevelUp);
        float prevXPtoLevel = 0;
        if (baseStats.GetLevel() > 1)
            prevXPtoLevel = baseStats.GetStatByLevel(Stat.ExperienceToLevelUp, baseStats.GetLevel() - 1);
        experiencePoints.text ="Lv " + baseStats.GetLevel() + ": " + experience.GetCurrentExperience() + "/" + XPtoLevelUp;
        experienceSlider.fillAmount = (experience.GetCurrentExperience() - prevXPtoLevel) / (XPtoLevelUp - prevXPtoLevel);
    }
}
