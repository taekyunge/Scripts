using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleCursor : MonoBehaviour {

    private Image _CursorImage;

    public void Awake()
    {
        _CursorImage = GetComponent<Image>();
    }

    public void OnEnable()
    {
        if (Puzzle.Instance.ClickedItem != null)
        {
            transform.position = Puzzle.Instance.ClickedItem.transform.position;
            _CursorImage.sprite = Puzzle.Instance.ClickedItem.ItemSprite;
            _CursorImage.SetNativeSize();

            transform.localScale = Vector3.one * 1.2f;
        }
        else
            gameObject.SetActive(false);
    }

    public void OnDisable()
    {
        // 퍼즐 계산
        Puzzle.Instance.Finished();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Gem")
        {
            Puzzle.Instance.ChangeItem(collision.GetComponent<PuzzleItem>());
        }
    }

    void Update()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = worldPoint;        
    }
}
