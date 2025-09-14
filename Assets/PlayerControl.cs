using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    Transform cube;
    public enum InputDeviceType
    {
        Mouse,       // 마우스
        Controller   // 게임패드
    }
    [SerializeField]
    private InputDeviceType selectedInputDevice; //게임패드 , 마우스 선택

    // Start is called before the first frame update
    public float speed = 1000f; //SERIALIZEFIELD에 왜 있는진 모르겠지만 이걸로 속도 조절
    public float currentDirection;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;//시작시 마우스 잠금
        Cursor.visible = false;
    }
    void Update()
    {
        /*if (selectedInputDevice == InputDeviceType.Mouse) //마우스,키보드일시인데 키보드입력과 컨트롤러입력이 겹쳐서 사용 불가
        {
            //ㅁ디테일한 움직임은 달라질 수 있음---------------------------------------------------------------------------------ㄱ
            float moveHorizontal = Input.GetAxis("Horizontal"); //키보드 입력받음 a,d                                                   
            float moveVertical = Input.GetAxis("Vertical");  //키보드 입력 w,s
            float mouseX = Input.GetAxis("Mouse X"); //마우스 좌 우

            // 로컬 좌표계로 방향 계산(큐브 오브젝트 중심의 위치)
            Vector3 forward = cube.forward; // cube 기준 전방
            Vector3 right = cube.right;
            //큐브의 방향을 받는 이유는 앞 뒤 좌 우로 움직일 때 드론의 앞 뒤 좌 우를 참조하면 이상하게 감 그래서 고정된 각도를 가진 큐브를 기준으로
            Vector3 targetDirection = (moveHorizontal * right + moveVertical * forward).normalized; //키보드 누른 방향을 vector로 변환

            //움직임 ,움직임에따른 회전제어                                                                               
            if (targetDirection.magnitude > 0) //wasd 입력 받으면                                                                            
            {
                transform.position += targetDirection * speed * Time.deltaTime;   //wasd에따라 움직임

                float tiltAngleX = moveVertical * 15f; // 앞뒤 기울기 * 키보드 위치
                float tiltAngleZ = moveHorizontal * -15f; // 좌우 기울기 * 키보드 위치(각도는 float만 받기에)

                Quaternion targetRotation = Quaternion.Euler(tiltAngleX, transform.rotation.eulerAngles.y, tiltAngleZ); //slerp에 사용하기위해 쿼터니언으로 변환 y는 로컬기준 각도인데 현제 오브젝트의 y좌표로 고정

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime); //wasd누른 방향으로 드론이 천천히 각도 변환

            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0), speed * Time.deltaTime);  //wasd 입력이 끝나면 y제외 각도를 0으로 되돌림
            }

            transform.Rotate(Vector3.up, mouseX * speed); //마우스 입력을 받아 각도 y를 조정 vector3.up인 이유는 월드좌표기준 up 방향이 드론기준으론 좌우이기 떄문(포지션과 로테이트는 쓰는 xyz순서가 다름)

            //위 아래                                                                                                     
            if (Input.GetKey(KeyCode.Space)) //드론을 위로
            {
                transform.position += Vector3.up * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.LeftControl)) //드론을 아래로
            {
                transform.position += Vector3.down * speed * Time.deltaTime;
            }

        }*/
        if (selectedInputDevice == InputDeviceType.Controller)
        {
            float moveHorizontal = DroneControllerSupport.rightHorizontalAxis; //키보드 입력받음 a,d
            float moveVertical = DroneControllerSupport.rightVerticalAxis;  //키보드 입력 w,s //이거만 정상
            float mouseX = DroneControllerSupport.leftHorizontalAxis; //마우스 좌 우
            float updown = DroneControllerSupport.leftVerticalAxis; //각 값의 특정값까지는 0을 유지하도록 해야함
            mouseX = MapValue(mouseX, -0.76f, 1.26f, -1f, 1f); //입력받은 값 범위를 -1~1로 변경
            mouseX = ApplyDeadZone(mouseX, 0f, 0.3f);         //중간에 작은 값의 입력은 무시
            updown = MapValue(updown, -0.72f, 0.98f, -1f, 1f);
            updown = ApplyDeadZone(updown, -0.2f, 0f);
            moveVertical = MapValue(moveVertical, -0.76f, 0.98f, -1f, 1f);
            moveVertical = ApplyDeadZone(moveVertical, -0.24f, 0f);

            moveHorizontal = MapValue(moveHorizontal, -0.86f, 1f, -1f, 1f);
            moveHorizontal = ApplyDeadZone(moveHorizontal, 0f, 0.2f);

            UnityEngine.Debug.Log(moveHorizontal);

            //0.15~0.20
            float MapValue(float value, float fromMin, float fromMax, float toMin, float toMax)
            {
                return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
            }
            float ApplyDeadZone(float value, float value2, float value3)
            {
                // 입력값이 데드존 범위 내에 있으면 0으로 처리
                if (value >= value2 && value <= value3)
                {
                    return 0f; // 입력값이 데드존 범위 내에 있으면 0으로 처리
                }
                return value; // 그 외에는 원래 값 그대로 반환
            }

            // 로컬 좌표계로 방향 계산(큐브 오브젝트 중심의 위치)
            Vector3 forward = cube.forward; // cube 기준 전방
            Vector3 right = cube.right;
            //큐브의 방향을 받는 이유는 앞 뒤 좌 우로 움직일 때 드론의 앞 뒤 좌 우를 참조하면 이상하게 감 그래서 고정된 각도를 가진 큐브를 기준으로
            Vector3 targetDirection = (moveHorizontal * right + moveVertical * forward).normalized; //키보드 누른 방향을 vector로 변환

            //움직임 ,움직임에따른 회전제어                                                                               
            if (targetDirection.magnitude > 0) //wasd 입력 받으면                                                                            
            {
                transform.position += targetDirection * speed * Time.deltaTime;   //wasd에따라 움직임

                float tiltAngleX = moveVertical * 15f; // 앞뒤 기울기 * 키보드 위치
                float tiltAngleZ = moveHorizontal * -15f; // 좌우 기울기 * 키보드 위치(각도는 float만 받기에)

                Quaternion targetRotation = Quaternion.Euler(tiltAngleX, transform.rotation.eulerAngles.y, tiltAngleZ); //slerp에 사용하기위해 쿼터니언으로 변환 y는 로컬기준 각도인데 현제 오브젝트의 y좌표로 고정

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime); //wasd누른 방향으로 드론이 천천히 각도 변환

            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0), speed * Time.deltaTime);  //wasd 입력이 끝나면 y제외 각도를 0으로 되돌림
            }

            transform.Rotate(Vector3.up, mouseX * 0.5f); //마우스 입력을 받아 각도 y를 조정 vector3.up인 이유는 월드좌표기준 up 방향이 드론기준으론 좌우이기 떄문(포지션과 로테이트는 쓰는 xyz순서가 다름)


            transform.position += Vector3.up * speed * updown * Time.deltaTime;

            //위 아래                                                                                                     

        }
        //ㄴ------------------------------------------------------------------------------------------------------------------ㅁ

    }
}