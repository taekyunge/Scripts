using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMgr : Singleton<PopupMgr>
{
    [SerializeField]
    private Transform _Root = null;

    [SerializeField]
    private GameObject _Background = null;

    private Dictionary<PopupType, Pooling<Popup>> _PopupPooling = new Dictionary<PopupType, Pooling<Popup>>();

    private List<Popup> _OpenPopups = new List<Popup>();

    protected override void Awake()
    {
        base.Awake();

        transform.localPosition = Vector3.zero;
    }

    private void Start()
    {
        Popup[] popups = GetComponentsInChildren<Popup>();

        for (int i = 0; i < popups.Length; i++)
        {
            var popup = popups[i];

            Pooling<Popup> pooling = new Pooling<Popup>(1, popup, _Root);

            _PopupPooling.Add(popup.Type, pooling);
        }
    }

    private void SetBackground()
    {
        for (int i = _OpenPopups.Count - 1; i >= 0; i--)
        {
            var popup = _OpenPopups[i];

            if (popup.Background)
            {
                _Background.SetActive(true);
                _Background.transform.SetSiblingIndex(popup.transform.GetSiblingIndex() - 1);
                return;
            }
        }

        _Background.transform.SetAsFirstSibling();
        _Background.SetActive(false);
    }

    public Popup Open(PopupType popupType, object obj = null)
    {
        if (!_PopupPooling.ContainsKey(popupType))
            return null;

        var popupPooling = _PopupPooling[popupType];    
        var popup = popupPooling.Get();

        if(popup.OnlyOnce)
            Close(popup.Type);

        popup.Open(obj);
        popup.transform.SetAsLastSibling();

        _OpenPopups.Add(popup);

        SetBackground();

        return popup;
    }

    public void Close(Popup popup)
    {
        PopupType type = popup.Type;

        _PopupPooling[type].Delete(popup);
        _OpenPopups.Remove(popup);

        SetBackground();
    }

    public void Close(PopupType popupType)
    {
        for (int i = _OpenPopups.Count - 1; i >= 0 ; i--)
        {
            if(_OpenPopups[i].Type == popupType)
                _OpenPopups[i].Close();
        }
    }
}
