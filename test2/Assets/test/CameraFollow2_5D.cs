using UnityEngine;

public class CameraFollow2_5D : MonoBehaviour
{
    public Transform target;          // 跟随目标（通常是玩家）
    public Vector3 offset = new Vector3(0, 5, -10);  // 视角偏移量（Z为负表示相机在后上方）
    public float smoothSpeed = 5f;    // 平滑速度

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // 保持相机始终朝向玩家前方
        transform.LookAt(target.position + Vector3.forward * 5f);
    }
}
