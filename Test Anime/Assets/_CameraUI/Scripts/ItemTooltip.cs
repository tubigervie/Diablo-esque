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
                bodyText.text = value;
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

        public void SpawnModifier(BonusType bonus, float amount, Stat stat)
        {
            modifierText.Spawn(bonus, amount, stat);
        }
    }
}