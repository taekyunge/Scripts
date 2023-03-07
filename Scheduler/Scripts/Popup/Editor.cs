using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Editor : Popup
{
    [SerializeField]
    private ScrollRect _Scroll;
    
    [SerializeField]
    private GameObject _CheckObject = null;

    [SerializeField]
    private EditorItem _BaseObject = null;

    [SerializeField]
    private Transform _Root = null;

    [SerializeField]
    private Text _NameText = null;

    private Pooling<EditorItem> _Pooling = null;

    private List<EditorItem> _UsingItems = new List<EditorItem>();

    private string _Name = string.Empty;

    public override void Open(object obj = null)
    {
        base.Open(obj);

        _Name = (string)obj;
        _NameText.text = _Name;

        if (_Pooling == null)
            _Pooling = new Pooling<EditorItem>(20, _BaseObject, _Root);

        for (int i = 0; i < _UsingItems.Count; i++)
        {
            _Pooling.Delete(_UsingItems[i]);
        }

        _UsingItems.Clear();

        for (int i = 0; i < (int)ContentType.NONE; i++)
        {
            var item = _Pooling.Get();
            ContentType type = (ContentType)i;

            item.transform.SetAsLastSibling();
            item.SetItem(_Name, type);

            _UsingItems.Add(item);
        }

        _CheckObject.SetActive(LocalDB.IsGold(_Name));
        _Scroll.verticalNormalizedPosition = 1;
    }

    public void OnClickGold()
    {
        bool value = !LocalDB.IsGold(_Name);

        _CheckObject.SetActive(value);

        LocalDB.SetGold(_Name, value);

        SchedulerMgr.Instance.Refresh();
    }
}
