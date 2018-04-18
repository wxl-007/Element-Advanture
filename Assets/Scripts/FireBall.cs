using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour {
    Vector3 tStartPos;
    Rigidbody2D rig;
    Collider2D col;
    float speed =3f;
    void Start () {
        tStartPos = transform.position;
        rig = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
      //  Invoke("OnDestroy",2f);
        iTween.MoveTo(gameObject, iTween.Hash("x", tStartPos.x+20, "speed",15f,"islocal", true,"easetype",iTween.EaseType.linear,"oncomplete","OnDestroy"));

    }
    private void OnDestroy()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy") {

        }
    }

    void FixedUpdate()
    {
       
    }
}
