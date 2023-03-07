using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentGroup : MonoBehaviour
{
    [SerializeField]
    private ContentItem _BaseObject = null;

    [SerializeField]
    private Transform _Root = null;

    private Pooling<ContentItem> _Pooling = null;

    private List<ContentItem> _UsingItems = new List<ContentItem>();

    public void SetContent(string name, CharacterItem characterItem)
    {
        if (_Pooling == null)
            _Pooling = new Pooling<ContentItem>(1, _BaseObject, _Root);

        for (int i = 0; i < _UsingItems.Count; i++)
        {
            _Pooling.Delete(_UsingItems[i]);
        }
        
        _UsingItems.Clear();

        var contentDatas = LocalDB.GetContentDatas(name);
        int count = (contentDatas.Count > 12) ? 12 : contentDatas.Count;

        for (int i = 0; i < count; i++)
        {
            var item = _Pooling.Get();

            item.transform.SetAsLastSibling();
            item.SetItem(contentDatas[i], characterItem);

            _UsingItems.Add(item);
        }
    }
}
