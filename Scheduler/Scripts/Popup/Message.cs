using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : Popup
{
    [SerializeField]
    private Text _Text = null;

    public override void Open(object obj = null)
    {
        base.Open(obj);

        _Text.text = (string)obj;
    }
}
