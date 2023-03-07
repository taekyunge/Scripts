using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CharacterItem : MonoBehaviour
{
    [SerializeField]
    private EventTrigger _EventTrigger = null;

    [SerializeField]
    private ContentGroup _ContentGroup = null;

    [SerializeField]
    private Image _CharacterImage = null;

    [SerializeField]
    private Image _PointerImage = null;

    [SerializeField]
    private Text _GuildText = null;

    [SerializeField]
    private Text _NameText = null;

    [SerializeField]
    private Text _ServerText = null;

    [SerializeField]
    private Text _JobText = null;

    [SerializeField]
    private Text _LevelText = null;

    [SerializeField]
    private Text _ItemLevelText = null;

    [SerializeField]
    private Text _GoldText = null;

    [SerializeField]
    private Graphic[] Graphics = null;

    private CharacterData _CharacterData = null;

    private bool _Gold = false;

    public bool _Drag
    {
        set
        {
            for (int i = 0; i < Graphics.Length; i++)
            {
                Graphics[i].raycastTarget = !value;
            }
        }
    }

    public string Name { get { return _CharacterData.Name; } }

    private void Update()
    {
        if (_Gold)
        {
            var color = _NameText.color;

            color.a = Mathf.PingPong(Time.time * 2f, 1);

            _NameText.color = color;
        }

        _PointerImage.gameObject.SetActive(CharacterScroll.SwapItem != null && CharacterScroll.SwapItem == this);
        _EventTrigger.enabled = !SchedulerMgr.Lock;
    }

    public void SetItem(CharacterData  characterData)
    {
        _CharacterData = characterData;

        _NameText.text = characterData.Name;
        _JobText.text = characterData.Job;
        _LevelText.text = characterData.Level;
        _ServerText.text = LocalDB.GetServerName(characterData.Name);
        _ItemLevelText.text = LocalDB.GetItemLevel(characterData.Name);
        _GuildText.text = LocalDB.GetGuildName(characterData.Name);
        _Gold = LocalDB.IsGold(characterData.Name);
        _ContentGroup.SetContent(characterData.Name, this);

        SetGold();

        SpriteMgr.Instance.GetSprite(characterData.Url, _CharacterImage);
    }

    public void SetGold()
    {
        int gold = 0;
        int useGold = 0;
        bool isGold = LocalDB.IsGold(_CharacterData.Name);

        if (isGold)
        {
            var contentDatas = LocalDB.GetContentDatas(_CharacterData.Name);
            float itemLevel = float.Parse(LocalDB.GetItemLevel(_CharacterData.Name));
            int count = (contentDatas.Count > 12) ? 12 : contentDatas.Count;

            for (int i = 0; i < count; i++)
            {
                GoldData goldData = LocalDB.GetGoldData(contentDatas[i].Type);

                if (goldData != null && (goldData.Start_Level <= itemLevel || goldData.Start_Level == 0) && (goldData.End_Level > itemLevel || goldData.End_Level == 0))
                {
                    gold += (contentDatas[i].IsMore) ? goldData.Gold - goldData.More_Gold : goldData.Gold;
                    useGold += (contentDatas[i].IsClear) ? (contentDatas[i].IsMore) ? goldData.Gold - goldData.More_Gold : goldData.Gold : 0;
                }
            }
        }

        _GoldText.text = gold < 0 ? string.Format("{0:#,0}G", gold) : string.Format("{0:#,0}G / {1:#,0}G", useGold, gold);
        _GoldText.gameObject.SetActive(isGold);
        _NameText.color = _Gold ? new Color(1, 0.84f, 0) : Color.white;
    }

    public void SaveNumber()
    {
        _CharacterData.Number = transform.GetSiblingIndex();
        LocalDB.SetCharacterNumber(_CharacterData.Name, transform.GetSiblingIndex());
    }

    public void OnClickEditor()
    {
        PopupMgr.Instance.Open(PopupType.Editor, _CharacterData.Name);
    }

    public void OnClickRefresh()
    {
        SchedulerMgr.Instance.SearchUser(_CharacterData.Name);
    }
}
