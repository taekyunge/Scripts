using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTable : Table
{
    public Dictionary<string, string> ServerNames = new Dictionary<string, string>();
    public Dictionary<string, string> ItemLevels = new Dictionary<string, string>();
    public Dictionary<string, string> GuildNames = new Dictionary<string, string>();
    public Dictionary<string, bool> Gold = new Dictionary<string, bool>();
    public Dictionary<string, int> CharacterNumber = new Dictionary<string, int>();

    public override void Load()
    {
        ServerNames.Clear();
        ItemLevels.Clear();
        GuildNames.Clear();
        Gold.Clear();
        CharacterNumber.Clear();

        int count = PlayerPrefs.GetInt("ServerNameCount");

        for (int i = 0; i < count; i++)
        {
            string key = PlayerPrefs.GetString(string.Format("ServerName-{0}", i));
            string value = PlayerPrefs.GetString(string.Format("ServerName-{0}-{1}", i, key));

            ServerNames.Add(key, value);
        }

        count = PlayerPrefs.GetInt("ItemLevelCount");

        for (int i = 0; i < count; i++)
        {
            string key = PlayerPrefs.GetString(string.Format("ItemLevel-{0}", i));
            string value = PlayerPrefs.GetString(string.Format("ItemLevel-{0}-{1}", i, key));

            ItemLevels.Add(key, value);
        }

        count = PlayerPrefs.GetInt("GuildNameCount");

        for (int i = 0; i < count; i++)
        {
            string key = PlayerPrefs.GetString(string.Format("GuildName-{0}", i));
            string value = PlayerPrefs.GetString(string.Format("GuildName-{0}-{1}", i, key));

            GuildNames.Add(key, value);
        }

        count = PlayerPrefs.GetInt("GoldCount");

        for (int i = 0; i < count; i++)
        {
            string key = PlayerPrefs.GetString(string.Format("Gold-{0}", i));
            int value = PlayerPrefs.GetInt(string.Format("Gold-{0}-{1}", i, key));

            Gold.Add(key, value == 1 ? true : false);
        }

        count = PlayerPrefs.GetInt("CharacterNumberCount");

        for (int i = 0; i < count; i++)
        {
            string key = PlayerPrefs.GetString(string.Format("CharacterNumber-{0}", i));
            int value = PlayerPrefs.GetInt(string.Format("CharacterNumber-{0}-{1}", i, key));

            CharacterNumber[key] = value;
        }
    }

    public override void Save()
    {
        PlayerPrefs.SetInt("ServerNameCount", ServerNames.Count);

        int i = 0;

        foreach (var table in ServerNames)
        {
            PlayerPrefs.SetString(string.Format("ServerName-{0}", i), table.Key);
            PlayerPrefs.SetString(string.Format("ServerName-{0}-{1}", i++, table.Key), table.Value);
        }

        PlayerPrefs.SetInt("ItemLevelCount", ItemLevels.Count);

        i = 0;

        foreach (var table in ItemLevels)
        {
            PlayerPrefs.SetString(string.Format("ItemLevel-{0}", i), table.Key);
            PlayerPrefs.SetString(string.Format("ItemLevel-{0}-{1}", i++, table.Key), table.Value);
        }

        PlayerPrefs.SetInt("GuildNameCount", GuildNames.Count);

        i = 0;

        foreach (var table in GuildNames)
        {
            PlayerPrefs.SetString(string.Format("GuildName-{0}", i), table.Key);
            PlayerPrefs.SetString(string.Format("GuildName-{0}-{1}", i++, table.Key), table.Value);
        }

        PlayerPrefs.SetInt("GoldCount", Gold.Count);

        i = 0;

        foreach (var table in Gold)
        {
            PlayerPrefs.SetString(string.Format("Gold-{0}", i), table.Key);
            PlayerPrefs.SetInt(string.Format("Gold-{0}-{1}", i++, table.Key), table.Value ? 1 : 0);
        }

        PlayerPrefs.SetInt("CharacterNumberCount", CharacterNumber.Count);

        i = 0;

        foreach (var table in CharacterNumber)
        {
            PlayerPrefs.SetString(string.Format("CharacterNumber-{0}", i), table.Key);
            PlayerPrefs.SetInt(string.Format("CharacterNumber-{0}-{1}", i++, table.Key), table.Value);
        }
    }

    public void SetGold(string name, bool value)
    {
        Gold[name] = value;

        Save();
    }

    public void SetCharacterNumber(string name, int number)
    {
        CharacterNumber[name] = number;

        Save();
    }

    public int GetCharacterNumber(string name)
    {
        return !CharacterNumber.ContainsKey(name) ? -1 : CharacterNumber[name];
    }

    public bool IsCharacter(string name)
    {
        return ServerNames.ContainsKey(name);
    }

    public bool IsGold(string name)
    {
        return !Gold.ContainsKey(name) ? false : Gold[name];
    }
}
