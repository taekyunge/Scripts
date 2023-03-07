using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserScroll : MonoBehaviour
{
    [SerializeField]
    private UserItem _BaseObject = null;

    [SerializeField]
    private Transform _Root = null;

    private Pooling<UserItem> _Pooling = null;

    private List<UserItem> _UsingItems = new List<UserItem>();

    public void Refresh()
    {
        if(_Pooling == null)
            _Pooling = new Pooling<UserItem>(1, _BaseObject, _Root);

        for (int i = 0; i < _UsingItems.Count; i++)
        {
            _Pooling.Delete(_UsingItems[i]);
        }

        _UsingItems.Clear();

        var userNames = SchedulerMgr.Instance.UserNames;

        for (int i = 0; i < userNames.Count; i++)
        {
            var item = _Pooling.Get();

            item.SetName(i, userNames[i]);

            item.transform.SetAsLastSibling();

            _UsingItems.Add(item);
        }
    }
}
