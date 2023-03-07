using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleTimer : MonoBehaviour {

    private Slider _Slider;

    private void Awake()
    {
        _Slider = GetComponentInChildren<Slider>();

        PuzzleManager.Instance.OnPuzzlePlayEvnet += OnPlayEvnet;
        PuzzleManager.Instance.OnPuzzleResetEvent += OnResetEvent;
        PuzzleManager.Instance.OnPuzzleStartEvent += OnStartEvent;
        PuzzleManager.Instance.OnPuzzlePauseEvent += OnPauseEvnet;
    }

    private void OnPlayEvnet()
    {
        PuzzleData.TimeValue -= Time.deltaTime * PuzzleData.Multiple;
        PuzzleData.Multiple += Time.deltaTime * 0.001f;

        if(PuzzleData.MaxValue < PuzzleData.TimeValue)
            PuzzleData.MaxValue = PuzzleData.TimeValue;

        _Slider.value = PuzzleData.TimeValue / PuzzleData.MaxValue;

        if (PuzzleData.TimeValue <= 0)
            PuzzleManager.Instance.OnClickGameReset();
    }

    private void OnResetEvent()
    {
        PuzzleData.MaxValue = PuzzleData.BaseMaxTime;
        PuzzleData.TimeValue = PuzzleData.BaseMaxTime;
        PuzzleData.Multiple = PuzzleData.BaseMultiple;

        _Slider.value = 1;
    }

    private void OnStartEvent()
    {
        PuzzleData.MaxValue = PuzzleData.BaseMaxTime;
        PuzzleData.TimeValue = PuzzleData.BaseMaxTime;
        PuzzleData.Multiple = PuzzleData.BaseMultiple;
    }

    private void OnPauseEvnet(bool pause)
    {
    }    
}
