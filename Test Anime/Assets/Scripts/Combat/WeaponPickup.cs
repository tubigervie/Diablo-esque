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
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 5;
        Camera camera;

        private void Awake()
        {
            canvas = GetComponentInChildren<Canvas>();
            canvas.worldCamera = Camera.main;
            camera = Camera.main;
        }

        private void Start()
        {
            weaponName.text = weapon.name;
        }

        private void Update()
        {
            pivot.transform.LookAt(camera.transform.position);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                StartCoroutine(HideForSeconds(respawnTime));
            }
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

