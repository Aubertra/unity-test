using UnityEngine;

public class Prop : MonoBehaviour
{
    public int propId;
    public string propName;
    public string propDescription;
    public string propIconPath;
    public int propNumber;
    public int propPrice;
    public int propMaxNub;

    private Item item;

    private void Awake()
    {
        InitializeProp();
    }

    private void InitializeProp()
    {
        // 创建物品并初始化
        item = new Item(propId, propName, propDescription, propIconPath, propNumber, propPrice, propMaxNub);
        item.Icon = Resources.Load<Sprite>(propIconPath);
    }

    // 使用道具，并同步到 Item 中的数量
    public bool isUseProp(int number)
    {
        //下面为道具功效

        return item.isUseItem(number);
    }

    public void UseProp(int number)
    {
        isUseProp(number);
    }

    // 获取对应的 Item 对象
    public Item GetItem()
    {
        return new Item(item); // 返回 Item 的副本，防止外部修改原始对象
    }
}

