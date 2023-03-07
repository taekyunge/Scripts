using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PuzzleState
{
    PLAY = 0,
    STOP,
}

public class Puzzle : Singleton<Puzzle>
{
    // 보석 아이템 프리팹
    public GameObject PrefabItem;
    public GameObject PrefabEffect;

    public PuzzleCursor Cursor;

    private PuzzlePos[,] _PuzzlePosArray = new PuzzlePos[PuzzleData.PuzzleMaxWidth, PuzzleData.PuzzleMaxHeight + 1];

    private PuzzleItem _ClickedItem;

    public PuzzleItem ClickedItem
    {
        set
        {
            _ClickedItem = value;
            Cursor.gameObject.SetActive(_ClickedItem != null);
        }
        get
        {
            return _ClickedItem;
        }
    }

    public PuzzleState State = PuzzleState.STOP;

    [SerializeField]
    private List<CompleteItem> CompleteItemList = new List<CompleteItem>();

    private Coroutine CoroutineFinished = null;

    public void Awake()
    {
        // 드롭 좌표 지정
        ObjectPool.Instance.CreateObject("PuzzleItem", PrefabItem, 40);

        Transform puzzlePos = transform.Find("PuzzlePos");

        if (puzzlePos == null)
            return;

        for (int i = 0; i <= PuzzleData.PuzzleMaxHeight; i++)
        {
            for (int j = 0; j < PuzzleData.PuzzleMaxWidth; j++)
            {
                _PuzzlePosArray[j, i] = puzzlePos.Find(string.Format("{0}-{1}", j, i)).GetComponent<PuzzlePos>();
                _PuzzlePosArray[j, i].X = j;
                _PuzzlePosArray[j, i].Y = i;
                _PuzzlePosArray[j, i].Effect = Instantiate(PrefabEffect, _PuzzlePosArray[j, i].transform).GetComponent<ParticleController>(); 
            }
        }       
        
        PuzzleManager.Instance.OnPuzzlePlayEvnet += OnPlayEvnet;
        PuzzleManager.Instance.OnPuzzleResetEvent += OnResetEvent;
        PuzzleManager.Instance.OnPuzzleStartEvent += OnStartEvent;
        PuzzleManager.Instance.OnPuzzlePauseEvent += OnPauseEvnet;
    }

    private void OnPlayEvnet()
    {
    }

    private void OnResetEvent()
    {
        State = PuzzleState.STOP;

        if (CoroutineFinished != null)
            StopCoroutine(CoroutineFinished);

        ClickedItem = null;
        DeletePuzzleItem();
    }

    private void OnStartEvent()
    {
        CreatePuzzleItem();
    }

    private void OnPauseEvnet(bool pause)
    {
        ClickedItem = null;
    }

    public void SetReward()
    {
        long point = 0;

        for (int i = 0; i < CompleteItemList.Count; i++)
        {
            if (CompleteItemList[i].PuzzleItemList.Count == 0)
                continue;

            switch(CompleteItemList[i].PuzzleItemList[0].Type)
            {
                case PuzzleItemType.Blue:
                case PuzzleItemType.Orange:
                case PuzzleItemType.Green:
                case PuzzleItemType.Brown:
                case PuzzleItemType.Purple:
                case PuzzleItemType.BlueGem:
                case PuzzleItemType.GreenGem:
                case PuzzleItemType.YellowGem:
                case PuzzleItemType.OrangeGem:
                case PuzzleItemType.PurpleGem:
                case PuzzleItemType.LightningGem:
                case PuzzleItemType.GGem:
                case PuzzleItemType.LockGem:
                    point += ((int)CompleteItemList[i].PuzzleItemList[0].Type + 1) * 100 * CompleteItemList[i].PuzzleItemList.Count;
                    break;
            }
        }
        Debug.Log(string.Format("Reward Point : {0}", point));
    }

    public void CreatePuzzleItem()
    {
        // 빈자리의 보석을 생성한다.
        for (int i = 1; i <= PuzzleData.PuzzleMaxHeight; i++)
        {
            for (int j = 0; j < PuzzleData.PuzzleMaxWidth; j++)
            {
                if (_PuzzlePosArray[j, i].ChildItem == null)
                {
                    PuzzleItem item = ObjectPool.Instance.GetPoolItem("PuzzleItem").GetComponent<PuzzleItem>();

                    _PuzzlePosArray[j, i].ChildItem = item;

                    item.transform.SetParent(_PuzzlePosArray[j, i].transform);
                    item.transform.localPosition = Vector3.zero;
                    item.transform.transform.localScale = Vector3.one;

                    // 떨어질 초기 위치 지정
                    item.transform.position = _PuzzlePosArray[j, 0].transform.position + new Vector3(0, (PuzzleData.PuzzleMaxHeight - i) * 0.2f, 0);
                    item.Type = (PuzzleItemType)Random.Range(0, 4);

                    item.gameObject.SetActive(true);
                }
            }
        }
    }

