using UnityEngine;

[System.Serializable]
public class Item
{
    public int Id;
    public string Name;
    public string Description;
    public string IconPath;
    public int Number;
    public int Price;
    public int MaxNub;

    [System.NonSerialized]
    public Sprite Icon;

    public Item() { }

    public Item(Item item)
    {
        this.Id = item.Id;
        this.Name = item.Name;
        this.Description = item.Description;
        this.IconPath = item.IconPath;
        this.Number = item.Number;
        this.Price = item.Price;
        //this.Icon = item.Icon;  // 确保复制时也包含图标
        this.MaxNub = item.MaxNub;
    }

    public Item(int id, string name, string description, string iconPath, int number, int price, int maxNub)
    {
        this.Id = id;
        this.Name = name;
        this.Description = description;
        this.IconPath = iconPath;
        this.Number = number;
        this.Price = price;
        this.Icon = Resources.Load<Sprite>(iconPath); // 加载图标
        this.MaxNub = maxNub;
    }

    public bool isUseItem(int amount)
    {
        if (Number > 0 && Number >= amount)
        {
            Number -= amount;
            return true;
        }
        else
        {
            Debug.Log("Not enough items to use");
        }
        return false;
    }

    public void UseItem(int amount)
    {
        isUseItem(amount);
    }

    public Item Clone()
    {
        string json = JsonUtility.ToJson(this);
        Item clone = JsonUtility.FromJson<Item>(json);
        clone.Icon = this.Icon;
        return clone;
    }

}
