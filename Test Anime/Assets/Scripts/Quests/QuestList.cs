using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Questing
{
    [CreateAssetMenu(menuName = ("RPG/Quest List"))]
    public class QuestList : ScriptableObject
    {
        [SerializeField] Quest[] quests;

        public Quest GetQuestByID(string id)
        {
            foreach(var quest in quests)
            {
                if (quest.uniqueName == id)
                    return quest;
            }
            return null;
        }
    }
}

