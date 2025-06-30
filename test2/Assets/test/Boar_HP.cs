using UnityEngine;
using UnityEngine.UI;

public class Boar_HP : MonoBehaviour
{
    public Image fillImage;
    public Transform target; // 要跟随的目标
    public Vector3 offset = new Vector3(0, 2f, 0); // 在目标头顶

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.LookAt(Camera.main.transform); // 始终面向相机
        }
    }

    public void UpdateHealth(float current, float max)
    {
        fillImage.fillAmount = Mathf.Clamp01(current / max);
    }
}
