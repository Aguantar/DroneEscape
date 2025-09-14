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

        // 변환된 값을 계산
        float normalizedHorizontalValue = NormalizeLeftStickHorizontal(state.leftStickHorizontal);
        //Debug.Log($"왼쪽 수평 스틱 값: {normalizedHorizontalValue}");

        float normalizedRightHorizontalValue = NormalizeRightStickHorizontal(state.rightStickHorizontal);
        //Debug.Log($"오른쪽 수평 스틱 값: {normalizedRightHorizontalValue}");

        leftHorizontalAxis = normalizedHorizontalValue;
        rightHorizontalAxis = normalizedRightHorizontalValue;
    }


    //들어온 메모리 주소값을 -1 0 ~ 1 의 값으로 변환
    private static float NormalizeLeftStickHorizontal(byte rawValue)
    {
        float leftMax = 4;     // 왼쪽 최대값
        float midMinValue = 110;   // 중간값 최소
        float midValue = 125;   // 중간값 최소

        float midMaxValue = 135;   // 중간값 최대
        float rightMax = 252;  // 오른쪽 최대값

        float sensitivity = 1f; // 감도 배율, 1.0 기본 (0.5 -> 민감도 낮음, 2.0 -> 민감도 높음)

        float normalizedValue;

        if (rawValue >= midMinValue && rawValue <= midMaxValue)
        {
            return 0f; // 중립 상태
        }

        if (rawValue < midValue)
        {
            normalizedValue = -1 * (rawValue - midValue) / (leftMax - midValue);
        }
        else // rawValue > midValue
        {
            normalizedValue = (rawValue - midValue) / (rightMax - midValue);
        }

        // 감도를 곱해서 반환
        return Mathf.Clamp(normalizedValue * sensitivity, -1f, 1f);
    }

    private static float NormalizeRightStickHorizontal(byte rawValue)
    {
        float leftMax = 7;      // 왼쪽 최대값
        float midMinValue = 120;   // 중간값 최소
        float midMaxValue = 140;   // 중간값 최대
        float midValue = 125;   // 중간값
        float rightMax = 253;   // 오른쪽 최대값

        float sensitivity = 0.3f; // 감도 배율, 1.0 기본 (0.5 -> 민감도 낮음, 2.0 -> 민감도 높음)

        float normalizedValue;

        if (rawValue >= midMinValue && rawValue <= midMaxValue)
        {
            return 0f; // 중립 상태
        }

        if (rawValue < midValue)
        {
            normalizedValue = -1 * (rawValue - midValue) / (leftMax - midValue);
        }
        else // rawValue > midValue
        {
            normalizedValue = (rawValue - midValue) / (rightMax - midValue);
        }

        // 감도를 곱해서 반환
        return Mathf.Clamp(normalizedValue * sensitivity, -1f, 1f);
    }


    [StructLayout(LayoutKind.Explicit, Size = 8)] // 구조체 크기를 정의
    internal struct DroneControllerState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('H', 'I', 'D'); // 포맷 정의

        // 메모리 비트값 정의
        [FieldOffset(0)] public byte rawData0; // 첫 번째 바이트
        [FieldOffset(1)] public byte rightStickHorizontal; // 두 번째 바이트
        [FieldOffset(2)] public byte leftStickHorizontal; // 세 번째 바이트 (좌우)
        [FieldOffset(3)] public byte rawData3; // 네 번째 바이트
        [FieldOffset(4)] public byte rawData4; // 다섯 번째 바이트
        [FieldOffset(5)] public byte rawData5; // 여섯 번째 바이트
        [FieldOffset(6)] public byte rawData6; // 일곱 번째 바이트
        [FieldOffset(7)] public byte rawData7; // 여덟 번째 바이트
    }
}