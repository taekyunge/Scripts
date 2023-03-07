using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LocalDB
{
    private static Dictionary<string, Table> _Tables = new Dictionary<string, Table>();

    private static List<GoldData> _ContentGolds = new List<GoldData>();

    public static void Initialize()
    {
        _Tables.Add("UserTable", new UserTable());
        _Tables.Add("CharacterTable", new CharacterTable());
        _Tables.Add("ContentTable", new ContentTable());


        _ContentGolds.Add(new GoldData(ContentType.오레하의_유물_노말, 1100, 1355, 1415, 600));
        _ContentGolds.Add(new GoldData(ContentType.오레하의_유물_하드, 1300, 1355, 1415, 800));
        _ContentGolds.Add(new GoldData(ContentType.아르고스, 1400, 1370, 1475, 500));
        _ContentGolds.Add(new GoldData(ContentType.발탄_노말, 2500, 1415, 0, 1300));
        _ContentGolds.Add(new GoldData(ContentType.발탄_하드, 4500, 1445, 0, 2100));
        _ContentGolds.Add(new GoldData(ContentType.비아키스_노말, 2500, 1430, 0, 1800));
        _ContentGolds.Add(new GoldData(ContentType.비아키스_하드, 4500, 1460, 0, 2800));
        _ContentGolds.Add(new GoldData(ContentType.쿠크세이튼_노말, 4500, 1475, 0, 3100));
        _ContentGolds.Add(new GoldData(ContentType.아브렐슈드_노말_1a2관문, 4500, 1490, 0, 1000));
        _ContentGolds.Add(new GoldData(ContentType.아브렐슈드_노말_1a4관문, 6000, 1500, 0, 2500));
        _ContentGolds.Add(new GoldData(ContentType.아브렐슈드_노말_1a6관문, 8500, 1520, 0, 4500));
        _ContentGolds.Add(new GoldData(ContentType.아브렐슈드_하드_1a2관문, 5500, 1540, 0, 1500));
        _ContentGolds.Add(new GoldData(ContentType.아브렐슈드_하드_1a4관문, 7500, 1550, 0, 3500));
        _ContentGolds.Add(new GoldData(ContentType.아브렐슈드_하드_1a6관문, 10500,1560, 0, 6000));
        _ContentGolds.Add(new GoldData(ContentType.카양겔_노말, 0, 1475, 0, 1000));
        _ContentGolds.Add(new GoldData(ContentType.카양겔_하드_1, 0, 1520, 0, 1500));
        _ContentGolds.Add(new GoldData(ContentType.카양겔_하드_2, 0, 1560, 0, 2000));
        _ContentGolds.Add(new GoldData(ContentType.카양겔_하드_3, 0, 1580, 0, 2500));
        _ContentGolds.Add(new GoldData(ContentType.일리아칸_노말, 5500, 1580, 0, 3500));
        _ContentGolds.Add(new GoldData(ContentType.일리아칸_하드, 6500, 1600, 0, 4500));

        Load();
    }

    public static void Save()
    {
        foreach (var table in _Tables)
        {
            table.Value.Save();
        }
    }

    public static void Load()
    {
        foreach (var table in _Tables)
        {
            table.Value.Load();
        }
    }

    public static GoldData GetGoldData(ContentType type)
    {
        return _ContentGolds.Find(x => x.Type == type);
    }

    public static void SetUserData(List<string> userNames)
    {
        (_Tables["UserTable"] as UserTable).UserNames = userNames;
    }

    public static List<string> GetUserData()
    {
        return (_Tables["UserTable"] as UserTable).UserNames;
    }

    public static void SetServerMark(int number, bool value)
    {
        (_Tables["UserTable"] as UserTable).ServerMark[number] = value;
    }

    public static bool GetServerMark(int number)
    {
        return (_Tables["UserTable"] as UserTable).ServerMark[number];
    }

    public static bool IsCharacterData(string name)
    {
        return (_Tables["CharacterTable"] as CharacterTable).IsCharacter(name);
    }

    public static bool IsGold(string name)
    {
        return (_Tables["CharacterTable"] as CharacterTable).IsGold(name);
    }

    public static void SetGold(string name, bool value)
    {
        (_Tables["CharacterTable"] as CharacterTable).SetGold(name, value);
    }

    public static void SetCharacterData(string name, string serverName, string itemlevel, string guildName)
    {
        var table = _Tables["CharacterTable"] as CharacterTable;

        table.ServerNames[name] = serverName;
        table.ItemLevels[name] = itemlevel;
        table.GuildNames[name] = guildName;

        table.Save();
    }

    public static string GetServerName(string name)
    {
        return (_Tables["CharacterTable"] as CharacterTable).ServerNames[name];
    }

    public static int GetServerIndex(string name)
    {
        string serverName = (_Tables["CharacterTable"] as CharacterTable).ServerNames[name];

        switch (serverName)
        {
            case "루페온":
                return 0;
            case "실리안":
                return 1;
            case "아만":
                return 2;
            case "카마인":
                return 3;
            case "카제로스":
                return 4;
            case "아브렐슈드":
                return 5;
            case "카단":
                return 6;
            case "니나브":
                return 7;
            default:
                return 0;
        }
    }

    public static string GetItemLevel(string name)
    {
        return (_Tables["CharacterTable"] as CharacterTable).ItemLevels[name];
    }

    public static string GetGuildName(string name)
    {
        return (_Tables["CharacterTable"] as CharacterTable).GuildNames[name];
    }

    public static void AddContentData(string name, ContentData contentData)
    {
        var table = _Tables["ContentTable"] as ContentTable;

        table.ContentDatas.Add(contentData);

        table.Save();
    }

    public static void RemoveContentData(string name, ContentType contentType)
    {
        var table = _Tables["ContentTable"] as ContentTable;
        var contentData = table.ContentDatas.Find(x => x.Name == name && x.Type == contentType);

        if (contentData != null)
        {
            table.ContentDatas.Remove(contentData);
            table.Save();
        }
    }

    public static void SetCharacterNumber(string name, int number)
    {
        (_Tables["CharacterTable"] as CharacterTable).SetCharacterNumber(name, number);
    }

    public static int GetCharacterNumber(string name)
    {
        return (_Tables["CharacterTable"] as CharacterTable).GetCharacterNumber(name);
    }

    public static List<ContentData> GetContentDatas(string name)
    {
        var table = _Tables["ContentTable"] as ContentTable;

        return table.ContentDatas.FindAll(x => x.Name == name);
    }

    public static List<ContentData> GetContentDatas()
    {
        var table = _Tables["ContentTable"] as ContentTable;

        return table.ContentDatas;
    }

    public static ContentData GetContentData(string name, ContentType contentType)
    {
        var table = _Tables["ContentTable"] as ContentTable;

        return table.ContentDatas.Find(x => x.Name == name && x.Type == contentType);
    }
}