    public void DeletePuzzleItem()
    {
        for (int i = 1; i <= PuzzleData.PuzzleMaxHeight; i++)
        {
            for (int j = 0; j < PuzzleData.PuzzleMaxWidth; j++)
            {
                if (_PuzzlePosArray[j, i].ChildItem != null)
                {
                    ObjectPool.Instance.DeleteItem("PuzzleItem", _PuzzlePosArray[j, i].ChildItem.gameObject);
                    _PuzzlePosArray[j, i].ChildItem = null;
                    _PuzzlePosArray[j, i].Effect.Play();
                }
            }
        }        
    }

    public void OnDropFinish()
    {
        if (State == PuzzleState.PLAY)
            return;

        for (int i = 1; i <= PuzzleData.PuzzleMaxHeight; i++)
        {
            // X축 체크
            for (int j = 0; j < PuzzleData.PuzzleMaxWidth; j++)
            {
                PuzzleItem item = _PuzzlePosArray[j, i].ChildItem;

                if(item == null || item.State == PuzzleItemState.DROP)
                {
                    State = PuzzleState.STOP;
                    return;
                }
            }            
        }

        Finished();
    }

    public void ChangeItem(PuzzleItem item)
    {
        // 선택된 퍼즐아이템과 위치를 바꾼다. (체크 안되는 퍼즐이 있어 보정 처리 함)
        if (item == null || ClickedItem == item) return;

        if(Mathf.Abs(item.MyPos.X - ClickedItem.MyPos.X) == 1 && Mathf.Abs(item.MyPos.Y - ClickedItem.MyPos.Y) == 1)
        {
            // 대각선 이동 허용
            PuzzlePos ClickedPos = ClickedItem.MyPos;
            PuzzlePos ChangedPos = item.MyPos;

            ClickedPos.ChildItem = item;
            ChangedPos.ChildItem = ClickedItem;
        }
        else
        {
            if (ClickedItem.MyPos.X > item.MyPos.X)
            {
                for (int i = ClickedItem.MyPos.X - 1; i >= item.MyPos.X; i--)
                {
                    PuzzleItem temp = _PuzzlePosArray[i, ClickedItem.MyPos.Y].ChildItem;

                    PuzzlePos ClickedPos = ClickedItem.MyPos;
                    PuzzlePos ChangedPos = temp.MyPos;

                    ClickedPos.ChildItem = temp;
                    ChangedPos.ChildItem = ClickedItem;
                }
            }
            else if (ClickedItem.MyPos.X < item.MyPos.X)
            {
                for (int i = ClickedItem.MyPos.X; i <= item.MyPos.X; i++)
                {
                    PuzzleItem temp = _PuzzlePosArray[i, ClickedItem.MyPos.Y].ChildItem;

                    PuzzlePos ClickedPos = ClickedItem.MyPos;
                    PuzzlePos ChangedPos = temp.MyPos;

                    ClickedPos.ChildItem = temp;
                    ChangedPos.ChildItem = ClickedItem;
                }
            }

            if (ClickedItem.MyPos.Y > item.MyPos.Y)
            {
                for (int i = ClickedItem.MyPos.Y - 1; i >= item.MyPos.Y; i--)
                {
                    PuzzleItem temp = _PuzzlePosArray[ClickedItem.MyPos.X, i].ChildItem;

                    PuzzlePos ClickedPos = ClickedItem.MyPos;
                    PuzzlePos ChangedPos = temp.MyPos;

                    ClickedPos.ChildItem = temp;
                    ChangedPos.ChildItem = ClickedItem;
                }
            }
            else if (ClickedItem.MyPos.Y < item.MyPos.Y)
            {
                for (int i = ClickedItem.MyPos.Y; i <= item.MyPos.Y; i++)
                {
                    PuzzleItem temp = _PuzzlePosArray[ClickedItem.MyPos.X, i].ChildItem;

                    PuzzlePos ClickedPos = ClickedItem.MyPos;
                    PuzzlePos ChangedPos = temp.MyPos;

                    ClickedPos.ChildItem = temp;
                    ChangedPos.ChildItem = ClickedItem;
                }
            }
        }       
    }

    public void Finished()
    {
        if (CoroutineFinished != null)
            StopCoroutine(CoroutineFinished);

        CoroutineFinished = StartCoroutine(FinishedCheck());
    }

