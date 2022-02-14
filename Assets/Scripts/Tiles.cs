using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tiles : MonoBehaviour
{
    private SpriteRenderer rend;
    public Sprite[] tileGraphics;

    public float hoverAmount;

    public LayerMask obstacleLayer;

    public Color hightlitedColor;
    public Color createbleColor;

    public bool isWalkable;
    public bool isCreateble;
    GameManager gm;
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        int randTile = Random.Range(0, tileGraphics.Length);
        rend.sprite = tileGraphics[randTile];
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseEnter()
    {
        transform.localScale += Vector3.one * hoverAmount;
    }
    private void OnMouseExit()
    {
        transform.localScale -= Vector3.one * hoverAmount;
    }
    public bool isClear()
    {
        Collider2D obstacle = Physics2D.OverlapCircle(transform.position, 0.2f, obstacleLayer);
        if (obstacle != null)
        {
            return false;
        }     
        else
        return true;
    }
    public void HighLite()
    {
        rend.color = hightlitedColor;
        isWalkable = true;
    }
    public void Reset()
    {
        rend.color = Color.white;
        isWalkable = false;
        isCreateble = false;
    }
    void OnMouseDown()
    {
        if (isWalkable && gm.selectedUnits!=null)
        {
            gm.selectedUnits.Move(this.transform.position);
        }else if (isCreateble)
        {
            BarrakItem item = Instantiate(gm.purchasedItem, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            gm.ResetTiles();
            Unit unit = item.GetComponent<Unit>();
            if (unit != null)
            {
                unit.hasAttacked = true;
                unit.hasMoved = true;
            }
        }
    }
    public void SetCreatable()
    {
        rend.color = Color.red;
        isCreateble = true;
    }
}
