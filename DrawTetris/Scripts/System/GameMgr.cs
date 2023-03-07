using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : Singleton<GameMgr>
{
    [SerializeField]
    private List<MapBlock> _Map = new List<MapBlock>();

    public static float BlockSpeed = 0.0f;

    private Vector2 MapSize;

    private void Start()
    {
        BlockSpeed = Define.BlockSpeed;

        MapSize.x = 10;
        MapSize.y = _Map.Count / 10;

        for (int i = 0; i < MapSize.y; i++)
        {
            var lineCheck = gameObject.AddComponent<LineCheck>();

            lineCheck.LineNumber = i;

            for (int j = 0; j < MapSize.x; j++)
            {
                var mapBlock = _Map[j + i * 10];

                mapBlock.ID = j + i * 10;

                lineCheck.AddBlock(mapBlock);
            }
        }
    }

    public void DropBlock(int lineNumber)
    {
        for (int i = lineNumber * 10 - 1; i >= 0; i--)
        {
            if (_Map[i].Block)
            {
                var startBlock = _Map[i];
                var targetBlock = _Map[i + 10];
                var block = _Map[i].Block;

                _Map[i].Block = null;
                targetBlock.Block = block;
                block.Set(startBlock.transform.position, targetBlock.transform.position, BlockSpeed);
            }
        }
    }

    public void CreateBlocks(List<TouchObj> touchObjs)
    {
        int repeat = CalculateRepeat(touchObjs);
        int randomNumber = Random.Range(0, 7);

        for (int i = 0; i < touchObjs.Count; i++)
        {
            var startBlock = _Map[touchObjs[i].ID];
            var targetBlock = _Map[touchObjs[i].ID + (10 * repeat)];

            targetBlock.Block = BlockMgr.Instance.CreateBlock(startBlock.transform.position, targetBlock.transform.position, randomNumber, BlockSpeed);
        }
    }

    private int CalculateRepeat(List<TouchObj> touchObjs)
    {
        int repeat = -1;

        for (int i = 0; i < touchObjs.Count; i++)
        {
            var id = touchObjs[i].ID;
            var count = 0;

            for (; count < MapSize.y; count++)
            {
                var target = id + (count * 10);

                if (target >= _Map.Count || (target < _Map.Count && _Map[target].Block))
                    break;
            }

            if (repeat < 0 || repeat > count - 1)
                repeat = count - 1;
        }

        return repeat;
    }
}
