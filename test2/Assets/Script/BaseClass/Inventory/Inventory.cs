using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Item> knapsackItems, shoppingMallItems;
    private int knapsack_capacity, shoppingMall_capacity;
    private string knapsackFilePath, shoppingMailFilePath, ContentType;
    private InventoryUI inventoryUI;
    private bool existInventoryUI;

    private void Awake()
    {
        knapsackItems = new List<Item>();
        shoppingMallItems = new List<Item>();

        existInventoryUI = false;
        if (FindObjectOfType<InventoryUI>())
        {
            inventoryUI = FindObjectOfType<InventoryUI>();
            existInventoryUI = true;
        }

        knapsackFilePath = InfoFilePath.KnapsackFilePath_Test;
        shoppingMailFilePath = InfoFilePath.ShoppingMallFilePath_Test;

        LoadItemInfo();
        UpdateUI();
        print("Inventory is ready!");
    }

    public List<Item> getKnapsackItems()
    {
        return this.knapsackItems;
    }

    public List<Item> getShoppingMallItems()
    {
        return this.shoppingMallItems;
    }

    private void OnDestroy()
    {
        SaveItemInfo();
    }

    // 返回添加是否成功
    public bool isAddItem(Item item, int add_nub = -1)
    {
        int amountToAdd = (add_nub == -1) ? item.Number : add_nub;

        // 找可堆叠物品槽
        Item existItem = knapsackItems.Find(i => i.Id == item.Id && i.Number < i.MaxNub);
        if (existItem != null)
        {
            int spaceLeft = existItem.MaxNub - existItem.Number;
            int toStack = Mathf.Min(spaceLeft, amountToAdd);
            existItem.Number += toStack;
            amountToAdd -= toStack;
        }

        // 剩下的以新格子存放
        while (amountToAdd > 0)
        {
            if (knapsack_capacity <= 0)
            {
                print("knapsackItems is full!");
                return false; // 返回失败
            }

            int toAdd = Mathf.Min(item.MaxNub, amountToAdd);
            Item newItem = item.Clone();
            newItem.Number = toAdd;
            knapsackItems.Add(newItem);
            amountToAdd -= toAdd;
            knapsack_capacity--;
        }

        UpdateUI();
        return true; // 返回成功
    }

    public void AddItem(Item item, int add_nub = -1)
    {
        isAddItem(item, add_nub);
    }

    // 删除物品，返回删除是否成功
    public bool isRemoveItem(Item item, int deleteNumber = -1)
    {
        int amountToRemove = (deleteNumber == -1) ? item.Number : deleteNumber;

        // 从后向前遍历，确保后添加的格子先被消耗
        for (int i = knapsackItems.Count - 1; i >= 0 && amountToRemove > 0; i--)
        {
            Item current = knapsackItems[i];

            if (current.Id != item.Id) continue;

            int toRemove = Mathf.Min(current.Number, amountToRemove);
            bool success = current.isUseItem(toRemove);

            if (success)
            {
                amountToRemove -= toRemove;

                if (current.Number == 0)
                {
                    knapsackItems.RemoveAt(i);
                    knapsack_capacity++;
                }
            }
            else
            {
                Debug.LogWarning($"Failed to remove {toRemove} from slot {i}");
            }
        }

        if (amountToRemove > 0)
        {
            Debug.LogWarning($"Could not remove enough items. Remaining: {amountToRemove}");
            return false; // 返回失败
        }

        UpdateUI();
        return true; // 返回成功
    }

    public void RemoveItem(Item item, int deleteNumber = -1)
    {
        isRemoveItem(item, deleteNumber);
    }

    // 清空所有物品，返回是否成功
    public bool isRemoveAllItems()
    {
        knapsackItems.Clear();
        knapsack_capacity = InventoryData.DEFAULT_KNAPSACK_CAPACITY; // 恢复初始容量
        UpdateUI();
        return true; // 返回成功
    }

    public void RemoveAllItems()
    {
        isRemoveAllItems();
    }

    // 加载物品信息
    public void LoadItemInfo()
    {
        #region Knapsack
        if (File.Exists(knapsackFilePath))
        {
            string json = File.ReadAllText(knapsackFilePath);
            KnapsackItemList itemList = JsonUtility.FromJson<KnapsackItemList>(json);

            foreach (Item item in itemList.knapsackItems)
            {
                item.Icon = Resources.Load<Sprite>(item.IconPath);
            }
            knapsackItems = itemList.knapsackItems;
            knapsack_capacity = itemList.capacity;
        }
        else
        {
            print("knapsack file not exist");
            knapsack_capacity = InventoryData.DEFAULT_KNAPSACK_CAPACITY;
        }
        #endregion

        #region ShoppingMall
        if (File.Exists(shoppingMailFilePath))
        {
            string json = File.ReadAllText(shoppingMailFilePath);
            ShoppingMallItemList itemList = JsonUtility.FromJson<ShoppingMallItemList>(json);

            foreach (Item item in itemList.shoppingMallItems)
            {
                item.Icon = Resources.Load<Sprite>(item.IconPath);
            }
            shoppingMallItems = itemList.shoppingMallItems;
            shoppingMall_capacity = itemList.capacity;
        }
        else
        {
            print("shoppingMall file not exist");
            shoppingMall_capacity = InventoryData.DEFAULT_SHOPPINGMALL_CAPACITY;
        }
        #endregion
    }

    // 添加道具
    public bool isAddProp(Prop prop)
    {
        return isAddItem(prop.GetItem());
    }

    public void AddProp(Prop prop)
    {
        isAddProp(prop);
    }

    // 删除道具
    public bool isRemoveProp(Prop prop)
    {
        return isRemoveItem(prop.GetItem());
    }

    public void RemoveProp(Prop prop)
    {
        isRemoveProp(prop);
    }

    public void SetExistInventoryUITrue()
    {
        existInventoryUI = true;
        inventoryUI = FindObjectOfType<InventoryUI>();
        UpdateUI();
    }

    public void SetExistInventoryUIFalse()
    {
        existInventoryUI = false;
    }

    // 保存物品信息
    public void SaveItemInfo()
    {
        string knapstack_json = JsonUtility.ToJson(new KnapsackItemList(knapsackItems,knapsack_capacity), true),
               shoppingMail_json = JsonUtility.ToJson(new ShoppingMallItemList(shoppingMallItems,shoppingMall_capacity), true);

        File.WriteAllText(knapsackFilePath, knapstack_json);
        File.WriteAllText(shoppingMailFilePath, shoppingMail_json);
    }

    private void UpdateUI()
    {
        if (existInventoryUI && inventoryUI != null)
        {
            ContentType = inventoryUI.gameObject.name;
            print(ContentType);
            if (ContentType == "KnapsackContent")
                inventoryUI.updateUI(knapsackItems);
            if (ContentType == "ShoppingMallContent")
                inventoryUI.updateUI(shoppingMallItems);
        }
        else
        {
            existInventoryUI = false;
        }
    }

    [System.Serializable]
    public class KnapsackItemList
    {
        public List<Item> knapsackItems;
        public int capacity;

        public KnapsackItemList(List<Item> knapsackItems,int capacity)
        {
            this.knapsackItems = knapsackItems;
            this.capacity = capacity;
        }
    }

    [System.Serializable]
    public class ShoppingMallItemList
    {
        public List<Item> shoppingMallItems;
        public int capacity;

        public ShoppingMallItemList(List<Item> shoppingMallItems,int capacity = InventoryData.DEFAULT_SHOPPINGMALL_CAPACITY)
        {
            this.shoppingMallItems = shoppingMallItems;
            this.capacity = capacity;
        }
    }
}
