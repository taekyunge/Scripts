using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateUser : Popup
{
    [SerializeField]
    private InputField _Input = null;

    public override void Open(object obj = null)
    {
        base.Open(obj);

        _Input.text = string.Empty;
    }

    public void OnClickEnter()
    {
        SchedulerMgr.Instance.AddUserName(_Input.text);

        Close();
    }
}
