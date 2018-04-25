using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : Enemy {

    public float max_x;
    public float min_x;
    public Transform target;
    bool isPause=false;
    float lTimeX;
    enum enemyState
    {
        idle,
        warning,
        freezing,
    }
    enemyState curState = enemyState.idle;

    void Start () {
        iTween.MoveFrom(gameObject, iTween.Hash("x",max_x, "time", 1.5f, "easetype", "linear", "looptype", "pingPong"));
    }
	
	// Update is called once per frame
	void Update () {
        if (target.transform.position.x <= max_x && target.transform.position.x >= min_x && target.transform.position.y > transform.position.y)
        {
            iTween.Pause(gameObject);
            isPause = true;
            gameObject.transform.position = new Vector3(target.transform.position.x, transform.position.y, -5);
        }
        else {
            if(isPause == true)
                iTween.Resume(gameObject);

        }
        if (Time.frameCount % 5 == 0)
        {
            float tX = transform.position.x;
            if (lTimeX > tX)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = Vector3.one;
            lTimeX = tX;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player")) {
        }
        
    }
}
