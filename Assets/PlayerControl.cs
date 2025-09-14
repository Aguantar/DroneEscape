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
        Mouse,       // ���콺
        Controller   // �����е�
    }
    [SerializeField]
    private InputDeviceType selectedInputDevice; //�����е� , ���콺 ����

    // Start is called before the first frame update
    public float speed = 1000f; //SERIALIZEFIELD�� �� �ִ��� �𸣰����� �̰ɷ� �ӵ� ����
    public float currentDirection;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;//���۽� ���콺 ���
        Cursor.visible = false;
    }
    void Update()
    {
        /*if (selectedInputDevice == InputDeviceType.Mouse) //���콺,Ű�����Ͻ��ε� Ű�����Է°� ��Ʈ�ѷ��Է��� ���ļ� ��� �Ұ�
        {
            //���������� �������� �޶��� �� ����---------------------------------------------------------------------------------��
            float moveHorizontal = Input.GetAxis("Horizontal"); //Ű���� �Է¹��� a,d                                                   
            float moveVertical = Input.GetAxis("Vertical");  //Ű���� �Է� w,s
            float mouseX = Input.GetAxis("Mouse X"); //���콺 �� ��

            // ���� ��ǥ��� ���� ���(ť�� ������Ʈ �߽��� ��ġ)
            Vector3 forward = cube.forward; // cube ���� ����
            Vector3 right = cube.right;
            //ť���� ������ �޴� ������ �� �� �� ��� ������ �� ����� �� �� �� �츦 �����ϸ� �̻��ϰ� �� �׷��� ������ ������ ���� ť�긦 ��������
            Vector3 targetDirection = (moveHorizontal * right + moveVertical * forward).normalized; //Ű���� ���� ������ vector�� ��ȯ

            //������ ,�����ӿ����� ȸ������                                                                               
            if (targetDirection.magnitude > 0) //wasd �Է� ������                                                                            
            {
                transform.position += targetDirection * speed * Time.deltaTime;   //wasd������ ������

                float tiltAngleX = moveVertical * 15f; // �յ� ���� * Ű���� ��ġ
                float tiltAngleZ = moveHorizontal * -15f; // �¿� ���� * Ű���� ��ġ(������ float�� �ޱ⿡)

                Quaternion targetRotation = Quaternion.Euler(tiltAngleX, transform.rotation.eulerAngles.y, tiltAngleZ); //slerp�� ����ϱ����� ���ʹϾ����� ��ȯ y�� ���ñ��� �����ε� ���� ������Ʈ�� y��ǥ�� ����

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime); //wasd���� �������� ����� õõ�� ���� ��ȯ

            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0), speed * Time.deltaTime);  //wasd �Է��� ������ y���� ������ 0���� �ǵ���
            }

            transform.Rotate(Vector3.up, mouseX * speed); //���콺 �Է��� �޾� ���� y�� ���� vector3.up�� ������ ������ǥ���� up ������ ��б������� �¿��̱� ����(�����ǰ� ������Ʈ�� ���� xyz������ �ٸ�)

            //�� �Ʒ�                                                                                                     
            if (Input.GetKey(KeyCode.Space)) //����� ����
            {
                transform.position += Vector3.up * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.LeftControl)) //����� �Ʒ���
            {
                transform.position += Vector3.down * speed * Time.deltaTime;
            }

        }*/
        if (selectedInputDevice == InputDeviceType.Controller)
        {
            float moveHorizontal = DroneControllerSupport.rightHorizontalAxis; //Ű���� �Է¹��� a,d
            float moveVertical = DroneControllerSupport.rightVerticalAxis;  //Ű���� �Է� w,s //�̰Ÿ� ����
            float mouseX = DroneControllerSupport.leftHorizontalAxis; //���콺 �� ��
            float updown = DroneControllerSupport.leftVerticalAxis; //�� ���� Ư���������� 0�� �����ϵ��� �ؾ���
            mouseX = MapValue(mouseX, -0.76f, 1.26f, -1f, 1f); //�Է¹��� �� ������ -1~1�� ����
            mouseX = ApplyDeadZone(mouseX, 0f, 0.3f);         //�߰��� ���� ���� �Է��� ����
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
                // �Է°��� ������ ���� ���� ������ 0���� ó��
                if (value >= value2 && value <= value3)
                {
                    return 0f; // �Է°��� ������ ���� ���� ������ 0���� ó��
                }
                return value; // �� �ܿ��� ���� �� �״�� ��ȯ
            }

            // ���� ��ǥ��� ���� ���(ť�� ������Ʈ �߽��� ��ġ)
            Vector3 forward = cube.forward; // cube ���� ����
            Vector3 right = cube.right;
            //ť���� ������ �޴� ������ �� �� �� ��� ������ �� ����� �� �� �� �츦 �����ϸ� �̻��ϰ� �� �׷��� ������ ������ ���� ť�긦 ��������
            Vector3 targetDirection = (moveHorizontal * right + moveVertical * forward).normalized; //Ű���� ���� ������ vector�� ��ȯ

            //������ ,�����ӿ����� ȸ������                                                                               
            if (targetDirection.magnitude > 0) //wasd �Է� ������                                                                            
            {
                transform.position += targetDirection * speed * Time.deltaTime;   //wasd������ ������

                float tiltAngleX = moveVertical * 15f; // �յ� ���� * Ű���� ��ġ
                float tiltAngleZ = moveHorizontal * -15f; // �¿� ���� * Ű���� ��ġ(������ float�� �ޱ⿡)

                Quaternion targetRotation = Quaternion.Euler(tiltAngleX, transform.rotation.eulerAngles.y, tiltAngleZ); //slerp�� ����ϱ����� ���ʹϾ����� ��ȯ y�� ���ñ��� �����ε� ���� ������Ʈ�� y��ǥ�� ����

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime); //wasd���� �������� ����� õõ�� ���� ��ȯ

            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0), speed * Time.deltaTime);  //wasd �Է��� ������ y���� ������ 0���� �ǵ���
            }

            transform.Rotate(Vector3.up, mouseX * 0.5f); //���콺 �Է��� �޾� ���� y�� ���� vector3.up�� ������ ������ǥ���� up ������ ��б������� �¿��̱� ����(�����ǰ� ������Ʈ�� ���� xyz������ �ٸ�)


            transform.position += Vector3.up * speed * updown * Time.deltaTime;

            //�� �Ʒ�                                                                                                     

        }
        //��------------------------------------------------------------------------------------------------------------------��

    }
}