using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;

public class ItemDatabase : MonoBehaviour
{

    public static ItemDatabase instance;
    
    private List<Item> database = new List<Item>();
    private JsonData itemData;

    void Awake()
    {

        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of an ItemDatabase.
            Destroy(gameObject);
    }

    void Start()
    {
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
        ConstructItemDatabase();
    }

    void ConstructItemDatabase()
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            Item newItem = new Item((int)itemData[i]["id"], itemData[i]["itemName"].ToString(), 
                itemData[i]["description"].ToString(), itemData[i]["slug"].ToString(), (int)itemData[i]["value"], 
                bool.Parse(itemData[i]["sellable"].ToString()), bool.Parse(itemData[i]["stackable"].ToString()));

            switch (itemData[i]["itemType"].ToString())
            {
                case "item":
                    database.Add(newItem);
                    break;

                case "weapon":
                    database.Add(new Weapon(newItem, 
                        float.Parse(itemData[i]["damage"].ToString(), System.Globalization.CultureInfo.InvariantCulture), 
                        float.Parse(itemData[i]["damageRange"].ToString(), System.Globalization.CultureInfo.InvariantCulture), 
                        float.Parse(itemData[i]["criticalChance"].ToString(), System.Globalization.CultureInfo.InvariantCulture), 
                        float.Parse(itemData[i]["criticalBoost"].ToString(), System.Globalization.CultureInfo.InvariantCulture), 
                        (WeaponType)System.Enum.Parse(typeof(WeaponType), itemData[i]["weaponType"].ToString())));
                    break;

                case "armor":
                    break;
            }
        }
    }

    public Item Fetch(int id)
    {
        foreach (Item item in database)
        {
            if (item.id == id)
            {
                return item;
            }
        }
        return null;
    }

    public Item Fetch(Item item)
    {
        return Fetch(item.id);
    }

}
