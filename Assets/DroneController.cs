using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;
using UnityEngine.UIElements;

public class DroneController : MonoBehaviour
{
    public float speed = 10.0f; // 기본 이동 속도
    public float sensitivity = 15.0f; // 마우스 감도
    private float originalSpeed; // 원래 속도 저장
    private Vector3 originalScale; // 원래 크기 저장
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    private Rigidbody rb;
    private bool isSpeedBoosted = false; // 속도 증가 상태 확인
    private bool isSizeChanged = false; // 크기 변경 상태 확인

    private UnityEngine.InputSystem.InputDevice droneController;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.drag = 5.0f; // 움직임 감속
        rb.angularDrag = 10.0f; // 회전 감속
        originalSpeed = speed; // 원래 속도 저장
        originalScale = transform.localScale; // 원래 크기 저장
        StartCoroutine(InitializeController());

    }
    private IEnumerator InitializeController()
    {
        // 장치가 시스템에 추가될 때까지 대기
        while ((droneController = InputSystem.GetDevice("PengFei Model RC Simulator - XTR+G2+FMS Controller")) == null)
        {
            yield return null;
        }
        // 드론 컨트롤러 
        droneController = InputSystem.GetDevice("PengFei Model RC Simulator - XTR+G2+FMS Controller");

        Debug.Log("Drone controller initialized.");
    }
    void Update()
    {
        // 마우스 잠금 상태 전환
        /*
        if (Input.GetMouseButtonDown(0))
        {
            UnityEngine.Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = !Cursor.visible;
        }

        // 마우스 입력에 따른 회전
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            rotationX += Input.GetAxis("Mouse X") * sensitivity;
            rotationY -= Input.GetAxis("Mouse Y") * sensitivity;
            rotationY = Mathf.Clamp(rotationY, -90, 90); // 회전 각도 제한
            transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
        }
        */

        DroneControllerSupport.ReadLeftRightStickHorizontal(droneController);

        transform.Rotate(0, DroneControllerSupport.rightHorizontalAxis, 0);

        //Debug.Log(DroneControllerSupport.leftHorizontalAxis + "  " + DroneControllerSupport.rightHorizontalAxis);
    }

    void FixedUpdate()
    {
        // 이동 입력 처리
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) move += transform.forward;
        if (Input.GetKey(KeyCode.S)) move -= transform.forward;
        if (Input.GetKey(KeyCode.A)) move -= transform.right;
        if (Input.GetKey(KeyCode.D)) move += transform.right;
        if (Input.GetKey(KeyCode.Space)) move += transform.up;
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C)) move -= transform.up;

        float leftStickVertical = -1 * Input.GetAxis("Vertical");
        float rightStickVertical = Input.GetAxis("Horizontal");

        Debug.Log(DroneControllerSupport.leftHorizontalAxis + "   " + DroneControllerSupport.rightHorizontalAxis);

        Vector3 moveForwardValue = transform.forward * rightStickVertical;
        Vector3 moveSideValue = transform.right * DroneControllerSupport.leftHorizontalAxis;
        Vector3 moveUpperValue = transform.up * leftStickVertical;

        Vector3 forwardDirection = transform.forward;
        move = moveForwardValue + moveSideValue + moveUpperValue;
        //Debug.Log(move);
        rb.velocity = move * speed;

        
        

        // 움직임 없을 때 속도 초기화
        if (move == Vector3.zero)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep(); // Sleep 상태 활성화
        }
        else
        {
            /*
            float leftStickVertical = -1 * Input.GetAxis("Vertical");
            float rightStickVertical = Input.GetAxis("Horizontal");

            move = new Vector3(0, leftStickVertical, DroneControllerSupport.leftHorizontalAxis);
            rb.velocity = move * speed;\
            */
        }
    }

    /// <summary>
    /// 속도 증가 효과를 적용합니다.
    /// </summary>
    public void ApplySpeedBoost(float duration, float multiplier)
    {
        if (isSpeedBoosted) return; // 이미 속도 증가 상태라면 무시

        isSpeedBoosted = true;
        speed *= multiplier; // 속도 증가
        Debug.Log($"Speed boosted! Current speed: {speed}");

        // 일정 시간 후 속도 복원
        Invoke(nameof(ResetSpeed), duration);
    }

    /// <summary>
    /// 원래 속도로 복원합니다.
    /// </summary>
    private void ResetSpeed()
    {
        speed = originalSpeed; // 원래 속도로 복원
        isSpeedBoosted = false;
        Debug.Log($"Speed reset. Current speed: {speed}");
    }

    /// <summary>
    /// 랜덤 크기 변경 효과를 적용합니다.
    /// </summary>
    public void ApplyRandomSizeChange(float duration)
    {
        if (isSizeChanged) return; // 이미 크기 변경 상태라면 무시

        isSizeChanged = true;

        // 50% 확률로 크기 변경
        float sizeMultiplier = Random.value < 0.5f ? 0.5f : 2f;
        transform.localScale = originalScale * sizeMultiplier;
        Debug.Log($"Size changed! Multiplier: {sizeMultiplier}");

        // 일정 시간 후 크기 복원
        Invoke(nameof(ResetSize), duration);
    }

    /// <summary>
    /// 원래 크기로 복원합니다.
    /// </summary>
    private void ResetSize()
    {
        transform.localScale = originalScale; // 원래 크기로 복원
        isSizeChanged = false;
        Debug.Log("Size reset to original scale");
    }
}