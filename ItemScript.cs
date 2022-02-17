using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;
    public Item item;

    // Start is called before the first frame update
    void Start()
    {
        //create a new Item
        item = new Item();
        
        item.itemName = itemName;
        item.itemIcon = itemIcon;
    }

    public Item GetItem()
    {
        return item;
    }


}
