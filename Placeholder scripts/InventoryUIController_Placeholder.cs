using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryUIController_Placeholder : MonoBehaviour
{
    public GameObject rootObj;
    public GameObject UIItemPrefab;
    public Inventory inv;
    public PlayerMovementFirstPerson playerController;
    public List<MonoBehaviour> disableOnOpen = new List<MonoBehaviour>();

    bool isOpen = false;
    // Use this for initialization


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            ToggleOpen();
        }
    }

    public void Open()
    {
        rootObj.SetActive(true);
        isOpen = true;
        foreach (Item item in inv.inventory)
        {
            GameObject buttonObj = GameObject.Instantiate(UIItemPrefab);
            buttonObj.transform.parent = rootObj.transform;
            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(() => inv.UseItem(item));
            Text text = buttonObj.GetComponentInChildren<Text>();
            text.text = item.itemName;
        }
        foreach (MonoBehaviour mb in disableOnOpen)
        {
            mb.enabled = false;
        }
        playerController.playerControlled = false;
    }

    public void Close()
    {
        rootObj.SetActive(false);
        isOpen = false;
        foreach (Transform button in rootObj.GetComponentInChildren<Transform>())
        {
            Destroy(button.gameObject);
        }
        foreach (MonoBehaviour mb in disableOnOpen)
        {
            mb.enabled = true;
        }
        playerController.playerControlled = true;

    }

    public void ToggleOpen()
    {
        if (isOpen)
            Close();
        else
            Open();
    }
}
