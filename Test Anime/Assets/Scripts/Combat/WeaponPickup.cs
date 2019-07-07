using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class WeaponPickup : ItemPickup
    {
        public Weapon weapon = null;
        public WeaponInstance weaponInstance;

        public override void InitRandom(int level, bool isDrop = false)
        {
            itemName.text = item.displayName;
            weaponInstance = new WeaponInstance(weapon, 1);
        }

        public override void InitInstance(ItemInstance item)
        {
            itemName.text = item.itemBase.name;
            WeaponInstance weapInst = new WeaponInstance(item.itemBase as Weapon, item.properties);
            this.weapon = weapInst.weaponBase;
            weaponInstance = weapInst;
        }
    }
}

