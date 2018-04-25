using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    

    enum enemyState {
        idle,
        warning,
        freezing,
    }
    public int health = 3;
    public enemyType curType = enemyType.Fire;
    PolygonCollider2D myCol;
    Rigidbody2D myRig;
    Animator myAni;
    enemyState curState = enemyState.idle;
    enemyState lastState = enemyState.idle;
    float fireInterval;
    public GameObject fireBall;
    Transform mouth;
    float lastTimeX;
    float lastHurtTime;
    public GameObject Dropitem;
    bool isAlive = true;

    public float max_x;
    public float min_x;

    private void Awake()
    {
        myRig = GetComponent<Rigidbody2D>();
        myCol = GetComponent<PolygonCollider2D>();
        myAni = GetComponent<Animator>();
        lastTimeX = transform.position.x;
        if (curType != enemyType.Ice)
        { 
            mouth = transform.Find("Mouth");
        }
    }

    public void SetState(int pState) {
        if (curState != enemyState.freezing)
        {
            if (curType == enemyType.Fire)
            {
                curState = pState == 0 ? curState = enemyState.idle : curState = enemyState.warning;
                if (curState == enemyState.warning)
                    iTween.Pause(gameObject);
                else
                {
                    iTween.Resume(gameObject);
                    myAni.SetFloat("speed", 8);
                }
            }
        }
    }


    void Start()
    {
        if (curType == enemyType.Fire)
        {
            iTween.MoveFrom(gameObject, iTween.Hash("position", gameObject.transform.position + Vector3.left * 15, "time", 5f, "easetype", "linear", "looptype", "pingPong"));
        }
        else if (curType == enemyType.Ground) {

            iTween.MoveFrom(gameObject, iTween.Hash("x", max_x, "time", 1.5f, "easetype", "linear", "looptype", "pingPong"));

        }
        else
        {
            myRig.constraints = RigidbodyConstraints2D.FreezeAll;
        }

    }


    public void GetFreezing() {
        lastState = curState;
        curState = enemyState.freezing;
        iTween.Pause(gameObject, "move");
        iTween.ShakePosition(gameObject, iTween.Hash("amount", Vector3.one * 0.5f, "time", 1.5f));
        Invoke("FreezingBack",1.6f);
        GetComponent<SpriteRenderer>().color = new Color(0.32f,1,0.9f,1);
    }

    public void FreezingBack() {
        iTween.Stop(gameObject,"shake");
        Debug.Log("in FreezingBack");
        curState = lastState;
        lastState = enemyState.freezing;
        iTween.Resume(gameObject, "move");
        GetComponent<SpriteRenderer>().color = Color.white ;

    }


    public void GetHurt() {
        if (curType == enemyType.Fire)
        {
            if (curState == enemyState.idle)
            {
                SetState(1);
                transform.localScale = new Vector3(-1, 1, 1);
            }
            if (Time.time - lastHurtTime > 1 && health > 0)
            {

                lastHurtTime = Time.time;
                health--;

                myAni.SetBool("hurt", true);
                myAni.Play("E_Hurt");

                if (health <= 0)
                {
                    myAni.SetInteger("health", health);
                    //  myAni.Play("E_DIE");
                }
            }
        }
        else if (curType == enemyType.Ice)
        {
            health--;
            GetComponent<SpriteRenderer>().color = Color.red;
            if (health <= 0)
            {
                OnDIE();
            }
        }
        else if (curType == enemyType.Ground) {
            health--;
            if (health <= 0)
            {
                OnDIE();
            }
        }
    }

    public void HurtEnd() {
        myAni.SetBool("hurt", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (curType == enemyType.Ground) {

            if (collision.gameObject.tag == "Player" && curState == enemyState.freezing)
            {
                GetHurt();
            }
            else if(collision.gameObject.tag == "Player" && curState != enemyState.freezing&&collision.gameObject.name.StartsWith("DragonNinja"))
            {
                collision.gameObject.GetComponent<PlayerController>().GetHurt();

            }
        }
    }

    //found player  then fire 
    public void Fire() {
        if (Time.time - fireInterval>2.5f) {
            myAni.Play("E_ATK");
            fireInterval = Time.time;
        }
    }

    public void BreedFire() {
        GameObject tFire = Instantiate(fireBall, mouth.position, transform.rotation);
        FireBall tBall = tFire.GetComponent<FireBall>();
        tBall.isRright = transform.localScale.x == -1 ? false : true;
        tBall.Go();
        myAni.SetBool("Atk", false);
    }

    public void OnDIE()
    {
        
        if (isAlive == true)
        {
            if (Dropitem != null)
            {
                GameObject tItem = Instantiate(Dropitem, transform.position, transform.rotation);
                iTween.PunchPosition(tItem, iTween.Hash("y", -5.5f, "looptype", iTween.LoopType.loop, "time", 2.5f));
            }
            GameController.Instance.NextEnemy();
            Destroy(gameObject);
            isAlive = false;
        }
    }

    // Update is called once per frame
    void Update () {

        if (health <= 0 )
            return;
        if (curType == enemyType.Fire && myAni.GetBool("hurt")) {
            return;
        }
        if (curState == enemyState.warning)
        {
            Fire();
            myAni.SetFloat("speed", 0);
        }
        else if (curState == enemyState.freezing) {
            //iTween.Pause(gameObject,"move");
            //iTween.ShakePosition(gameObject, iTween.Hash("amount",Vector3.one*0.5f,"time",2));
        }
        else
        {
            if (Time.frameCount % 5 == 0)
            {
                float tX = transform.position.x;
                if (lastTimeX > tX)
                    transform.localScale = new Vector3(-1, 1, 1);
                else
                    transform.localScale = Vector3.one;
                lastTimeX = tX;
            }
        }
	}
}
