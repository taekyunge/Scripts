using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Popup : MonoBehaviour
{
    public PopupType Type;

    public bool OnlyOnce;
    public bool Background;

    public virtual void Open(object obj = null)
    {

    }

    public virtual void Close()
    {
        PopupMgr.Instance.Close(this);
    }
}
