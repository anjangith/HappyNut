using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBounds : MonoBehaviour {

    public bool CliffOnLeft = true;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        var enemy = collider.gameObject.GetComponent<Enemy>();
        if(enemy)
        {
            enemy.CliffOnLeft = CliffOnLeft;
            enemy.NearCliff = true;
        }
    }

    //public void OnTriggerStay2D(Collision2D collision)
    //{
    //    var enemy = collision.gameObject.GetComponent<Enemy>();
    //    if (enemy)
    //    {
    //        enemy.NearCliff = true;
    //    }
    //}

}
