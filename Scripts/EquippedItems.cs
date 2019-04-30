using UnityEngine;
using System.Collections;

public class EquippedItems : MonoBehaviour
{
    [SerializeField]
    private Weapon weapon;
    public GameObject weaponGO;
    public Inventory inventory;
    public Transform weaponHoldPosition;
    public Weapon Weapon
    {
        get { return weapon; }
        set
        {
            if (weapon != null && weaponGO != null)
            {
                Destroy(weaponGO);
                Resources.UnloadUnusedAssets();
            }
            weapon = value;
            weaponGO = Instantiate(Resources.Load(value.slug) as GameObject);
            weaponGO.transform.parent = weaponHoldPosition;
            weaponGO.transform.localPosition = Vector3.zero;
            weaponGO.transform.localRotation = Quaternion.identity;
            WeaponDamagePhysics_Placeholder weaponDamage = weaponGO.GetComponentInChildren<WeaponDamagePhysics_Placeholder>();
            weaponDamage.weapon = weapon;
        }
    }
	// Use this for initialization
	void Start ()
    {
        if (inventory == null)
            inventory = GetComponent<Inventory>();

	}
	

    public void Equip(Item _item)
    {
        if (_item is Weapon)
            Weapon = _item as Weapon;
    }
}
