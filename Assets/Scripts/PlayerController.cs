using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    enum Elements {
        Fire=1,
        Ground,
        Sword,
        Water,
        Eletric,
    }


    float speed = 10f;
    float jump = 15f;
    bool grounded = true;
    bool atk = false;
    bool skill = false;
    float skilInterval = 0;
    Transform mouth;
    private Rigidbody2D rig2d;
    Animator playerAni;
    SpriteRenderer playerSp;
    public GameObject fireBallObj;
    public GameObject groundObj;
    Elements curElement;
    public GameObject[] elementList;
    private int[] unlockedElement =new int[5];

    void Start()
    {
        rig2d = GetComponent<Rigidbody2D>();
        playerAni = GetComponent<Animator>();
        playerSp = GetComponent<SpriteRenderer>();
        grounded = true;
        rig2d.velocity = Vector2.zero;
        playerAni.SetBool("Ground", grounded);
        playerAni.SetFloat("speed", 0f);
        atk = false;
        playerAni.SetBool("ATK", atk);
        mouth=transform.Find("Mouth");
        curElement = Elements.Sword;
        unlockedElement[0] = 3;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
            playerAni.SetBool("Ground", grounded);
        }
    }


    void ChangeELement(bool tDir) {
        //true = left
        float tMove = 2;
        if (!tDir) {
            tMove = -2;
        }

        for (int i = 0; i < 5; i++)
        {
            float x= elementList[i].transform.localPosition.x+tMove;
            if (x > 0 || x < -8 )
            {
                if(x>0)
                    x = -8;
                if (x < -8)
                    x = 0;
                iTween.ScaleFrom(elementList[i].gameObject, iTween.Hash("scale", Vector3.one * 0.5f, "time", 0.8f, "easetype", iTween.EaseType.easeInOutBounce));
                iTween.MoveTo(elementList[i].gameObject, iTween.Hash("x", x,"time",0.7f,"easetype", iTween.EaseType.easeInOutCubic,"islocal",true));
            }
            else 
            {
                iTween.MoveTo(elementList[i].gameObject, iTween.Hash("x", x, "time", 0.7f, "easetype", iTween.EaseType.easeInOutCubic, "islocal",true));
            }
        }
    }
    // Update is called once per frame
    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.U))
        {
            //select element  left dir   
            ChangeELement(true);

        }
        else if(Input.GetKeyDown(KeyCode.I)){
            //select element right dir
            ChangeELement(false);
        }
        if (Input.GetKeyDown(KeyCode.J) && grounded == true)
        {
            //atk
            atk = true;
            playerAni.SetBool("ATK", atk);
            playerAni.Play("N_ATK");
            speed = 0;
            playerAni.SetFloat("speed", Mathf.Abs(speed));
        }
        else if (Input.GetKeyDown(KeyCode.J) && grounded == false)
        {
            atk = true;
            playerAni.SetBool("ATK", atk);
            playerAni.Play("J_ATK");
        }
        else if (Input.GetKeyDown(KeyCode.K) && grounded == true) {

            if (Time.time - skilInterval > 1)
            {
                skill = true;
                playerAni.SetBool("Skill", skill);
                playerAni.Play("N_Skill");
                skilInterval = Time.time;
            }
        }
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && grounded == true)
        {
            grounded = false;
            playerAni.SetBool("Ground", grounded);
            playerAni.Play("N_Jump");
            rig2d.AddForce(new Vector2(rig2d.velocity.x * Mathf.Abs(speed), 1) * jump, ForceMode2D.Impulse);

        }
    }
    void FixedUpdate()
    {
        if (atk == true|| skill == true) {
            return;
        }
        if (Input.GetKey(KeyCode.A) && grounded == true)
        {
            //left
            speed = -10f;
            playerAni.SetFloat("speed", Mathf.Abs(speed));
            rig2d.velocity = Vector2.right * speed * Time.deltaTime;
            transform.Translate(rig2d.velocity, Space.World);

            if (grounded == true)
            {
                playerAni.Play("N_Walk");
                playerSp.flipX = true;
            }

        }
        else if (Input.GetKey(KeyCode.D) && grounded == true)
        {
            //right
            speed = 10f;
            playerAni.SetFloat("speed", speed);
            rig2d.velocity = Vector2.right * speed * Time.deltaTime;
            transform.Translate(rig2d.velocity, Space.World);
            playerSp.flipX = false;
            if (grounded == true)
            {
                playerAni.Play("N_Walk");
            }
        }
        else 
        {
            if (grounded == true && atk == false)
            {
                playerAni.SetBool("Ground", grounded);
                if (rig2d.velocity == Vector2.zero)
                {
                    speed = 0;
                    playerAni.SetFloat("speed", speed);
                    playerAni.Play("N_Idle");
                }
            }
        }

    }

    public void SetEventAfterATK() {
        atk = false;
        playerAni.SetBool("ATK", atk);
    }
    public void SetEventAfterSkill() {

        skill = false;
        SetSkill();
    }

    void SetSkill() {
        GameObject curObj =null;
        Vector3 tBornPos = mouth.transform.position;
        if (curElement == Elements.Fire)
        {
            curObj = fireBallObj;
        }
        else if (curElement == Elements.Ground)
        {
            curObj = groundObj;
            tBornPos = new Vector3(2,2.5f,-5);
        }
        Instantiate(curObj, tBornPos, transform.rotation);
    }
 
}
