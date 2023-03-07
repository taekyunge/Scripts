using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellSize : MonoBehaviour
{
    public float MinWidth = 0;

    private int Width = 0;

    private GridLayoutGroup _Layout = null;

    private void Start()
    {
        _Layout = GetComponent<GridLayoutGroup>();

        Width = Screen.width;

        Fit();
    }

    // Update is called once per frame
    void Update()
    {
        if(Screen.width != Width)
        {
            Width = Screen.width;

            Fit();
        }
    }

    private void Fit()
    {
        if(_Layout != null)
        {
            var cellSize = _Layout.cellSize;

            cellSize.x = (float)(Width - 15) / 2;

            if (cellSize.x < MinWidth)
                cellSize.x = MinWidth;

            _Layout.cellSize = cellSize;
        }
    }
}
