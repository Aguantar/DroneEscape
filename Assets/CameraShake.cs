using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition; // 카메라의 원래 위치 저장
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // 화면만 흔들리도록 위치만 변경
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        // 원래 위치로 복구
        transform.localPosition = originalPosition;
    }
}
