using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall : MonoBehaviour {

    Rigidbody2D rig;
    Collider2D col;
    float speed = 3f;
    [HideInInspector]
    public bool isRright = false;
    public ObjType curType;
    Animator Ani;
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        Ani = GetComponent<Animator>();
    }
    public void Go()
    {
        float tDis = isRright == true ? transform.position.x + 20 : transform.position.x - 20;
        transform.localScale = isRright == true ? Vector3.one : new Vector3(-1, 1, 1);
        iTween.MoveTo(gameObject, iTween.Hash("x", tDis, "speed", 20f, "islocal", true, "easetype", iTween.EaseType.linear, "oncomplete","OnDestroy"));
    }

    private void OnDestroy()
    {
        Destroy(gameObject);
    }

    private void DestroySelf()
    {
        iTween.Pause(gameObject,"move");
        Ani.SetBool("Final",true);
        Ani.Play("WaterBallEnd");
        Destroy(gameObject,0.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.transform.GetComponent<Enemy>() != null)
            {
                Enemy tEnemy = collision.transform.GetComponent<Enemy>();
                tEnemy.GetFreezing();
                DestroySelf();
            }
        }
        
    }
}
