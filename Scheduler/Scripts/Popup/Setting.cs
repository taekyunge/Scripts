using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : Popup
{
    [SerializeField]
    private Toggle[] _Toggles = null;

    public override void Open(object obj = null)
    {
        base.Open(obj);

        for (int i = 0; i < _Toggles.Length; i++)
        {
            _Toggles[i].isOn = LocalDB.GetServerMark(i);
        }
    }

    public override void Close()
    {
        base.Close();

        for (int i = 0; i < _Toggles.Length; i++)
        {
            LocalDB.SetServerMark(i, _Toggles[i].isOn);
        }

        LocalDB.Save();

        SchedulerMgr.Instance.Refresh();
    }
}
