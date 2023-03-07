using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentItem : MonoBehaviour
{
    [SerializeField]
    private Image _ContentImage = null;

    [SerializeField]
    private Image _MoreImage = null;

    private CharacterItem _CharacterItem = null;

    private ContentData _ContentData = null;

    private Popup _Help = null;

    public void SetItem(ContentData contentData, CharacterItem characterItem)
    {
        _CharacterItem = characterItem;
        _ContentData = contentData;
        _ContentImage.sprite = SpriteMgr.Instance.GetSprite(GetSpriteName(contentData.Type));
        _ContentImage.color = contentData.IsClear ? new Color(0.25f, 0.25f, 0.25f, 1) : Color.white;
        _MoreImage.gameObject.SetActive(contentData.IsMore);
        _MoreImage.color = contentData.IsClear ? new Color(0.25f, 0.25f, 0.25f, 1) : new Color(1, 0.84f, 0);

        _ContentImage.SetNativeSize();
    }

    private string GetSpriteName(ContentType type)
    {
        switch (type)
        {
            case ContentType.유령선:
                return "Icon_36";
            case ContentType.길드_토벌전:
                return "Icon_37";
            case ContentType.도전_가디언_토벌:
                return "Icon_8";
            case ContentType.도전_어비스_던전:
                return "Icon_14";
            case ContentType.오레하의_유물_노말:
            case ContentType.오레하의_유물_하드:
            case ContentType.카양겔_노말:
            case ContentType.카양겔_하드_1:
            case ContentType.카양겔_하드_2:
            case ContentType.카양겔_하드_3:
                return "Icon_13";
            case ContentType.아르고스:
                return "Icon_9";
            case ContentType.발탄_노말:
                return "Icon_11";
            case ContentType.발탄_하드:
                return "Icon_11";
            case ContentType.비아키스_노말:
                return "Icon_11";
            case ContentType.비아키스_하드:
                return "Icon_11";
            case ContentType.쿠크세이튼_노말:
                return "Icon_11";
            case ContentType.쿠크세이튼_리허설:
                return "Icon_12";
            case ContentType.아브렐슈드_노말_1a2관문:
            case ContentType.아브렐슈드_노말_1a4관문:
            case ContentType.아브렐슈드_노말_1a6관문:
                return "Icon_11";
            case ContentType.아브렐슈드_하드_1a2관문:
            case ContentType.아브렐슈드_하드_1a4관문:
            case ContentType.아브렐슈드_하드_1a6관문:
                return "Icon_11";
            case ContentType.아브렐슈드_리허설:
                return "Icon_120";
            case ContentType.카오스_던전:
                return "Icon_7";
            case ContentType.가디언_토벌:
                return "Icon_8";
            case ContentType.에포나_의뢰:
                return "Icon_22";

            case ContentType.일리아칸_노말:
            case ContentType.일리아칸_하드:
            case ContentType.일리아칸_에피데믹:
                return "Icon_11";

            default:
                return "Icon_29";
        }
    }

    public void OnPointerEnter()
    {
        if(_Help == null)
        {
            HelpData helpData = new HelpData();
            GoldData goldData = LocalDB.GetGoldData(_ContentData.Type);
            float itemLevel = float.Parse(LocalDB.GetItemLevel(_CharacterItem.Name));

            string message = string.Format("{0}", _ContentData.Type.ToString().Replace('_', ' ').Replace('a', '~'));

            if (goldData != null)
            {
                message += (goldData.Start_Level >  0) ? string.Format(" Lv.{0}▲\n", goldData.Start_Level) : string.Empty;
                message += string.Format("[보상:{0}G]\n", goldData.Gold);
                message += (goldData.More_Gold > 0) ? string.Format("[더보기:{0}G]", goldData.More_Gold) : string.Empty;

                if ((goldData.Start_Level > 0 && goldData.Start_Level > itemLevel) || (goldData.End_Level > 0 && goldData.End_Level <= itemLevel))
                {
                    message += "\n<color=#FA451E>획득불가</color>";
                }
            }

            helpData.Message = message;
            helpData.Pos = transform.position;

            _Help = PopupMgr.Instance.Open(PopupType.Help, helpData);
        }        
    }

    public void OnPointerExit()
    {
        if (_Help != null)
        {
            PopupMgr.Instance.Close(PopupType.Help);
            _Help = null;
        }
    }

    public void OnClickItem()
    {
        _ContentData.IsClear = !_ContentData.IsClear;

        if(_ContentData.IsClear)
        {
            _ContentData.ClearTime = Utill.ConvertToUnixTimestamp(System.DateTime.Now);
        }

        SetItem(_ContentData, _CharacterItem);
        _CharacterItem.SetGold();
        SchedulerMgr.Instance.SetGold();

        LocalDB.Save();
    }
}
