using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpData
{
    public string Message = string.Empty;
    public Vector3 Pos;
}

public class Help : Popup
{
    [SerializeField]
    private Text _Text = null;

    public override void Open(object obj = null)
    {
        base.Open(obj);

        var data = (HelpData)obj as HelpData;

        _Text.text = data.Message;

        transform.position = data.Pos;
    }
}
