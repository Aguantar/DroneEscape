using UnityEngine;
using System.Collections;

public class DroneCollision : MonoBehaviour
{
    public HealthManager healthManager; // HealthManager 연결
    public DroneController droneController; // DroneController 연결
    public float collisionCooldown = 0.7f; // 충돌 쿨다운 시간
    public float knockbackForce = 10.0f; // 밀려나는 힘
    public float shakeDuration = 0.5f; // 카메라 흔들림 지속 시간
    public float shakeMagnitude = 0.7f; // 카메라 흔들림 강도

    public AudioClip obstacleSound; // 장애물 충돌 사운드
    public AudioClip monsterSound; // 몬스터 충돌 사운드
    public AudioClip itemPickupSound; // 아이템 수집 사운드
    private AudioSource audioSource; // 오디오 소스

    private bool canTakeDamage = true; // 충돌 감지 상태
    private bool isDamageImmune = false; // 모든 충돌 데미지 무효화 상태
    private Rigidbody rb; // 드론의 Rigidbody
    private CameraShake cameraShake; // CameraShake 스크립트

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody 초기화
        cameraShake = Camera.main.GetComponent<CameraShake>(); // Main Camera의 CameraShake 연결
        audioSource = GetComponent<AudioSource>(); // AudioSource 초기화

        if (cameraShake == null)
        {
            Debug.LogError("CameraShake script not found on Main Camera.");
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on this object.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDamageImmune) // 데미지 무효화 상태라면 충돌 무시
        {
            Debug.Log("Collision ignored due to immunity: " + collision.gameObject.tag);
            return;
        }

        if (collision.gameObject.CompareTag("Obstacle") && canTakeDamage)
        {
            Debug.Log("Collided with Obstacle");
            healthManager.DecreaseHealth(); // 체력 감소
            PlaySound(obstacleSound); // 사운드 재생
            ApplyKnockback(collision); // 밀려나는 힘 적용
            StartCoroutine(CollisionCooldown()); // 쿨다운 타이머
            StartCameraShake(); // 카메라 흔들림 시작
        }

        if (collision.gameObject.CompareTag("Monster") && canTakeDamage)
        {
            Debug.Log("Collided with Monster");
            healthManager.DecreaseHealth(); // 체력 감소
            PlaySound(monsterSound); // 사운드 재생
            ApplyKnockback(collision); // 밀려나는 힘 적용
            StartCoroutine(CollisionCooldown()); // 쿨다운 타이머
            StartCameraShake(); // 카메라 흔들림 시작
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDamageImmune) // 데미지 무효화 상태라면 충돌 무시
        {
            Debug.Log("Trigger collision ignored due to immunity: " + other.gameObject.tag);
            return;
        }

        if (other.CompareTag("HealthItem")) // HealthItem 태그 확인
        {
            healthManager.IncreaseHealth(); // 체력 회복
            PlaySound(itemPickupSound); // 아이템 수집 사운드 재생
            Destroy(other.gameObject); // 아이템 제거
        }

        if (other.CompareTag("SpeedBoostItem")) // SpeedBoostItem 태그 확인
        {
            droneController.ApplySpeedBoost(5f, 2f); // 5초 동안 속도 2배 증가
            PlaySound(itemPickupSound); // 아이템 수집 사운드 재생
            Destroy(other.gameObject); // 아이템 제거
        }

        if (other.CompareTag("SizeChangeItem")) // SizeChangeItem 태그 확인
        {
            droneController.ApplyRandomSizeChange(10f); // 10초 동안 랜덤 크기 변경
            PlaySound(itemPickupSound); // 아이템 수집 사운드 재생
            Destroy(other.gameObject); // 아이템 제거
        }
    }

    private void ApplyKnockback(Collision collision)
    {
        Vector3 collisionNormal = collision.contacts[0].normal; // 충돌 방향 벡터
        Vector3 knockbackDirection = -collisionNormal.normalized; // 역방향으로 계산
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse); // 힘 적용
    }

    private IEnumerator CollisionCooldown()
    {
        canTakeDamage = false; // 충돌 감지 비활성화
        yield return new WaitForSeconds(collisionCooldown); // 쿨다운 시간 대기
        canTakeDamage = true; // 충돌 감지 활성화
    }

    private void StartCameraShake()
    {
        if (cameraShake != null)
        {
            StartCoroutine(cameraShake.Shake(shakeDuration, shakeMagnitude)); // 카메라 흔들림 시작
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // 지정된 사운드 재생
        }
    }

    /// <summary>
    /// 모든 충돌 데미지를 일정 시간 동안 무효화합니다.
    /// </summary>
    /// <param name="duration">데미지 무효화 지속 시간</param>
    public void ApplyDamageImmunity(float duration)
    {
        if (isDamageImmune) return; // 이미 면역 상태라면 무시

        isDamageImmune = true; // 면역 활성화
        Debug.Log("Damage immunity activated for " + duration + " seconds.");
        StartCoroutine(DamageImmunityCooldown(duration));
    }

    private IEnumerator DamageImmunityCooldown(float duration)
    {
        yield return new WaitForSeconds(duration); // 면역 지속 시간 대기
        isDamageImmune = false; // 면역 해제
        Debug.Log("Damage immunity deactivated.");
    }

    /// <summary>
    /// 몬스터 공격에 의해 체력을 감소시킵니다.
    /// </summary>
    public void TakeDamage()
    {
        if (!canTakeDamage) return; // 쿨다운 중이면 무시

        canTakeDamage = false;
        healthManager.DecreaseHealth(); // 체력 감소
        Debug.Log("Drone took damage from Monster!");

        // 쿨다운 후 다시 데미지 감지 가능
        Invoke(nameof(ResetDamageCooldown), collisionCooldown);
    }

    private void ResetDamageCooldown()
    {
        canTakeDamage = true; // 데미지 감지 가능 상태로 복원
    }
}
