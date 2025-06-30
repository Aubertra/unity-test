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
        // ������Ʒ����ʼ��
        item = new Item(propId, propName, propDescription, propIconPath, propNumber, propPrice, propMaxNub);
        item.Icon = Resources.Load<Sprite>(propIconPath);
    }

    // ʹ�õ��ߣ���ͬ���� Item �е�����
    public bool isUseProp(int number)
    {
        //����Ϊ���߹�Ч

        return item.isUseItem(number);
    }

    public void UseProp(int number)
    {
        isUseProp(number);
    }

    // ��ȡ��Ӧ�� Item ����
    public Item GetItem()
    {
        return new Item(item); // ���� Item �ĸ�������ֹ�ⲿ�޸�ԭʼ����
    }
}

