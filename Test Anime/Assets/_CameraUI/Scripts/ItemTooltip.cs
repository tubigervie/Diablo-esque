using UnityEngine;
using UnityEngine.UI;

namespace RPG.CameraUI
{
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField] Text titleText;
        [SerializeField] Text bodyText;

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
    }
}
