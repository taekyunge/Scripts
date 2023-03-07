using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserTable : Table
{
    public List<string> UserNames = new List<string>();

    public bool[] ServerMark = new bool[8];

    public override void Load()
    {
        UserNames.Clear();

        int userCount = PlayerPrefs.GetInt("UserNameCount");

        for (int i = 0; i < userCount; i++)
        {
            UserNames.Add(PlayerPrefs.GetString(string.Format("UserName-{0}", i)));
        }

        for (int i = 0; i < ServerMark.Length; i++)
        {
            ServerMark[i] = PlayerPrefs.GetInt(string.Format("ServerMark-{0}", i)) == 0 ? true : false;
        }
    }

    public override void Save()
    {
        PlayerPrefs.SetInt("UserNameCount", UserNames.Count);

        for (int i = 0; i < UserNames.Count; i++)
        {
            PlayerPrefs.SetString(string.Format("UserName-{0}", i), UserNames[i]);
        }

        for (int i = 0; i < ServerMark.Length; i++)
        {
            PlayerPrefs.SetInt(string.Format("ServerMark-{0}", i), ServerMark[i] ? 0 : 1);
        }
    }
}
