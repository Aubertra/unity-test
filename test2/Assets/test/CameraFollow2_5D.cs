using UnityEngine;

public class CameraFollow2_5D : MonoBehaviour
{
    public Transform target;          // ����Ŀ�꣨ͨ������ң�
    public Vector3 offset = new Vector3(0, 5, -10);  // �ӽ�ƫ������ZΪ����ʾ����ں��Ϸ���
    public float smoothSpeed = 5f;    // ƽ���ٶ�

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // �������ʼ�ճ������ǰ��
        transform.LookAt(target.position + Vector3.forward * 5f);
    }
}
