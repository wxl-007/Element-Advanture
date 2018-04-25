using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour {
    Rigidbody2D rig;
    BoxCollider2D col;
    BoxCollider2D childCol;
    void Start () {
        childCol= transform.Find("ChildCollider").GetComponent<BoxCollider2D>();
        rig = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        Invoke("OnDestroy", 8f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Untagged")
        {
            rig.constraints = RigidbodyConstraints2D.FreezeAll;
           // rig.isKinematic = true;

        }
        else if(collision.gameObject.tag == "Enemy") {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall") {
         //   rig.isKinematic = true;
        }
    }

    void OnDestroy()
    {
        Destroy(gameObject);
    }
    void Update () {
		
	}
}
