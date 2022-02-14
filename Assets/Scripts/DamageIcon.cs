using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIcon : MonoBehaviour
{
    public Sprite[] damageSprite;
    public float lifetime;

    public GameObject effect;
    private void Start()
    {
        Invoke("Destruction", lifetime);
    }
    public void Setup(int damage)
    {
        GetComponent<SpriteRenderer>().sprite = damageSprite[damage - 1];
    }
    void Destruction()
    {
        Instantiate(effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
