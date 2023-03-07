using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ContentResetType
{
    Day,
    Weekly,
    
    NONE,
}

public enum ContentType
{
    유령선,
    길드_토벌전,
    도전_가디언_토벌,
    도전_어비스_던전,
    오레하의_유물_노말,
    오레하의_유물_하드,
    아르고스,
    발탄_노말,
    발탄_하드,
    비아키스_노말,
    비아키스_하드,
    쿠크세이튼_노말,
    쿠크세이튼_리허설,
    아브렐슈드_노말_1a2관문,
    아브렐슈드_노말_1a4관문,
    아브렐슈드_노말_1a6관문,
    아브렐슈드_하드_1a2관문,
    아브렐슈드_하드_1a4관문,
    아브렐슈드_하드_1a6관문,
    아브렐슈드_리허설,
    카양겔_노말,
    카양겔_하드_1,
    카양겔_하드_2,
    카양겔_하드_3,
    일리아칸_노말,
    일리아칸_하드,
    일리아칸_에피데믹,

    카오스_던전,
    가디언_토벌,
    에포나_의뢰,
    
    NONE,
}

public class ContentData
{
    public string Name = string.Empty;

    public ContentType Type;
    public ContentResetType ResetType
    {
        get
        {
            switch (Type)
            {
                case ContentType.카오스_던전:
                case ContentType.가디언_토벌:
                case ContentType.에포나_의뢰:
                case ContentType.NONE:
                    return ContentResetType.Day;
                default:
                    return ContentResetType.Weekly;
            }
        }
    }

    public bool IsClear;
    public bool IsMore;
    public int ClearTime;
}
