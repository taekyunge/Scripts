using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController> {

    public Animator PlayerAnimator;

    public void Start()
    {
        PuzzleManager.Instance.OnPuzzlePlayEvnet += OnPlayEvnet;
        PuzzleManager.Instance.OnPuzzleResetEvent += OnResetEvent;
        PuzzleManager.Instance.OnPuzzleStartEvent += OnStartEvent;
        PuzzleManager.Instance.OnPuzzlePauseEvent += OnPauseEvnet;
    }

    private void OnPlayEvnet()
    {
        PlayerAnimator.speed = PuzzleData.Speed;
        PuzzleData.Distance += 0.01f * PuzzleData.Speed;

        if (PuzzleData.Speed > 1f)
        {
            PuzzleData.Speed -= PuzzleData.MinusSpeed;
        }
        else
        {
            PuzzleData.Speed = 1;
        }
    }

    private void OnResetEvent()
    {
        PlayerAnimator.SetBool("run", false);
    }

    private void OnStartEvent()
    {
        PlayerAnimator.SetBool("run", true);
    }

    private void OnPauseEvnet(bool pause)
    {
        if(pause)
        {
            PlayerAnimator.speed = 0;
        }
        else
        {
            PlayerAnimator.speed = PuzzleData.Speed;
        }
    }

    //public void SetSpeed(float speed)
    //{
    //    Speed += speed;

    //    PlayerAnimator.SetTrigger("attack");
    //}
}
