using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;


using System.Runtime.InteropServices;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
public static class DroneControllerSupport
{
    public static float leftHorizontalAxis = 0f;
    public static float rightHorizontalAxis = 0f;
    public static float leftVerticalAxis = 0f;
    public static float rightVerticalAxis = 0f;

    public static void ReadLeftRightStickHorizontal(InputDevice device)
    {

        DroneControllerState state;
        device.CopyState(out state);

        // ��ȯ�� ���� ���
        float normalizedHorizontalValue = NormalizeLeftStickHorizontal(state.leftStickHorizontal);
        //Debug.Log($"���� ���� ��ƽ ��: {normalizedHorizontalValue}");

        float normalizedRightHorizontalValue = NormalizeRightStickHorizontal(state.rightStickHorizontal);
        //Debug.Log($"������ ���� ��ƽ ��: {normalizedRightHorizontalValue}");

        leftHorizontalAxis = normalizedHorizontalValue;
        rightHorizontalAxis = normalizedRightHorizontalValue;
    }


    //���� �޸� �ּҰ��� -1 0 ~ 1 �� ������ ��ȯ
    private static float NormalizeLeftStickHorizontal(byte rawValue)
    {
        float leftMax = 4;     // ���� �ִ밪
        float midMinValue = 110;   // �߰��� �ּ�
        float midValue = 125;   // �߰��� �ּ�

        float midMaxValue = 135;   // �߰��� �ִ�
        float rightMax = 252;  // ������ �ִ밪

        float sensitivity = 1f; // ���� ����, 1.0 �⺻ (0.5 -> �ΰ��� ����, 2.0 -> �ΰ��� ����)

        float normalizedValue;

        if (rawValue >= midMinValue && rawValue <= midMaxValue)
        {
            return 0f; // �߸� ����
        }

        if (rawValue < midValue)
        {
            normalizedValue = -1 * (rawValue - midValue) / (leftMax - midValue);
        }
        else // rawValue > midValue
        {
            normalizedValue = (rawValue - midValue) / (rightMax - midValue);
        }

        // ������ ���ؼ� ��ȯ
        return Mathf.Clamp(normalizedValue * sensitivity, -1f, 1f);
    }

    private static float NormalizeRightStickHorizontal(byte rawValue)
    {
        float leftMax = 7;      // ���� �ִ밪
        float midMinValue = 120;   // �߰��� �ּ�
        float midMaxValue = 140;   // �߰��� �ִ�
        float midValue = 125;   // �߰���
        float rightMax = 253;   // ������ �ִ밪

        float sensitivity = 0.3f; // ���� ����, 1.0 �⺻ (0.5 -> �ΰ��� ����, 2.0 -> �ΰ��� ����)

        float normalizedValue;

        if (rawValue >= midMinValue && rawValue <= midMaxValue)
        {
            return 0f; // �߸� ����
        }

        if (rawValue < midValue)
        {
            normalizedValue = -1 * (rawValue - midValue) / (leftMax - midValue);
        }
        else // rawValue > midValue
        {
            normalizedValue = (rawValue - midValue) / (rightMax - midValue);
        }

        // ������ ���ؼ� ��ȯ
        return Mathf.Clamp(normalizedValue * sensitivity, -1f, 1f);
    }


    [StructLayout(LayoutKind.Explicit, Size = 8)] // ����ü ũ�⸦ ����
    internal struct DroneControllerState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('H', 'I', 'D'); // ���� ����

        // �޸� ��Ʈ�� ����
        [FieldOffset(0)] public byte rawData0; // ù ��° ����Ʈ
        [FieldOffset(1)] public byte rightStickHorizontal; // �� ��° ����Ʈ
        [FieldOffset(2)] public byte leftStickHorizontal; // �� ��° ����Ʈ (�¿�)
        [FieldOffset(3)] public byte rawData3; // �� ��° ����Ʈ
        [FieldOffset(4)] public byte rawData4; // �ټ� ��° ����Ʈ
        [FieldOffset(5)] public byte rawData5; // ���� ��° ����Ʈ
        [FieldOffset(6)] public byte rawData6; // �ϰ� ��° ����Ʈ
        [FieldOffset(7)] public byte rawData7; // ���� ��° ����Ʈ
    }
}