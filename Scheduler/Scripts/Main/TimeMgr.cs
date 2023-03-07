using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMgr : Singleton<TimeMgr>
{
    private int _Hour = 6;
    private int _Minute = 00;

    public void Initialize()
    {
        int quitTime = (PlayerPrefs.HasKey("QuitTime")) ? PlayerPrefs.GetInt("QuitTime") : Utill.ConvertToUnixTimestamp(DateTime.Now);
        var quitDateTime = Utill.ConvertFromUnixTimestamp(quitTime);
        var dayInitTime = GetDayInitTime(quitDateTime);

        while (DateTime.Compare(DateTime.Now, dayInitTime) > 0)
        {
            SchedulerMgr.Instance.ContentClear(ContentResetType.Day);

            dayInitTime = GetDayInitTime(dayInitTime);
        }

        OnApplicationQuit();
    }

    private void OnApplicationQuit()
    {
        var quitTime = Utill.ConvertToUnixTimestamp(DateTime.Now);

        PlayerPrefs.SetInt("QuitTime", quitTime);
    }

    private DateTime GetDayInitTime(DateTime connectTime)
    {
        DateTime initTime;

        if (connectTime.Hour < _Hour || (connectTime.Hour == _Hour && connectTime.Minute < _Minute))
        {
            initTime = new DateTime(connectTime.Year, connectTime.Month, connectTime.Day, _Hour, _Minute, 0);
        }
        else
        {
            connectTime = connectTime.AddDays(1);
            initTime = new DateTime(connectTime.Year, connectTime.Month, connectTime.Day, _Hour, _Minute, 0);
        }

        return initTime;
    }
}
