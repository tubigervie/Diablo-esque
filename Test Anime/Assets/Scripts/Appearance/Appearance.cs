using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appearance : MonoBehaviour
{
    const string skin = "Skin";
    const string headPiece = "Head";
    const string hair = "Hair";

    GameObject headSlot;
    [SerializeField] GameObject currentBodyPrefab = null;
    [SerializeField] GameObject currentHeadPrefab = null;
    [SerializeField] GameObject hairPrefab = null;

    Material skinMat;

    public bool isMale;

    Fighter fighter;

    private void Awake()
    {
    }

    private void Start()
    {
        if (!currentHeadPrefab || !currentBodyPrefab)
        {
            Init();
            fighter = GetComponent<Fighter>();
            fighter.anim = GetComponent<Animator>();
        }
    }

    private void Init()
    {
        headSlot = GameObject.FindGameObjectWithTag("PlayerHead");
        currentBodyPrefab = this.transform.GetChild(0).gameObject;
        GameObject skinGO = currentBodyPrefab.transform.Find(skin).gameObject;
        skinMat = skinGO.GetComponent<SkinnedMeshRenderer>().material;
        currentHeadPrefab = headSlot.transform.GetChild(0).gameObject;
        hairPrefab = currentHeadPrefab.transform.Find(hair).gameObject;
    }

    public IEnumerator EquipBody(GameObject bodyPrefab)
    {
        if(!currentHeadPrefab || !currentBodyPrefab)
        {
            Init();
            fighter = GetComponent<Fighter>();
            fighter.anim = GetComponent<Animator>();
        }
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

    public void EquipHead(GameObject headPrefab, bool enableHair)
    {
        if (!currentHeadPrefab || !currentBodyPrefab)
        {
            Init();
            fighter = GetComponent<Fighter>();
            fighter.anim = GetComponent<Animator>();
        }
        DestroyOldHead();
        hairPrefab.SetActive(enableHair);
        var newHead = Instantiate(headPrefab, currentHeadPrefab.transform);
        newHead.gameObject.name = headPiece;
        newHead.transform.localPosition = Vector3.zero;
        newHead.transform.localEulerAngles = Vector3.zero;
    }

    void DestroyOldHead()
    {
        Transform oldHead = currentHeadPrefab.transform.Find(headPiece);
        if (oldHead == null) return;
        oldHead.name = "DESTROYING";
        Destroy(oldHead.gameObject);
    }

    public void RemoveHead()
    {
        DestroyOldHead();
        hairPrefab.SetActive(true);
    }
}
