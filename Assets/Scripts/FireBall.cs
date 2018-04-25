using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjType {
    enemy,
    player,
}
public class FireBall : MonoBehaviour {
    Rigidbody2D rig;
    Collider2D col;
    float speed =3f;
    [HideInInspector]
    public bool isRright= false;
    public  ObjType curType;
    void Start () {
        rig = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

    }
    public void Go() {
        float tDis = isRright == true ? transform.position.x+20 : transform.position.x - 20;
        transform.localScale = isRright == true ? Vector3.one:new Vector3(-1, 1, 1) ;
        iTween.MoveTo(gameObject, iTween.Hash("x", tDis, "speed", 20f, "islocal", true, "easetype", iTween.EaseType.linear, "oncomplete", "OnDestroy"));
    }
    private void OnDestroy()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && curType == ObjType.player)
        {
            if (collision.transform.GetComponent<Enemy>() != null)
            {
                Enemy tEnemy = collision.transform.GetComponent<Enemy>();
                tEnemy.GetHurt();
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.tag == "Player" && curType == ObjType.enemy)
        {
            collision.transform.GetComponent<PlayerController>().GetHurt();
            Destroy(gameObject);
        }
        else {
            if(!collision.gameObject.tag.Equals("Enemy") && !collision.gameObject.tag.Equals("Player"))
                Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
       
    }
}
