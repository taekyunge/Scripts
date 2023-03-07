using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogMgr : MonoBehaviour
{
    private static LogMgr _Instance;
    public static LogMgr Instance
    {
        get
        {
            return _Instance;
        }
    }

    [SerializeField]
    private Text _LogText = null;

    private void Awake()
    {
        _Instance = this;
    }

    public void Log(string message)
    {
        Debug.Log(message);

        _LogText.text = message;
    }
}
