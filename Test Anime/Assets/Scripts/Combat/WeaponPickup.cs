using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        Canvas canvas;
        [SerializeField] GameObject pivot;
        [SerializeField] Text weaponName;
        public InventoryItem weapon = null;
        public ItemInstance weaponInstance;
        public bool wasFromInventory;
        [SerializeField] float respawnTime = 5;
        Camera _camera;

        private void Awake()
        {
            canvas = GetComponentInChildren<Canvas>();
            canvas.worldCamera = Camera.main;
            _camera = Camera.main;
        }

        private void Start()
        {
            weaponName.text = weapon.displayName;

            weaponInstance = new ItemInstance(weapon, 1);
        }

        private void Update()
        {
            pivot.transform.LookAt(_camera.transform.position);
        }

        public void Hide()
        {
            ShowPickup(false);
        }

        private IEnumerator HideForSeconds(float time)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(time);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }

    }
}

