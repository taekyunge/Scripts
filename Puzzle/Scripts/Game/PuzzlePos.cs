using UnityEngine;
using UnityStandardAssets.Effects;

public class PuzzlePos : MonoBehaviour {

    public PuzzleItem _ChildItem = null;
    public PuzzleItem ChildItem
    {
        set
        {
            _ChildItem = value;

            if(_ChildItem != null)
            {
                _ChildItem.transform.SetParent(transform);
                _ChildItem.MyPos = this;
            }
        }
        get
        {
            return _ChildItem;
        }
    }

    public ParticleController Effect;

    public int X;
    public int Y;

    public string GetKey()
    {
        return string.Format("{0}-{1}", X, Y);
    }

    public string GetPosLog()
    {
        return string.Format("GemPos x : {0} y : {1}", X, Y);
    }
}
