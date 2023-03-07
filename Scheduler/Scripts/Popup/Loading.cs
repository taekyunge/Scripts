using Assets.GifAssets.PowerGif;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Loading : Popup
{
    [SerializeField]
    private AnimatedImage _AnimatedImage = null;

    private Gif _Gif = null;

    public override void Open(object obj)
    {
        if(_Gif == null)
        {
            var path = Path.Combine(Application.streamingAssetsPath, "loading.gif");

            if (path == "") return;

            var bytes = File.ReadAllBytes(path);

            _Gif = Gif.Decode(bytes);
        }

        _AnimatedImage.Play(_Gif);
    }

    public override void Close()
    {
        _AnimatedImage.Stop();

        base.Close();
    }
}
