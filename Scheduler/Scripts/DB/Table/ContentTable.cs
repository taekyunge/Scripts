using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentTable : Table
{
    public List<ContentData> ContentDatas = new List<ContentData>();

    public override void Load()
    {
        ContentDatas.Clear();

        int count = PlayerPrefs.GetInt("ContentDataCount");

        for (int i = 0; i < count; i++)
        {
            var contentData = new ContentData();            

            if(System.Enum.TryParse(PlayerPrefs.GetString(string.Format("ContentType-{0}", i)), out contentData.Type))
            {
                contentData.Name = PlayerPrefs.GetString(string.Format("ContentData-{0}", i));
                contentData.IsClear = PlayerPrefs.GetInt(string.Format("ContentIsClear-{0}", i)) == 0 ? false : true;
                contentData.IsMore = PlayerPrefs.GetInt(string.Format("ContentIsMore-{0}", i)) == 0 ? false : true;
                contentData.ClearTime = PlayerPrefs.GetInt(string.Format("ContentClearTime-{0}", i));

                ContentDatas.Add(contentData);
            }
        }
    }

    public override void Save()
    {
        PlayerPrefs.SetInt("ContentDataCount", ContentDatas.Count);

        for (int i = 0; i < ContentDatas.Count; i++)
        {
            var contentData = ContentDatas[i];

            PlayerPrefs.SetString(string.Format("ContentData-{0}", i), contentData.Name);
            PlayerPrefs.SetString(string.Format("ContentType-{0}", i), contentData.Type.ToString());
            PlayerPrefs.SetInt(string.Format("ContentIsClear-{0}", i), contentData.IsClear ? 1 : 0);
            PlayerPrefs.SetInt(string.Format("ContentIsMore-{0}", i), contentData.IsMore ? 1 : 0);
            PlayerPrefs.SetInt(string.Format("ContentClearTime-{0}", i), contentData.ClearTime);
        }
    }
}
