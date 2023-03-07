using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserItem : MonoBehaviour
{
    private int _Index = 0;

    [SerializeField]
    private Text _NameText = null;

    [SerializeField]
    private Image _BackImage = null;

    [SerializeField]
    private Color _EnableColor;

    [SerializeField]
    private Color _DisableColor;

    public void Update()
    {
        _BackImage.color = (SchedulerMgr.CurrentIndex == _Index) ? _EnableColor : _DisableColor;
    }

    public void SetName(int index, string name)
    {
        _Index = index;
        _NameText.text = name;
    }

    public void OnClickItem()
    {
        SchedulerMgr.Instance.SelectName(_Index);
    }

    public void OnClickDelete()
    {
        SchedulerMgr.Instance.RemoveUserName(_Index);
    }
}
