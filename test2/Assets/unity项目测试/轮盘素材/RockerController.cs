using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class RockerController : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    public static RockerController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (!Input.GetMouseButton(0)) // 没有正在触摸或点击
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            Vector2 inputDir = new Vector2(h, v);
            if (inputDir.magnitude > 0.1f)
            {
                outPos = Vector2.ClampMagnitude(inputDir, 1f) * R;
                yaoGanPos.localPosition = outPos;
                yaoGanLight.transform.up = outPos.normalized;
            }
            else
            {
                outPos = Vector2.zero;
                yaoGanPos.localPosition = Vector2.zero;
                yaoGanLight.localRotation = Quaternion.identity;
            }
        }
#endif
    }




    public void OnDrag(PointerEventData eventData)
    {



        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            yaoGanBGPos, eventData.position, eventData.enterEventCamera, out outPos
            );
        float S = Vector2.Distance(Vector2.zero, outPos);
        if (S > R)
        {
            outPos = outPos.normalized * R;
        }

        yaoGanLight.transform.up = outPos.normalized;
        yaoGanPos.localPosition = outPos;

    }










    /*
    public void OnPointerDown(PointerEventData eventData)
    {
     
      
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform, eventData.pressPosition, eventData.enterEventCamera, out outPos
            );
       
        yaoGanBGPos.localPosition = outPos;
        outPos = Vector2.zero;
    
    }
    bool dragOver;
    public void OnEnable()
    {
        dragOver = true;
 
    }
    bool isSetactive;
    public void OnDisable()
    {

        outPos = Vector2.zero;
        yaoGanPos.localPosition = outPos;
      
    }
    */
    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform, eventData.pressPosition, eventData.enterEventCamera, out outPos
        );

        yaoGanBGPos.localPosition = outPos;

        // 初始触点即视为一次拖动，更新摇杆方向
        OnDrag(eventData);
    }


    public void OnPointerUp(PointerEventData eventData)
    {
    
       outPos = Vector2.zero;
        yaoGanPos.localPosition = outPos;
        if (Screen.width > Screen.height)
        {
            yaoGanBGPos.anchoredPosition = new Vector2(290, 215);
        }
        else
        {
            yaoGanBGPos.anchoredPosition = new Vector2(547, 244);
        }
        yaoGanLight.localRotation = Quaternion.identity;
    }
    private RectTransform yaoGanPos;
    private RectTransform yaoGanBGPos;
    private RectTransform yaoGanLight;
    public float R;//半径
    public Vector2 outPos;
    // Start is called before the first frame update
    void Start()
    {
    
        yaoGanPos = transform.GetChild(0).GetChild(0) as RectTransform;
        yaoGanBGPos = transform.GetChild(0) as RectTransform;
        yaoGanLight= transform.GetChild(0).GetChild(1) as RectTransform;
    }

  
}
