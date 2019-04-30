using UnityEngine;
using System.Collections;


[System.Serializable]
public class Item
{
    public string itemName;
    public int value;
    public string description;
    public string slug;
    public int id;
    public bool sellable;
    public bool stackable;

    public Item()
    {
        id = -1;
    }

    public Item(int _id, string _itemName, string _description, string _slug, int _value, bool _sellable, bool _stackable)
    {
        id = _id;
        itemName = _itemName;
        description = _description;
        slug = _slug;
        value = _value;
        sellable = _sellable;
        stackable = _stackable;
    }
}

[System.Serializable]
public class Weapon : Item
{
    public float damage;
    public float damageRange;
    public float criticalChance;
    public float criticalBoost;
    public WeaponType weaponType;

    public Weapon(int _id, string _itemName, string _description, string _slug, int _value, bool _sellable,
        bool _stackable, float _damage, float _damageRange, float _criticalChance, float _criticalBoost, WeaponType _weaponType)
        : base(_id, _itemName, _description, _slug, _value, _sellable, _stackable)
    {
        damage = _damage;
        damageRange = _damageRange;
        criticalChance = _criticalChance;
        criticalBoost = _criticalBoost;
        weaponType = _weaponType;
    }

    public Weapon(Item item, float _damage, float _damageRange, float _criticalChance, float _criticalBoost, WeaponType _weaponType)
    {
        id = item.id;
        itemName = item.itemName;
        description = item.description;
        slug = item.slug;
        value = item.value;
        sellable = item.sellable;
        stackable = item.stackable;
        damage = _damage;
        damageRange = _damageRange;
        criticalChance = _criticalChance;
        criticalBoost = _criticalBoost;
        weaponType = _weaponType;
    }
}

public enum WeaponType
{
    None,
    Sword,
    Axe,
    Blunt
}
