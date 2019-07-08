using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appearance : MonoBehaviour
{
    const string skin = "Skin";

    [SerializeField] GameObject headSlot; //NEED TO FIX THAT WAY THIS CAN BE FOUND IN CODE RATHER THAN VIA INSPECTOR
    [SerializeField] GameObject currentBodyPrefab = null;
    [SerializeField] GameObject currentHeadPrefab = null;
    Material skinMat;

    [SerializeField] GameObject testBodyPrefab;

    public bool isMale;

    Fighter fighter;

    private void Awake()
    {
        fighter = GetComponent<Fighter>();
    }

    private void Start()
    {
        currentBodyPrefab = this.transform.GetChild(0).gameObject;
        GameObject skinGO = currentBodyPrefab.transform.Find(skin).gameObject;
        skinMat = skinGO.GetComponent<SkinnedMeshRenderer>().material;
        currentHeadPrefab = headSlot.transform.GetChild(0).gameObject;
        headSlot = GameObject.FindGameObjectWithTag("PlayerHead");
    }

    public IEnumerator EquipBody(GameObject bodyPrefab)
    {
        Vector3 oldHeadTransform = currentHeadPrefab.transform.localPosition;
        currentHeadPrefab.transform.parent = null;
        headSlot = null;
        Avatar old = fighter.anim.avatar;
        fighter.anim.avatar = null;
        Destroy(currentBodyPrefab);
        yield return null;
        currentBodyPrefab = Instantiate(bodyPrefab, this.transform);
        yield return headSlot = GameObject.FindGameObjectWithTag("PlayerHead");
        Destroy(headSlot.transform.GetChild(0).gameObject);
        currentHeadPrefab.transform.parent = headSlot.transform;
        currentHeadPrefab.transform.localPosition = oldHeadTransform;
        Vector3 headRot = Vector3.zero;
        headRot.y = -90;
        headRot.z = -180;
        currentHeadPrefab.transform.localEulerAngles = headRot;
        yield return fighter.rightHandTransform = GameObject.FindGameObjectWithTag("PlayerRightHand").transform;
        yield return fighter.leftHandTransform = GameObject.FindGameObjectWithTag("PlayerLeftHand").transform;  
        fighter.anim.avatar = old;
        fighter.anim.Play("Locomotion");
        fighter.EquipWeapon(fighter.GetCurrentWeapon());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            StartCoroutine(EquipBody(testBodyPrefab));
        }
    }


}
