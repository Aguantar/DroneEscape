using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 드론 오브젝트 (카메라가 따라갈 대상)
    public Vector3 offset = new Vector3(0, 3, -5); // 드론과 카메라 사이의 거리 및 위치
    public float followSpeed = 10.0f; // 카메라가 따라가는 속도
    public float rotationSpeed = 5.0f; // 카메라가 회전하는 속도
    public float minDistance = 0.1f; // 드론과 카메라 사이의 최소 거리

    void LateUpdate()
    {
        if (target == null) return;

        // 드론의 뒤쪽 위치를 기준으로 카메라의 원하는 위치 계산
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);

        // 장애물 감지를 위한 Raycast
        RaycastHit hit;
        if (Physics.Raycast(target.position, (desiredPosition - target.position).normalized, out hit, offset.magnitude))
        {
            // 장애물이 감지되면 카메라 위치를 장애물 앞쪽으로 조정
            desiredPosition = target.position + (hit.point - target.position).normalized * (hit.distance - minDistance);
        }

        // 카메라 위치를 부드럽게 이동
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // 카메라가 드론을 바라보도록 회전
       Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
