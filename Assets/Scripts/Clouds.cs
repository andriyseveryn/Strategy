using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    public float speed = 0;
    void Update()
    {
        transform.position += new Vector3(1 * Time.deltaTime * speed, 0, 0);
        if (transform.position.x >= 14)
        { 
            transform.position = new Vector3(-12, transform.position.y, transform.position.z);
        }
    }
}
