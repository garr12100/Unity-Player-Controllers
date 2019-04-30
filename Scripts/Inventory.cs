using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{

    public EquippedItems equippedItems;
    public List<Item> inventory = new List<Item>();

	// Use this for initialization
	void Start ()
    {
        AddItem(2);
        AddItem(3);

    }

    public void AddItem(int id)
    {
        Item item = ItemDatabase.instance.Fetch(id);
        inventory.Add(item);
        if (item is Weapon)
        {
            Weapon weapon = item as Weapon;
            Debug.Log(weapon.damage);
        }

    }

    public void AddItem(Item item)
    {
        AddItem(item.id);
    }

    public void UseItem(Item item)
    {
        if (item is Weapon)
            equippedItems.Equip(item);
    }

}
