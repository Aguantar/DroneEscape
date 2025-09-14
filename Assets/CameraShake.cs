using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition; // ī�޶��� ���� ��ġ ����
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // ȭ�鸸 ��鸮���� ��ġ�� ����
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        // ���� ��ġ�� ����
        transform.localPosition = originalPosition;
    }
}
