using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.CameraUI
{
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField] Text titleText;
        [SerializeField] Text bodyText;
        [SerializeField] Text valueText;
        [SerializeField] Text rarityText;
        [SerializeField] ModifierTextSpawner modifierText;

        public string title
        {
            set
            {
                titleText.text = value;
            }
            get
            {
                return titleText.text;
            }
        }

        public string body
        {
            set
            {
                bodyText.text = "\"" + value + "\"";
            }
            get
            {
                return bodyText.text;
            }
        }

        public string value
        {
            set
            {
                valueText.text = value;
            }
            get
            {
                return valueText.text;
            }
        }

        public void setRarity(Rarity rarity)
        {
            rarityText.text = rarity.ToString();
            switch (rarity)
            {
                case Rarity.Common:
                    rarityText.color = Color.white;
                    break;
                case Rarity.Uncommon:
                    rarityText.color = Color.yellow;
                    break;
                case Rarity.Rare:
                    rarityText.color = Color.green;
                    break;
                case Rarity.Epic:
                    rarityText.color = Color.blue;
                    break;
                case Rarity.Legendary:
                    rarityText.color = Color.cyan;
                    break;
                case Rarity.Mythical:
                    rarityText.color = Color.red;
                    break;
            }
        }

        public void SpawnModifier(BonusType bonus, float amount, Stat stat)
        {
            if(stat == Stat.CriticalHitChance)
                modifierText.Spawn(BonusType.Percentage, amount, stat);
            else
                modifierText.Spawn(bonus, amount, stat);
        }
    }
}