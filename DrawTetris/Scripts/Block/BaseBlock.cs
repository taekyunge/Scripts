using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseBlock : MonoBehaviour
{
    [SerializeField]
    private Sprite[] _BlockSprites = null;

    private Vector3 _StartPos = Vector3.zero;
    private Vector3 _TargetPos = Vector3.zero;
    private float _Speed = 0.0f;

    private Image _Image = null;

    private bool _Move = false;

    public bool IsMove
    {
        get
        {
            return _Move;
        }
    }

    public void Clear()
    {
        _StartPos = Vector3.zero;
        _TargetPos = Vector3.zero;
        _Speed = 0;
        _Move = false;
    }

    public void Set(Vector3 startPos, Vector3 targetPos, float speed)
    {
        transform.position = startPos;

        _StartPos = startPos;
        _TargetPos = targetPos;
        _Speed = speed;
        _Move = true;
    }

    public void Set(Vector3 startPos, Vector3 targetPos, int number,float speed)
    {
        if (_Image == null)
            _Image = GetComponent<Image>();

        transform.position = startPos;

        _Image.sprite =  _BlockSprites[number];
        _StartPos = startPos;
        _TargetPos = targetPos;
        _Speed = speed;
        _Move = true;
    }

    void Update()
    {
        if(_Move)
        {
            var pos = transform.position;

            pos.y -= _Speed * Time.deltaTime;
            
            if (_TargetPos.y > pos.y)
            {
                transform.position = _TargetPos;

                _Move = false;
            }
            else
            {
                transform.position = pos;
            }
        }
    }
}
