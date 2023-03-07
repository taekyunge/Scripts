using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMgr : Singleton<BlockMgr>
{
    [SerializeField]
    private Transform _Root = null;

    [SerializeField]
    private BaseBlock _BaseBlock = null;

    private Pooling<BaseBlock> _Pooling;

    private void Start()
    {
        _Pooling = new Pooling<BaseBlock>(10, _BaseBlock, _Root);
    }

    public BaseBlock CreateBlock(Vector3 startPos, Vector3 targetPos, int number, float speed)
    {
        var baseBlock = _Pooling.Get();

        baseBlock.Set(startPos, targetPos, number, speed);

        return baseBlock;
    }

    public void DeleteBlock(BaseBlock baseBlock)
    {
        _Pooling.Delete(baseBlock);
    }
}
