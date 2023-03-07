using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterScroll : MonoBehaviour
{
    [SerializeField]
    private ScrollRect _Scroll = null;

    [SerializeField]
    private GridLayoutGroup _Group = null;

    [SerializeField]
    private CharacterItem _BaseObject = null;

    [SerializeField]
    private Transform _Root = null;

    private Pooling<CharacterItem> _Pooling = null;

    private List<CharacterItem> _UsingItems = new List<CharacterItem>();

    private Vector3 _StartPos;

    private int _StartSibling = 0;
    private CharacterItem _PointItem = null;
    public static CharacterItem SwapItem = null;

    private void Update()
    {
        if(_PointItem != null)
        {
            var pos = _StartPos - transform.TransformPoint(Input.mousePosition);

            _PointItem.transform.position -= pos;

            _StartPos = transform.TransformPoint(Input.mousePosition);
        }

        _Scroll.enabled = SchedulerMgr.Lock;
    }

    public void Refresh()
    {
        if (_Pooling == null)
            _Pooling = new Pooling<CharacterItem>(20, _BaseObject, _Root);

        for (int i = 0; i < _UsingItems.Count; i++)
        {
            _Pooling.Delete(_UsingItems[i]);
        }

        _UsingItems.Clear();

        var characterDatas = SchedulerMgr.Instance.CharacterDatas;

        for (int i = 0; i < characterDatas.Count; i++)
        {
            int serverNumber = LocalDB.GetServerIndex(characterDatas[i].Name);

            if (LocalDB.GetServerMark(serverNumber))
            {
                var item = _Pooling.Get();

                item.transform.SetAsLastSibling();
                item.SetItem(characterDatas[i]);

                _UsingItems.Add(item);
            }
        }
    }

    public void OnPointerDown(CharacterItem item)
    {
        if (SchedulerMgr.Lock)
            return;

        _StartPos = transform.TransformPoint(Input.mousePosition);
        _StartSibling = item.transform.GetSiblingIndex();

        item.transform.SetAsLastSibling();
        item._Drag = true;

        _PointItem = item;
        _Group.enabled = false;
    }

    public void OnPointerUp()
    {
        if (SchedulerMgr.Lock)
            return;

        if (_PointItem != null)
        {
            if(SwapItem != null)
            {
                int sibling = SwapItem.transform.GetSiblingIndex();

                _PointItem.transform.SetSiblingIndex(sibling);
                SwapItem.transform.SetSiblingIndex(_StartSibling);
            }
            else
            {
                _PointItem.transform.SetSiblingIndex(_StartSibling);
            }

            for (int i = 0; i < _UsingItems.Count; i++)
            {
                _UsingItems[i].SaveNumber();
            }

            SchedulerMgr.Instance.CharacterDatas.Sort((x, y) =>
            {
                if (x.Number > y.Number)
                    return 1;
                else if (x.Number < y.Number)
                    return -1;
                else
                    return 0;
            });

            _PointItem._Drag = false;
            _PointItem = null;
            SwapItem = null;
        }

        _StartSibling = -1;
        _Group.enabled = true;
    }

    public void OnPointerEnter(CharacterItem item)
    {
        if (SchedulerMgr.Lock)
            return;

        if (_PointItem != null)
        {
            SwapItem = item;
        }
    }

    public void OnPointerExit(CharacterItem item)
    {
        if (SchedulerMgr.Lock)
            return;

        if (_PointItem != null)
        {
            if (SwapItem == item)
                SwapItem = null;
        }
    }
}
