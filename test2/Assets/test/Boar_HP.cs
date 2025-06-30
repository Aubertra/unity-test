using UnityEngine;
using UnityEngine.UI;

public class Boar_HP : MonoBehaviour
{
    public Image fillImage;
    public Transform target; // Ҫ�����Ŀ��
    public Vector3 offset = new Vector3(0, 2f, 0); // ��Ŀ��ͷ��

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.LookAt(Camera.main.transform); // ʼ���������
        }
    }

    public void UpdateHealth(float current, float max)
    {
        fillImage.fillAmount = Mathf.Clamp01(current / max);
    }
}
