using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    WAIT = 0,
    PLAY,
    PAUSE,
}

public class PuzzleManager : Singleton<PuzzleManager> {

    public delegate void PuzzlePlayEvent();
    public delegate void PuzzleResetEvent();
    public delegate void PuzzleStartEvent();
    public delegate void PuzzlePauseEvent(bool pause);

    private GameState _State;
    public GameState State
    {
        set { _State = value; }
        get { return _State; }
    }

    public PuzzlePlayEvent OnPuzzlePlayEvnet = null;
    public PuzzleResetEvent OnPuzzleResetEvent = null;
    public PuzzleStartEvent OnPuzzleStartEvent = null;
    public PuzzlePauseEvent OnPuzzlePauseEvent = null;

    public Text TotalDistanceText;
    public Text SpeedText;

    //private float _WaitTime = 0.0f;
    //private float _PauseTime = 0.0f;

    #region TEST
    public Text StateText;
    #endregion

    private void Awake()
    {
        OnPuzzlePlayEvnet += OnPlayEvnet;
        OnPuzzleResetEvent += OnResetEvent;
        OnPuzzleStartEvent += OnStartEvent;
        OnPuzzlePauseEvent += OnPauseEvnet;
    }

    private void Update()
    {
        StateText.text = State.ToString();
        TotalDistanceText.text = string.Format("{0}M", PuzzleData.Distance.ToString("N1"));
        SpeedText.text = string.Format("{0}", PuzzleData.Speed.ToString("N1"));

        switch (State)
        {
            case GameState.WAIT:
                // 대기 상태 시간 체크 및 기타 이벤트 등록
                //_WaitTime += Time.deltaTime;
                break;

            case GameState.PLAY:
                OnPuzzlePlayEvnet.Invoke();
                break;

            case GameState.PAUSE:
                // 정지 상태 시간 체크 및 기타 이벤트 등록
                //_PauseTime += Time.deltaTime;
                break;
        }
    }

    private void OnPlayEvnet()
    {

    }

    private void OnResetEvent()
    {
       
    }

    private void OnStartEvent()
    {
        PuzzleData.Distance = 0f;
        PuzzleData.Speed = 1f;
    }

    private void OnPauseEvnet(bool pause)
    {

    }

    private void GameReset()
    {
        Debug.Log("Game Reset");

        State = GameState.WAIT;

        if (OnPuzzleResetEvent != null) OnPuzzleResetEvent.Invoke();
    }

    private void GameStart()
    {
        Debug.Log("Game Start");

        if (State > GameState.WAIT)
            return;

        State = GameState.PLAY;

        if (OnPuzzleStartEvent != null) OnPuzzleStartEvent.Invoke();
    }

    public void GamePause(bool pause)
    {
        Debug.Log("Game Stop");

        if(pause)
        {
            State = GameState.PAUSE;

            if (OnPuzzlePauseEvent != null) OnPuzzlePauseEvent.Invoke(pause);
        }
        else
        {
            State = GameState.PLAY;
        }
    }    

    public void OnClickGameStart()
    {
        GameStart();
    }

    public void OnClickGameReset()
    {
        GameReset();
    }

    public void OnClickGamePause()
    {
        GamePause((State != GameState.PAUSE));
    }    
}
