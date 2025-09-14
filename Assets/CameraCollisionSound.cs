using UnityEngine;

public class CameraCollisionSound : MonoBehaviour
{
    // AudioSource를 저장할 변수
    public AudioSource audioSource;

    void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트가 Obstacle 태그를 가진 경우에만 사운드 재생
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }
}
