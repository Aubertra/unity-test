using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    public PlayerController player;
    //背包/商城/设置界面预制体
    public GameObject KnapsackPrefab, ShoppingMallPrefab, SetPrefab;
    //界面中的各个元素
    private GameObject HP_Object, ArmorPoint_Object, MenPhoto_Object,
                      Knapsack_Object, ShoppingMall_Object, Set_object;
    //Canvas
    GameObject Canvas;
    //Inventory脚本
    Inventory MyInventory;
    //生命值，护甲值
    Slider HP, ArmorPoint;
    //头像
    Image MenPhoto;

    private void Awake()
    {
        //获取Canvas
        Canvas = GameObject.Find("Canvas");
        //获取Inventory脚本
        MyInventory = FindObjectOfType<Inventory>();
        //获取所有子物体transform
        Transform[] transforms = GetComponentsInChildren<Transform>();
        //构造对应的gameObeject数组
        GameObject[] gameObjects = new GameObject[transforms.Length - 1];

        //获取每个transform对应的gameObject
        foreach(Transform transform in transforms)
        {
            switch (transform.gameObject.name)
            {
                case "HP":
                    HP_Object = transform.gameObject;
                    break;
                case "ArmorPoint":
                    ArmorPoint_Object = transform.gameObject;
                    break;
                case "MenPhoto":
                    MenPhoto_Object = transform.gameObject;
                    break;
                case "Knapsack":
                    Knapsack_Object = transform.gameObject;
                    break;
                case "ShoppingMall":
                    ShoppingMall_Object = transform.gameObject;
                    break;
                case "Setting":
                    Set_object = transform.gameObject;
                    break;
            }
        }
        //生命值
        HP = HP_Object.GetComponent<Slider>();
        //护甲值
        ArmorPoint = ArmorPoint_Object.GetComponent<Slider>();
        //头像
        MenPhoto = MenPhoto_Object.GetComponent<Image>();
        //背包按钮
        Knapsack_Object.GetComponent<Button>().onClick.AddListener(OpenKnapsack);
        //商城按钮
        ShoppingMall_Object.GetComponent<Button>().onClick.AddListener(OpenShoppingMall);
        //设置按钮
        Set_object.GetComponent<Button>().onClick.AddListener(OpenSet);
    }

    private void Update()
    {
        HP.value = player.HP/10;
        ArmorPoint.value = 0.5f;
        MenPhoto.color = Color.black;
    }

    void OpenKnapsack()
    {
        GameObject instance = Instantiate(KnapsackPrefab,Vector3.zero,Quaternion.identity);
        instance.transform.SetParent(Canvas.transform, false);
        instance.transform.localScale = Vector3.one;

        MyInventory.SetExistInventoryUITrue();
    }

    void OpenShoppingMall()
    {
        // 创建商城实例
        GameObject instance = Instantiate(ShoppingMallPrefab, Vector3.zero, Quaternion.identity);

        UIPullAll(instance.transform, Canvas.transform);

        MyInventory.SetExistInventoryUITrue();
    }

    void OpenSet()
    {
        //创建设置面板
        GameObject instance = Instantiate(SetPrefab, Vector3.zero, Quaternion.identity);

        UIPullAll(instance.transform, Canvas.transform);

    }

    void UIPullAll(Transform UI,Transform Parent)
    {
        // 将实例作为 Canvas 的子物体，并保持其原始属性
        UI.SetParent(Parent, false);

        // 获取 RectTransform 组件
        RectTransform rectTransform = UI.GetComponent<RectTransform>();

        // 设置锚点，使其铺满整个父级 Canvas
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);

        // 设置偏移为 0，确保没有额外的偏移
        rectTransform.offsetMin = Vector2.zero; // 左下角偏移
        rectTransform.offsetMax = Vector2.zero; // 右上角偏移

        // 设置缩放为 (1, 1, 1)
        rectTransform.localScale = Vector3.one;
    }

    //设置最大生命值
    public void setMaxHP(int HP)
    {
        this.HP.maxValue = HP;
    }
    //设置最大护甲值
    public void setMaxArmorPoint(int ArmorPoint)
    {
        this.ArmorPoint.maxValue = ArmorPoint;
    }
}
