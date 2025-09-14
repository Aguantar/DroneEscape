using UnityEngine;

public class MonsterChase : MonoBehaviour
{
    public Transform drone; // 드론 Transform
    public float moveSpeed = 160f; // 몬스터 이동 속도
    public float chaseRadius = 50f; // 드론을 추적하는 범위
    public float returnSpeed = 2f; // 원래 위치로 돌아가는 속도
    public float lostSightTime = 5f; // 시야에서 벗어난 시간
    public float attackRange = 2f; // 몬스터 공격 범위
    public float attackCooldown = 1.5f; // 몬스터 공격 쿨다운 시간

    private float timeOutOfSight = 0f; // 시야에서 벗어난 시간 누적
    private bool isChasing = false; // 추적 상태
    private bool canAttack = true; // 몬스터가 공격 가능한 상태
    private Vector3 originalPosition; // 몬스터의 원래 위치
    private DroneCollision droneCollision; // 드론 충돌 스크립트
    private Rigidbody rb; // 몬스터의 Rigidbody

    void Start()
    {
        // Rigidbody 초기화
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing on the Monster!");
        }
        else
        {
            rb.isKinematic = true; // Rigidbody를 물리적으로 움직이지 않도록 설정
        }

        // 몬스터의 초기 위치 저장
        originalPosition = transform.position;

        if (drone != null)
        {
            droneCollision = drone.GetComponent<DroneCollision>();
            if (droneCollision == null)
            {
                Debug.LogError("DroneCollision script not found on the drone!");
            }
        }
    }

    void Update()
    {
        if (drone == null) return;

        float distanceToDrone = Vector3.Distance(transform.position, drone.position);

        if (distanceToDrone <= chaseRadius)
        {
            // 드론이 시야에 들어왔을 때 추적
            isChasing = true;
            timeOutOfSight = 0f; // 시야 밖 시간 초기화
            ChaseDrone(); // 드론 추적

            // 드론이 공격 범위에 있을 때 공격
            if (distanceToDrone <= attackRange && canAttack)
            {
                AttackDrone();
            }
        }
        else if (isChasing)
        {
            // 시야에서 벗어난 시간 계산
            timeOutOfSight += Time.deltaTime;

            if (timeOutOfSight >= lostSightTime)
            {
                // 일정 시간 이상 시야에서 벗어났을 경우 추적 중단
                isChasing = false;
                if (droneCollision != null)
                {
                    droneCollision.ApplyDamageImmunity(7f); // 7초 동안 모든 충돌 데미지 무효화
                }
            }
        }

        if (!isChasing)
        {
            ReturnToOriginalPosition(); // 원래 위치로 돌아감
        }
    }

    void ChaseDrone()
    {
        if (rb == null) return;

        // 드론의 위치에서 y 좌표를 고정
        Vector3 targetPosition = new Vector3(drone.position.x, transform.position.y, drone.position.z);

        // 드론 방향으로 이동
        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;

        // Rigidbody를 사용해 이동
        rb.MovePosition(newPosition);

        // 드론 방향으로 회전
        transform.LookAt(new Vector3(drone.position.x, transform.position.y, drone.position.z));
    }

    void ReturnToOriginalPosition()
    {
        if (rb == null) return;

        // 원래 위치로 이동
        Vector3 direction = (originalPosition - transform.position).normalized;
        Vector3 newPosition = transform.position + direction * returnSpeed * Time.deltaTime;

        // Rigidbody를 사용해 이동
        rb.MovePosition(newPosition);

        // 몬스터가 원래 위치에 도달했는지 확인
        if (Vector3.Distance(transform.position, originalPosition) < 0.1f)
        {
            rb.MovePosition(originalPosition); // 위치 고정
        }
    }

    void AttackDrone()
    {
        if (droneCollision != null)
        {
            droneCollision.TakeDamage(); // 드론에 데미지 적용
            Debug.Log("Monster attacked the drone!");
        }

        canAttack = false; // 공격 쿨다운 시작
        Invoke(nameof(ResetAttackCooldown), attackCooldown);
    }

    void ResetAttackCooldown()
    {
        canAttack = true; // 공격 가능 상태로 복원
    }

    void OnDrawGizmosSelected()
    {
        // 추적 범위 시각화
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);

        // 공격 범위 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
