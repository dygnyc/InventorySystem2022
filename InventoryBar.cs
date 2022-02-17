using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryBar : MonoBehaviour
{
    GameObject[] itemSlot; //array of itemslots
    List<Item> inventoryList; //hold reference to inventory list
    Inventory inventoryScript; //hold reference to inventory script

    // Start is called before the first frame update
    void Start()
    {
        itemSlot = new GameObject[3]; //create empty array of 3 game objects
        inventoryList = new List<Item>();

        // the item slots will be children of the Inventory Bar
        itemSlot[0] = transform.GetChild(0).gameObject;
        itemSlot[1] = transform.GetChild(1).gameObject;
        itemSlot[2] = transform.GetChild(2).gameObject;

        inventoryScript = FindObjectOfType<Inventory>().GetComponent<Inventory>();
        // hold reference to use later in UpdateBar
    }

    //subscribe UpdateBar to OnInventoryChange event

    void OnEnable()
    {
        Inventory.OnInventoryChange += UpdateBar;
    }

    void OnDisable()
    {
        Inventory.OnInventoryChange -= UpdateBar;
    }


    void UpdateBar()
    {
        inventoryList = inventoryScript.GetInventoryList();
        int x = inventoryList.Count;
        int max = 3; //total slots available

        //deactivate unused slots
        if (x < max)
        {
            for (int i = x; i < max; i++)
            {
                itemSlot[i].SetActive(false);
            }
        }

        //activate used slots
        for (int i = 0; i < x; i++)
        {
            itemSlot[i].SetActive(true);
            itemSlot[i].transform.GetChild(2).gameObject.SetActive(true);
            itemSlot[i].transform.GetChild(2).GetComponent<Image>().sprite =
                inventoryList[i].itemIcon;
            itemSlot[i].transform.GetChild(3).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                inventoryList[i].stackSize.ToString();
        }
    }


}
