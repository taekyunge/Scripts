using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PuzzleData {

    public static int PuzzleMaxWidth = 6;
    public static int PuzzleMaxHeight = 5;

    public static float PuzzleItemSpeed = 12;
    public static float PuzzleFinishedSpeed = 0.25f;

    public static float MinusSpeed = 0.005f;

    public static float PlusSpeed = 0.1f;
    public static float PlusTime = 0.35f;

    // 기본 타이머 최대시간
    public static float BaseMaxTime = 10;
    // 기본 타이머 빠르기 배수
    public static float BaseMultiple = 1;

    [HideInInspector] public static float MaxValue;
    // 사용중 타이머 최대시간
    [HideInInspector] public static float TimeValue;
    // 사용중 타이머 빠르기 배수
    [HideInInspector] public static float Multiple;
    [HideInInspector] public static float Speed = 0.5f;
    [HideInInspector] public static float Distance = 0f;
}
