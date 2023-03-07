using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPad : MonoBehaviour
{
    [SerializeField]
    private LineRenderer _LineRenderer = null;

    public List<TouchObj> _UseTouchObjs = new List<TouchObj>();

    private bool _Start = false;

    private void Clear()
    {
        _UseTouchObjs.Clear();

        OnDrawLine();
    }

    public void OnPointerDown(TouchObj touchObj)
    {
        _Start = true;

        if(touchObj != null)
        {
            OnEnter(touchObj);
        }
    }

    public void OnPointerUp()
    {
        _Start = false;

        if (_UseTouchObjs.Count >= 4)
        {
            GameMgr.Instance.CreateBlocks(_UseTouchObjs);
        }

        Clear();
    }

    public void OnEnter(TouchObj touchObj)
    {
        if (!_Start)
            return;

        if (_UseTouchObjs.Count == 0 || !_UseTouchObjs.Exists(x => x.ID == touchObj.ID))
        {
            _UseTouchObjs.Add(touchObj);

            if (_UseTouchObjs.Count > Define.MaxTouchCount)
            {
                _UseTouchObjs.RemoveAt(0);
            }

            OnDrawLine();
        }
    }

    private void OnDrawLine()
    {
        _LineRenderer.positionCount = _UseTouchObjs.Count;

        for (int i = 0; i < _UseTouchObjs.Count; i++)
        {
            _LineRenderer.SetPosition(i, _UseTouchObjs[i].Pos);
        }
    }
}
