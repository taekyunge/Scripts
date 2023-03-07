using HtmlAgilityPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SchedulerMgr : Singleton<SchedulerMgr>
{
    public static int CurrentIndex = 0;

    public static bool Lock = true;

    [SerializeField]
    private Text _GoldText = null;

    [SerializeField]
    private UserScroll _UserScroll = null;

    [SerializeField]
    private Image _LockImage = null;

    [SerializeField]
    private Image _LockBackImage = null;

    [SerializeField]
    private CharacterScroll _CharacterScroll = null;

    public List<string> UserNames = null;

    public List<CharacterData> CharacterDatas = new List<CharacterData>();

    private void Start()
    {
        base.Awake();

        LocalDB.Initialize();

        UserNames = LocalDB.GetUserData();

        _UserScroll.Refresh();
        _CharacterScroll.Refresh();

        _LockImage.sprite = SpriteMgr.Instance.GetSprite(Lock ? "lock" : "unlock");
        _LockBackImage.gameObject.SetActive(!Lock);

        SelectName((UserNames.Count > 0) ? 0 : -1);
    }

    public void SelectName(int index)
    {
        CharacterDatas.Clear();

        CurrentIndex = index;

        if (CurrentIndex < 0)
        {
            _UserScroll.Refresh();
            _CharacterScroll.Refresh();
        }
        else
        {
            StartCoroutine(SearchUser());
        }
    }

    public void AddUserName(string name)
    {
        UserNames.Add(name);

        _UserScroll.Refresh();

        if(CurrentIndex < 0)
        {
            SelectName(0);
        }

        LocalDB.Save();
    }

    public void RemoveUserName(int index)
    {
        UserNames.RemoveAt(index);

        _UserScroll.Refresh();

        if (CurrentIndex == index)
        {
            SelectName(UserNames.Count > 0 ? 0 : -1);
        }
        else if(CurrentIndex > index)
        {
            CurrentIndex--;
        }

        LocalDB.Save();
    }

    public void Refresh()
    {
        _UserScroll.Refresh();
        _CharacterScroll.Refresh();

        SetGold();
    }

    public void SetGold()
    {
        int gold = 0;
        int useGold = 0;

        for (int i = 0; i < CharacterDatas.Count; i++)
        {
            if(LocalDB.IsGold(CharacterDatas[i].Name))
            {
                var contentDatas = LocalDB.GetContentDatas(CharacterDatas[i].Name);
                float itemLevel = float.Parse(LocalDB.GetItemLevel(CharacterDatas[i].Name));
                int count = (contentDatas.Count > 12) ? 12 : contentDatas.Count;

                for (int j = 0; j < count; j++)
                {
                    ContentType contentType = contentDatas[j].Type;
                    GoldData goldData = LocalDB.GetGoldData(contentType);

                    if (goldData != null && (goldData.Start_Level <= itemLevel || goldData.Start_Level == 0) && (goldData.End_Level > itemLevel || goldData.End_Level == 0))
                    {
                        gold += (contentDatas[j].IsMore) ? goldData.Gold - goldData.More_Gold : goldData.Gold;
                        useGold += (contentDatas[j].IsClear) ? (contentDatas[j].IsMore) ? goldData.Gold - goldData.More_Gold : goldData.Gold : 0;                      
                    }
                }
            }
        }

        _GoldText.text = (gold < 0) ? string.Format("{0:#,0}G", gold) : string.Format("{0:#,0}G / {1:#,0}G", useGold, gold);
    }

    private IEnumerator SearchUser()
    {
        var loading = PopupMgr.Instance.Open(PopupType.Loading);

        yield return new WaitForSeconds(0.25f);

        string url = string.Format("https://lostark.game.onstove.com/Profile/Character/{0}", UserNames[CurrentIndex]);

        HtmlWeb web = new HtmlWeb();

        HtmlDocument doc = web.Load(url);

        var serverProfile = doc.DocumentNode.SelectNodes("//ul[@class='profile-character-list__char']");

        if (serverProfile == null)
        {
            PopupMgr.Instance.Open(PopupType.Message, string.Format("캐릭터 정보가 없습니다.\n캐릭터명을 확인해 주세요."));
        }
        else
        {
            for (int i = 0; i < serverProfile.Count; i++)
            {
                var nodes = serverProfile[i].SelectNodes("li");

                foreach (HtmlNode node in nodes)
                {
                    var selectNode = node.SelectSingleNode("span").SelectSingleNode("button");

                    if (selectNode.ChildNodes.Count == 5)
                    {
                        var characterData = new CharacterData
                        {
                            Job = selectNode.SelectSingleNode("img").Attributes["alt"].Value.Trim(),
                            Url = selectNode.SelectSingleNode("img").Attributes["src"].Value.Trim(),
                            Level = selectNode.ChildNodes[2].InnerText.Trim(),
                            Name = selectNode.ChildNodes[3].InnerText.Trim()
                        };

                        characterData.Number = LocalDB.GetCharacterNumber(characterData.Name);

                        CharacterDatas.Add(characterData);

                        if (!LocalDB.IsCharacterData(characterData.Name))
                        {
                            yield return StartCoroutine(SearchCharacter(characterData.Name));
                        }
                    }

                    yield return null;
                }
            }

            TimeMgr.Instance.Initialize();
        }

        CharacterDatas.Sort((x, y) =>
        {
            if (x.Number > y.Number)
                return 1;
            else if (x.Number < y.Number)
                return -1;
            else
                return 0;
        });

        _CharacterScroll.Refresh();

        SetGold();

        PopupMgr.Instance.Close(loading);
    }

    public void SearchUser(string name)
    {
        StartCoroutine(SearchCharacter(name, true));
    }

    private IEnumerator SearchCharacter(string name, bool refresh = false)
    {
        if (refresh)
        {
            var loading = PopupMgr.Instance.Open(PopupType.Loading);

            yield return new WaitForSeconds(0.25f);

            string url = string.Format("https://lostark.game.onstove.com/Profile/Character/{0}", name);

            HtmlWeb web = new HtmlWeb();

            HtmlDocument doc = web.Load(url);

            var serverNode = doc.DocumentNode.SelectSingleNode("//span[@class='profile-character-info__server']");
            var itemNode = doc.DocumentNode.SelectSingleNode("//div[@class='level-info2__expedition']").SelectNodes("span");
            var guildNode = doc.DocumentNode.SelectSingleNode("//div[@class='game-info']").SelectSingleNode("//div[@class='game-info__guild']").SelectNodes("span");

            if (serverNode != null && serverNode != null && serverNode != null)
                LocalDB.SetCharacterData(name, serverNode.InnerText.Replace("@", ""), itemNode[1].InnerText.Replace("Lv.", ""), guildNode[1].InnerText);

            Refresh();

            PopupMgr.Instance.Close(loading);
        }
        else
        {
            yield return new WaitForSeconds(0.25f);

            string url = string.Format("https://lostark.game.onstove.com/Profile/Character/{0}", name);

            HtmlWeb web = new HtmlWeb();

            HtmlDocument doc = web.Load(url);

            var serverNode = doc.DocumentNode.SelectSingleNode("//span[@class='profile-character-info__server']");
            var itemNode = doc.DocumentNode.SelectSingleNode("//div[@class='level-info2__expedition']").SelectNodes("span");
            var guildNode = doc.DocumentNode.SelectSingleNode("//div[@class='game-info']").SelectSingleNode("//div[@class='game-info__guild']").SelectNodes("span");

            if (serverNode != null && serverNode != null && serverNode != null)
                LocalDB.SetCharacterData(name, serverNode.InnerText.Replace("@", ""), itemNode[1].InnerText.Replace("Lv.", ""), guildNode[1].InnerText);
        }
    }

    public void ContentClear(ContentResetType type)
    {
        var contentDatas = LocalDB.GetContentDatas();

        for (int i = 0; i < contentDatas.Count; i++)
        {
            if (contentDatas[i].ResetType == type)
                contentDatas[i].IsClear = false;
        }

        Refresh();
        LocalDB.Save();
    }

    public void OnClickClear()
    {
        ContentClear(ContentResetType.Weekly);
    }

    public void OnClickCreateUser()
    {
        PopupMgr.Instance.Open(PopupType.CreateUser);
    }

    public void OnClickSetting()
    {
        PopupMgr.Instance.Open(PopupType.Setting);
    }

    public void OnClickLink()
    {
        PopupMgr.Instance.Open(PopupType.Link);
    }

    public void OnClickLock()
    {
        Lock = !Lock;

        _LockImage.sprite = SpriteMgr.Instance.GetSprite(Lock ? "lock" : "unlock");
        _LockBackImage.gameObject.SetActive(!Lock);
    }
}
