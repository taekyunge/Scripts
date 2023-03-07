using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCheck : MonoBehaviour
{
    public int LineNumber = -1;

    [SerializeField]
    private List<MapBlock> _Map = new List<MapBlock>();

    public void AddBlock(MapBlock mapBlock)
    {
        _Map.Add(mapBlock);
    }

    public void Update()
    {
        for (int i = 0; i < _Map.Count; i++)
        {
            if(_Map[i].Block == null || _Map[i].Block.IsMove)
            {
                return;
            }
        }

        for (int i = 0; i < _Map.Count; i++)
        {
            BlockMgr.Instance.DeleteBlock(_Map[i].Block);

            _Map[i].Block = null;
        }

        GameMgr.Instance.DropBlock(LineNumber);
    }
}
