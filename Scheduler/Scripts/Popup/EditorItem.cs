using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorItem : MonoBehaviour
{
    [SerializeField]
    private Text _ContentText = null;

    [SerializeField]
    private Image _MoreImage = null;

    [SerializeField]
    private Button _MoreButton = null;

    [SerializeField]
    private GameObject _CheckObject = null;

    private ContentData _ContentData = null;

    private GoldData _GoldData = null;

    private string _Name = string.Empty;
    private ContentType _ContentType;

    public void SetItem(string name, ContentType type)
    {
        if(_MoreButton == null)
            _MoreButton = _MoreImage.GetComponent<Button>();

        _Name = name;
        _ContentType = type;

        _GoldData = LocalDB.GetGoldData(type);
        _ContentData = LocalDB.GetContentData(name, type);

        _ContentText.text = type.ToString().Replace('_', ' ').Replace('a', '~');

        _CheckObject.SetActive(_ContentData != null);
        _MoreImage.gameObject.SetActive(!(_GoldData == null) || (_GoldData != null && _GoldData.More_Gold > 0));
        _MoreButton.interactable = (_MoreImage.gameObject.activeSelf && _ContentData != null);

        if (_MoreImage.gameObject.activeSelf)
        {
            _MoreImage.color = (_ContentData != null && _ContentData.IsMore) ? new Color(1, 0.84f, 0) : Color.gray;
        }
    }

    public void OnClickToggle()
    {
        bool value = !_CheckObject.activeSelf;

        _CheckObject.SetActive(value);

        if (value)
        {
            var contentData = new ContentData();

            contentData.Name = _Name;
            contentData.Type = _ContentType;

            _ContentData = contentData;

            LocalDB.AddContentData(_Name, contentData);

        }
        else
        {
            _ContentData = null;

            LocalDB.RemoveContentData(_Name, _ContentType);
        }

        SetItem(_Name, _ContentType);

        SchedulerMgr.Instance.Refresh();
    }

    public void OnMoreGold()
    {
        if(_ContentData != null)
        {
            _ContentData.IsMore = !_ContentData.IsMore;

            LocalDB.Save();

            SetItem(_Name, _ContentType);

            SchedulerMgr.Instance.Refresh();
        }
    }
}
