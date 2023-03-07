using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SpriteMgr : Singleton<SpriteMgr>
{
    [SerializeField]
    private List<Sprite> _Sprites = new List<Sprite>();

    public Sprite GetSprite(string spriteName)
    {
        return _Sprites.Find(x => x.name == spriteName);
    }

    public void GetSprite(string url, Image image)
    {
        if(_Sprites.Exists(x => x.name == url))
        {
            image.sprite = GetSprite(url);
        }
        else
        {
            StartCoroutine(GetTexture(url, image));
        }
    }

    IEnumerator GetTexture(string url, Image image)
    {
        var www = UnityWebRequestTexture.GetTexture(url);

        yield return www.SendWebRequest();

        if (!www.isHttpError && !www.isNetworkError)
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            Rect rect = new Rect(0, 0, texture.width, texture.height);

            var sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));

            sprite.name = url;
            image.sprite = sprite;

            _Sprites.Add(sprite);
        }
    }
}
