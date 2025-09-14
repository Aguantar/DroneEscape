using UnityEngine;

public class CameraCollisionSound : MonoBehaviour
{
    // AudioSource�� ������ ����
    public AudioSource audioSource;

    void OnCollisionEnter(Collision collision)
    {
        // �浹�� ������Ʈ�� Obstacle �±׸� ���� ��쿡�� ���� ���
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }
}
