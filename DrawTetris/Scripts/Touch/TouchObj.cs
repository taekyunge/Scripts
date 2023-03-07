using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchObj : MonoBehaviour
{
    public int ID = 0;

    public Vector3 Pos
    {
        get { return transform.position; }
    }

    private void Start()
    {
        ID = transform.GetSiblingIndex() - 1;
    }
}