    public IEnumerator FinishedCheck()
    {
        // 1차 체크 3개 이상 맞은게 있는지 확인  
        State = PuzzleState.STOP;

        List<CompleteItem> firstCheck = new List<CompleteItem>();

        CompleteItem result = new CompleteItem();

        for (int i = 1; i <= PuzzleData.PuzzleMaxHeight; i++)
        {
            // X축 체크
            for (int j = 0; j < PuzzleData.PuzzleMaxWidth; j++)
            {
                PuzzleItem item = _PuzzlePosArray[j, i].ChildItem;

                if (result.PuzzleItemList.Count == 0)
                {
                    result.PuzzleItemList.Add(item);
                }
                else
                {
                    if (result.PuzzleItemList[0].Type == item.Type)
                    {
                        result.PuzzleItemList.Add(item);
                    }
                    else
                    {
                        if (result.PuzzleItemList.Count >= 3)
                        {
                            firstCheck.Add(result.GetClone());
                        }
                        result.PuzzleItemList.Clear();
                        result.PuzzleItemList.Add(item);
                    }
                }
            }

            if (result.PuzzleItemList.Count >= 3)
            {
                firstCheck.Add(result.GetClone());
            }
            result.PuzzleItemList.Clear();
        }

        for (int j = 0; j < PuzzleData.PuzzleMaxWidth; j++)
        {
            // Y축 체크
            for (int i = 1; i <= PuzzleData.PuzzleMaxHeight; i++)
            {
                PuzzleItem item = _PuzzlePosArray[j, i].ChildItem;

                if (result.PuzzleItemList.Count == 0)
                {
                    result.PuzzleItemList.Add(item);
                }
                else
                {
                    if (result.PuzzleItemList[0].Type == item.Type)
                    {
                        result.PuzzleItemList.Add(item);
                    }
                    else
                    {
                        if (result.PuzzleItemList.Count >= 3)
                        {
                            firstCheck.Add(result.GetClone());
                        }
                        result.PuzzleItemList.Clear();
                        result.PuzzleItemList.Add(item);
                    }
                }
            }

            if (result.PuzzleItemList.Count >= 3)
            {
                firstCheck.Add(result.GetClone());
            }
            result.PuzzleItemList.Clear();
        }

        CompleteItemList.Clear();

        // 2차 체크 겹친 부분이 있는지 확인
        for (int i = 0; i < firstCheck.Count; i++)
        {
            result = firstCheck[i].GetClone();
            for (int j = i + 1; j < firstCheck.Count; j++)
            {
                bool check = false;

                for (int k = 0; k < firstCheck[j].PuzzleItemList.Count; k++)
                {
                    PuzzleItem item = firstCheck[j].PuzzleItemList[k];

                    if (result.PuzzleItemList.Exists(x => x.Type == item.Type))
                    {
                        if (result.PuzzleItemList.Exists(x => Mathf.Abs(x.MyPos.X - item.MyPos.X) <= 1) &&
                            result.PuzzleItemList.Exists(x => Mathf.Abs(x.MyPos.Y - item.MyPos.Y) == 0))
                        {
                            check = true;
                            break;
                        }

                        if (result.PuzzleItemList.Exists(x => Mathf.Abs(x.MyPos.X - item.MyPos.X) == 0) &&
                            result.PuzzleItemList.Exists(x => Mathf.Abs(x.MyPos.Y - item.MyPos.Y) <= 1))
                        {
                            check = true;
                            break;
                        }
                    }
                }

                if (check)
                {
                    for (int k = 0; k < firstCheck[j].PuzzleItemList.Count; k++)
                    {
                        if (!result.PuzzleItemList.Exists(x => x == firstCheck[j].PuzzleItemList[k]))
                        {
                            result.PuzzleItemList.Add(firstCheck[j].PuzzleItemList[k]);
                        }
                    }
                    firstCheck[j].PuzzleItemList.Clear();
                }
            }

            if (result.PuzzleItemList.Count > 0)
                CompleteItemList.Add(result.GetClone());
        }

        if (CompleteItemList.Count > 0)
        {
            // 점수 계산
            SetReward();

            for (int i = 0; i < CompleteItemList.Count; i++)
            {
                for (int j = 0; j < CompleteItemList[i].PuzzleItemList.Count; j++)
                {
                    if (CompleteItemList[i].PuzzleItemList[j] != null)
                    {
                        PuzzlePos pos = CompleteItemList[i].PuzzleItemList[j].MyPos;

                        ObjectPool.Instance.DeleteItem("PuzzleItem", CompleteItemList[i].PuzzleItemList[j].gameObject);

                        pos.ChildItem = null;
                        pos.Effect.Play();
                    }
                }

                PuzzleData.Speed += PuzzleData.PlusSpeed * CompleteItemList[i].PuzzleItemList.Count;
                PuzzleData.TimeValue += PuzzleData.PlusTime * CompleteItemList[i].PuzzleItemList.Count;

                if (i < CompleteItemList.Count)
                    yield return new WaitForSeconds(PuzzleData.PuzzleFinishedSpeed);
            }


            // 남은 드롭 떨구기
            for (int i = PuzzleData.PuzzleMaxHeight; i >= 1; i--)
            {
                for (int j = 0; j < PuzzleData.PuzzleMaxWidth; j++)
                {
                    if (_PuzzlePosArray[j, i].ChildItem == null)
                        continue;

                    PuzzlePos pos = null;

                    for (int k = i + 1; k <= PuzzleData.PuzzleMaxHeight; k++)
                    {
                        if (_PuzzlePosArray[j, k].ChildItem == null)
                        {
                            pos = _PuzzlePosArray[j, k];
                        }
                        else
                            break;
                    }

                    if (pos != null)
                    {
                        pos.ChildItem = _PuzzlePosArray[j, i].ChildItem;
                        _PuzzlePosArray[j, i].ChildItem = null;
                    }
                }
            }

            CreatePuzzleItem();
        }
        else
        {
            State = PuzzleState.PLAY;
        }

        CoroutineFinished = null;
    }
}
