using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform itemGrid;

    public void updateUI(List<Item> items)
    {
        if (itemGrid)
        {
            foreach (Transform item in itemGrid)
            {
                Destroy(item.gameObject);
            }

            foreach (Item item in items)
            {
                GameObject itemobj = Instantiate(itemPrefab, itemGrid);
                InitItemObj(item, itemobj);
            }
        }
    }

    public void InitItemObj(Item item, GameObject gameObject)
    {
        Transform iteminfo;
        print(item.Number+":"+item.IconPath);
        if (gameObject.transform.GetChild(0))
            iteminfo = gameObject.transform.GetChild(0);
        else
        {
            print("iteminfo is null");
            return;
        }
        //初始化数量
        if (iteminfo.childCount >= 2)
            iteminfo.GetChild(1).GetComponent<Text>().text = item.Number.ToString();
        //初始化图
        if (iteminfo.GetChild(0).GetComponent<Image>() != null)
            iteminfo.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(item.IconPath);

        if (gameObject.GetComponentInChildren<KnapsackContentCell>())
            gameObject.GetComponentInChildren<KnapsackContentCell>().setItem(item);

        if (gameObject.GetComponentInChildren<ShoppingMallContentCell>())
            gameObject.GetComponentInChildren<ShoppingMallContentCell>().setItem(item);
    }
}
