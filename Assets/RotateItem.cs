using UnityEngine;

public class RotateItem : MonoBehaviour
{
    public float rotationSpeed = 50f; // 회전 속도

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime); // Y축 기준 회전
    }
}
