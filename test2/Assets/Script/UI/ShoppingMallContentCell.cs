using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingMallContentCell : MonoBehaviour
{
    Image icon;
    Text description, price;
    new Text name;
    Button buyButton;
    Inventory inventory;
    Item item;

    private void Awake()
    {
        inventory = GameObject.FindObjectOfType<Inventory>();
        icon = GameObject.Find("Icon").GetComponent<Image>();
        name = GameObject.Find("Name").GetComponent<Text>();
        description = GameObject.Find("Description").GetComponent<Text>();
        price = GameObject.Find("Number").GetComponent<Text>();
        buyButton = GameObject.Find("BuyButton").GetComponent<Button>();
    }

    public void setItem(Item item)
    {
        this.item = item;
    }

    public void UpdateGoodsDetail()
    {
        if (item != null)
        {
            icon.sprite = item.Icon;
            description.text = item.Description;
            price.text = item.Price.ToString();
            name.text = item.Name;

            //移除已有监听器，避免重复添加
            buyButton.onClick.RemoveAllListeners();
            //使用lambda表达式，使点击按钮时才调用事件
            buyButton.onClick.AddListener(()=>inventory.AddItem(item,1));
        }
    }
}
