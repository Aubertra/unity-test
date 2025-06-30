using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnapsackContentCell : MonoBehaviour
{
    GameObject itemDetailPrefab;
    Canvas canvas;
    GameObject Prefab;
    Item item;

    private void Awake()
    {
        itemDetailPrefab = Resources.Load<GameObject>("PreFabs/Knapsack/ItemDetail");
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    

    public void setItem(Item item)
    {
        this.item = item;
    }

    public void ItemDetaills()
    {
        //获取鼠标位置,API提供的是二维坐标
        Vector3 mousePosition = Input.mousePosition;
        // 转换屏幕坐标到 UI 坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            mousePosition,
            canvas.worldCamera,
            out Vector2 localPoint
        );
        //Quaternion.identity表明默认旋转,即没有旋转
        Prefab = Instantiate(itemDetailPrefab, canvas.transform);
        //传入物品信息
        if (item != null && Prefab.GetComponentInChildren<ItemDetail>())
            Prefab.GetComponentInChildren<ItemDetail>().item = item;
        //初始化位置
        RectTransform rectTransform = Prefab.GetComponent<RectTransform>();
        localPoint = new Vector2(localPoint.x - 280, localPoint.y - 100);
        AdjustPositionWithinBounds(rectTransform, localPoint);
        //调整大小
        AdjustSize(Prefab);
    }

    // 调整位置，确保弹窗不超出屏幕边界
    private void AdjustPositionWithinBounds(RectTransform rectTransform, Vector2 localPoint)
    {
        // 获取Canvas的宽高
        float canvasWidth = canvas.GetComponent<RectTransform>().rect.width;
        float canvasHeight = canvas.GetComponent<RectTransform>().rect.height;

        // 获取弹窗的宽高
        float popupWidth = rectTransform.rect.width;
        float popupHeight = rectTransform.rect.height;

        // 调整位置，确保弹窗不会超出屏幕右侧或底部
        float adjustedX = localPoint.x;
        float adjustedY = localPoint.y;

        if (localPoint.x + popupWidth > canvasWidth)
        {
            adjustedX = canvasWidth - popupWidth;
        }

        if (localPoint.y + popupHeight > canvasHeight)
        {
            adjustedY = canvasHeight - popupHeight;
        }

        // 应用调整后的位置
        rectTransform.localPosition = new Vector3(adjustedX, adjustedY, 0);
    }


    public void AdjustSize(GameObject gameObject)
    {
        gameObject.transform.localScale = Vector3.one;
    }
}