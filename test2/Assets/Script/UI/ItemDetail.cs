using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDetail : MonoBehaviour
{
    Image image;
    new Text name;
    Text number, description;
    Button useButton;
    InventoryUI inventoryUI;
    Inventory inventory;

    public Item item;
    bool first;

    private void Awake()
    {
        image = GetComponentsInChildren<Image>()[2];
        name = GetComponentsInChildren<Text>()[0];
        number = GetComponentsInChildren<Text>()[1];
        description = GetComponentsInChildren<Text>()[2];
        useButton = GetComponentInChildren<Button>();

        inventory = GameObject.FindObjectOfType<Inventory>();
        inventoryUI = GameObject.FindObjectOfType<InventoryUI>();
        first = true;
    }

    private void Update()
    {
        if (first && item != null)
        {
            image.sprite = item.Icon;
            name.text = "名称：" + item.Name;
            number.text = "数量：" + item.Number.ToString();
            description.text = item.Description;

            useButton.onClick.RemoveAllListeners();
            useButton.onClick.AddListener(useItem);

            first=!first;
        }

        if (Input.GetMouseButtonDown(0))
         {
            if (!IsPointerOverUIObject(this.gameObject))
             {
                print("There");
                Destroy(this.gameObject);
             }
         }
    }

    void useItem()
    {
        if(item.Number > 0)
        {
            inventory.RemoveItem(item,1);
            if (item.Number == 0) Destroy(gameObject);
            inventoryUI.updateUI(inventory.getKnapsackItems());
            number.text = "数量：" + item.Number.ToString();
        }
        else
        {
            print("物品不足");
        }
    }

    private bool IsPointerOverUIObject(GameObject uiObject)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject == uiObject || result.gameObject.transform.IsChildOf(uiObject.transform))
            {
                return true;
            }
        }
        return false;
    }
}
