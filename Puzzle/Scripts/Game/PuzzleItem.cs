using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public enum PuzzleItemState
{
    WAIT,
    DROP,
    DROP_FINISH,
}

public enum PuzzleItemType
{
    Blue = 0,
    Orange,
    Green,
    Brown,
    Purple,
    BlueGem,
    GreenGem,
    YellowGem,
    OrangeGem,
    PurpleGem,
    LightningGem,
    GGem,
    LockGem,
}

public class PuzzleItem : MonoBehaviour {

    public delegate void OnGemEvent();

    public PuzzlePos MyPos = null;

    public List<Sprite> GemImages = new List<Sprite>();

    public OnGemEvent OnDropStart;
    public OnGemEvent OnDroping;
    public OnGemEvent OnDropEnd;

    private Image _ItemImage;

    public Sprite ItemSprite
    {
        get { return (_ItemImage == null) ? null : _ItemImage.sprite; }
    }   

    private PuzzleItemState _State;
    public PuzzleItemState State
    {
        set { _State = value; }
        get { return _State; }
    }
    public PuzzleItemType test;
    private PuzzleItemType _Type;
    public PuzzleItemType Type
    {
        set
        {
            test = value;
               _Type = value;

            if(_ItemImage != null)
            {
                _ItemImage.sprite = GemImages[(int)_Type];
                _ItemImage.SetNativeSize();
            }           
        }
        get { return _Type; }
    }

    public Color EnableColor;
    public Color DisableColor;

    public void Awake()
    {
        _ItemImage = transform.Find("Image - Gem").GetComponent<Image>();

        PuzzleManager.Instance.OnPuzzlePlayEvnet += OnPlayEvnet;
        PuzzleManager.Instance.OnPuzzleResetEvent += OnResetEvent;
        PuzzleManager.Instance.OnPuzzleStartEvent += OnStartEvent;
        PuzzleManager.Instance.OnPuzzlePauseEvent += OnPauseEvnet;
    }

    public void OnDestroy()
    {
        PuzzleManager.Instance.OnPuzzlePlayEvnet -= OnPlayEvnet;
        PuzzleManager.Instance.OnPuzzleResetEvent -= OnResetEvent;
        PuzzleManager.Instance.OnPuzzleStartEvent -= OnStartEvent;
        PuzzleManager.Instance.OnPuzzlePauseEvent -= OnPauseEvnet;
    }

    private void OnPlayEvnet()
    {
        if(Vector3.Distance(transform.localPosition, Vector3.zero) >= 1)
        {
            State = PuzzleItemState.DROP;
        }
        else
        {
            if(State == PuzzleItemState.DROP)
            {
                State = PuzzleItemState.DROP_FINISH;
            }
            else
            {
                State = PuzzleItemState.WAIT;
            }
        }

        switch(State)
        {
            case PuzzleItemState.DROP:
                transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * PuzzleData.PuzzleItemSpeed);
                break;
            case PuzzleItemState.DROP_FINISH:
                Puzzle.Instance.OnDropFinish();
                break;
            case PuzzleItemState.WAIT:
                break;
        }
    }

    private void OnResetEvent()
    {
    }

    private void OnStartEvent()
    {
    }

    private void OnPauseEvnet(bool pause)
    {
    }

    public void OnPointerDown()
    {
        switch (Puzzle.Instance.State)
        {
            case PuzzleState.PLAY:
                _ItemImage.color = DisableColor;
                Puzzle.Instance.ClickedItem = this;
                break;

            default:
                break;
        }       
    }

    public void OnPointerUp()
    {
        switch (Puzzle.Instance.State)
        {
            case PuzzleState.PLAY:
                _ItemImage.color = EnableColor;
                Puzzle.Instance.ClickedItem = null;
                break;

            default:
                break;
        }        
    }
}
