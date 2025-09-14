using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // ��� ������Ʈ (ī�޶� ���� ���)
    public Vector3 offset = new Vector3(0, 3, -5); // ��а� ī�޶� ������ �Ÿ� �� ��ġ
    public float followSpeed = 10.0f; // ī�޶� ���󰡴� �ӵ�
    public float rotationSpeed = 5.0f; // ī�޶� ȸ���ϴ� �ӵ�
    public float minDistance = 0.1f; // ��а� ī�޶� ������ �ּ� �Ÿ�

    void LateUpdate()
    {
        if (target == null) return;

        // ����� ���� ��ġ�� �������� ī�޶��� ���ϴ� ��ġ ���
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);

        // ��ֹ� ������ ���� Raycast
        RaycastHit hit;
        if (Physics.Raycast(target.position, (desiredPosition - target.position).normalized, out hit, offset.magnitude))
        {
            // ��ֹ��� �����Ǹ� ī�޶� ��ġ�� ��ֹ� �������� ����
            desiredPosition = target.position + (hit.point - target.position).normalized * (hit.distance - minDistance);
        }

        // ī�޶� ��ġ�� �ε巴�� �̵�
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // ī�޶� ����� �ٶ󺸵��� ȸ��
       Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
