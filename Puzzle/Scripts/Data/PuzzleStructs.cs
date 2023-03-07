using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CompleteItem
{
    // 완성된 잼
    public List<PuzzleItem> PuzzleItemList = new List<PuzzleItem>();

    public CompleteItem GetClone()
    {
        CompleteItem item = new CompleteItem();

        foreach (var i in PuzzleItemList)
        {
            item.PuzzleItemList.Add(i);
        }

        return item;
    }
}