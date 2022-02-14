using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public bool selected;
    GameManager gm;
    public GameObject weaponIcon;
    public GameObject deathEffect;

    public float moveSpeed;

    public int tilesSpeed;
    public int playerNumber;
    public int attackRange;
    public bool hasMoved;
    List<Unit> enemiesInRange=new List<Unit>();
    public bool hasAttacked;
    public int health, damage, defenseDamage,armor;

    public DamageIcon damageIcon;

    private Animator camAnim;

    public Text kingHealth;
    public bool isKing;

    // Start is called before the first frame update
    void Start()
    {
        camAnim = Camera.main.GetComponent<Animator>();
        gm = FindObjectOfType<GameManager>();
        UpdateKingHealth();
    }
    public void UpdateKingHealth()
    {
        if (isKing)
        {
            kingHealth.text = health.ToString();
        }
    }
    private void OnMouseDown()
    {
        ResetWeaponIcon();
        if (selected)
        {
            selected = false;
            gm.selectedUnits = null;
            gm.ResetTiles();
        }
        else
        {
            if (playerNumber == gm.playerTurn)
            {
                if (gm.selectedUnits != null)
                {
                    gm.selectedUnits.selected = false;
                }
                selected = true;
                gm.selectedUnits = this;
                gm.ResetTiles();
                GetEnemies();
                GetWalkableTiles();
            }           
        }

        Collider2D col = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.15f);
        Unit unit = col.GetComponent<Unit>();
        if (gm.selectedUnits != null)
        {
            if (gm.selectedUnits.enemiesInRange.Contains(unit) && !gm.selectedUnits.hasAttacked)
            {
                gm.selectedUnits.Attack(unit);
            }
        }
    }
    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            gm.ToggleStatesPanel(this);
        }
    }
    void Attack(Unit enemy)
    {
        camAnim.SetTrigger("Shake");

        hasAttacked = true;
        int enemyDamage = damage - enemy.armor;
        int mydamage = enemy.defenseDamage - armor;

        if (enemy.damage >= 1)
        {
            DamageIcon instance = Instantiate(damageIcon, enemy.transform.position, Quaternion.identity);
            instance.Setup(enemyDamage);
            enemy.health -= enemyDamage;
            enemy.UpdateKingHealth();
        }
        if (mydamage >= 1)
        {
            DamageIcon instance = Instantiate(damageIcon, transform.position, Quaternion.identity);
            instance.Setup(mydamage);
            health -= mydamage;
            UpdateKingHealth();
        }
        if (enemy.health <= 0)
        {
            Instantiate(deathEffect, enemy.transform.position, Quaternion.identity);
            Destroy(enemy.gameObject);
            GetWalkableTiles();
        }
        if (health <= 0)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            gm.ResetTiles();
            Destroy(this.gameObject);
        }
    }
    void GetEnemies()
    {
        enemiesInRange.Clear();
        foreach(Unit unit in FindObjectsOfType<Unit>())
        {
            if (Mathf.Abs(transform.position.x - unit.transform.position.x) + Mathf.Abs(transform.position.y - unit.transform.position.y) <= attackRange+0.1f)
            {
                if (unit.playerNumber != gm.playerTurn && hasAttacked==false)
                {
                    enemiesInRange.Add(unit);
                    unit.weaponIcon.SetActive(true);
                }
            }
        }
    }
    public void ResetWeaponIcon()
    {
        foreach(Unit unit in FindObjectsOfType<Unit>())
        {
            unit.weaponIcon.SetActive(false);
        }
    }
    void GetWalkableTiles()
    {
        if (hasMoved)
        {
            return;
        }
        foreach(Tiles tile in FindObjectsOfType<Tiles>())
        {
            if (Mathf.Abs(transform.position.x - tile.transform.position.x) + Mathf.Abs(transform.position.y - tile.transform.position.y) <= tilesSpeed)
            {
                if (tile.isClear())
                {
                    tile.HighLite();
                }
            }
        }
    }
    public void Move(Vector2 tilePos)
    {
        gm.ResetTiles();
        StartCoroutine(StartMovement(tilePos));
    }

    IEnumerator StartMovement(Vector2 tilePos)
    {
        while (transform.position.x != tilePos.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(tilePos.x, transform.position.y), moveSpeed * Time.deltaTime);
            yield return null;
        }
        while (transform.position.y != tilePos.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x,tilePos.y), moveSpeed * Time.deltaTime);
            yield return null;
        }
        hasMoved = true;
        ResetWeaponIcon();
        GetEnemies();
    }
}
