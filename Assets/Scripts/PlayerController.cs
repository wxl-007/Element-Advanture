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
    public float jump = 15f;
    bool grounded = true;
    bool atk = false;
    bool skill = false;
    float skilInterval = 0;
    float skillBtnInt = 0;
    Transform mouth;
    private Rigidbody2D rig2d;
    Animator playerAni;
    SpriteRenderer playerSp;
    public GameObject fireBallObj;
    public GameObject IceBall;
    public GameObject groundObj;
    Elements curElement;
    public GameObject[] elementList;
    private int[] unlockedElement =new int[5];
    [HideInInspector]
    public int health=3;

    //when playing atk  reshape the collider

    public Sprite[] aImages;
    private Dictionary<int, PolygonCollider2D> olFrameColliders;
    private SpriteRenderer oSpriteRenderer;
    Transform foot;
    public Transform[] HP;
    public Sprite[] HpSp;

    private void Awake()
    {
        oSpriteRenderer = this.GetComponent<SpriteRenderer>();
        olFrameColliders = new Dictionary<int, PolygonCollider2D>();

        for (int index = 0; index < aImages.Length; index++)
        {
            oSpriteRenderer.sprite = aImages[index];
            olFrameColliders.Add(index, gameObject.AddComponent<PolygonCollider2D>());
            olFrameColliders[index].enabled = false;
            olFrameColliders[index].isTrigger = true;
        }
        

    }

    void RefreshHP() {
        for (int i = 0; i < 3; i++)
        {
            if (i < health)
            {
                HP[i].GetComponent<SpriteRenderer>().sprite=HpSp[0];
            }
            else {
                HP[i].GetComponent<SpriteRenderer>().sprite = HpSp[1];
            }

        }
    }
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
        foot = transform.Find("Foot");
        curElement = Elements.Sword;
        //test unlock all
        unlockedElement =new int[]{0,0,1,0,0};

        skillBtnInt = Time.time;
        RefreshElement();
    }

    public void GetHurt() {
        health--;
        playerAni.SetBool("GetHurt", true);
        playerAni.Play("N_Hurt");
        //Debug.Log("my HP = "+health);
        RefreshHP();
        for (int index = 0; index < aImages.Length; index++)
        {
            olFrameColliders[index].enabled = false;
        }
       
        if (health <= 0)
        {
            GameController.Instance.GameOver();
        }

    }
    public void HurtEnd() {
        playerAni.SetBool("GetHurt", false);
    }

    public void GetElement(ItemType elementNum) {
        if (elementNum == ItemType.fire)
        {
            Debug.Log(" get fire ");

            unlockedElement[0] = 1;
        }
        else if (elementNum == ItemType.water)
        {
            unlockedElement[3] = 1;
        }
        else if (elementNum == ItemType.ground) {
            unlockedElement[1] = 1;
        }
        else if (elementNum == ItemType.HP)
        {
            health += 1;
            RefreshHP();
        }

        RefreshElement();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
     
        if (collision.gameObject.tag == "Enemy")
        {
            bool atkOther = false;
            for (int i = 0; i < aImages.Length; i++)
            {
                if (olFrameColliders[i].enabled)
                {
                    atkOther = true;
                    break;
                }
            }
            if (atkOther)
            {
                if (collision.GetComponent<Enemy>() != null)
                {
                    Enemy tEnemy = collision.GetComponent<Enemy>();
                    if (tEnemy.curType == enemyType.Fire)
                    {
                        tEnemy.GetHurt();
                    }
                    else if(tEnemy.curType == enemyType.Ice)
                    {
                        //can hurt
                    }
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            GameController.Instance.GameOver();
        }
        else if(collision.gameObject.tag =="Win") {
            GameController.Instance.WinGame();
        }
        if (collision.gameObject.tag == "Ground")
        {
            bool isOn = false;
            for (int i = 0; i < collision.contacts.Length; i++)
            {
                if (collision.contacts[i].point.y <= foot.transform.position.y)
                {

                    isOn = true;
                    break;
                }
            }
            if (isOn == true)
            {
                grounded = true;
                playerAni.SetBool("Ground", grounded);
            }
        }
        else if (collision.gameObject.tag == "Wall") {
            grounded = true;
            playerAni.SetBool("Ground", grounded);
        }
    }
 

    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.tag.Equals("Ground"))
        {
                grounded = false;
                playerAni.SetBool("Ground", grounded);
        }
    }


    void RefreshElement() {
        for (int i = 0; i < 5; i++)
        {
            if(unlockedElement[i]==0)
                elementList[i].GetComponent<SpriteRenderer>().color = Color.grey;
            else
                elementList[i].GetComponent<SpriteRenderer>().color = Color.white;

        }
    }


    void ChangeELement(bool tDir) {
        if (Time.time  -skillBtnInt > 1)
        {
            //true = left
            float tMove = 2;
            if (!tDir)
            {
                tMove = -2;
                curElement = (Elements)(((int)curElement + 1)>5?1: ((int)curElement + 1));

            }
            else {
                curElement = (Elements)(((int)curElement - 1) < 1 ? 5 : ((int)curElement - 1));
            }
            //Debug.Log("cur element"+curElement);
            for (int i = 0; i < 5; i++)
            {
                float x = elementList[i].transform.localPosition.x + tMove;
                if (x > 0 || x < -8)
                {
                    if (x > 0)
                        x = -8;
                    if (x < -8)
                        x = 0;
                    iTween.ScaleFrom(elementList[i].gameObject, iTween.Hash("scale", Vector3.one * 0.5f, "time", 0.8f, "easetype", iTween.EaseType.easeInOutBounce));
                    iTween.MoveTo(elementList[i].gameObject, iTween.Hash("x", x, "time", 0.7f, "easetype", iTween.EaseType.easeInOutCubic, "islocal", true));
                }
                else
                {
                    iTween.MoveTo(elementList[i].gameObject, iTween.Hash("x", x, "time", 0.7f, "easetype", iTween.EaseType.easeInOutCubic, "islocal", true));
                }
            }
            skillBtnInt = Time.time;
        }
    }


    public void SetEventAfterATK()
    {
        atk = false;
        playerAni.SetBool("ATK", atk);
        olFrameColliders[2].enabled = false;
        olFrameColliders[5].enabled = false;
    }
    public void SetEventAfterSkill()
    {

        skill = false;
        playerAni.SetBool("Skill",skill);
        SetSkill();
    }

    void SetSkill()
    {
        GameObject curObj = null;
        Vector3 tBornPos = mouth.transform.position;
        if (curElement == Elements.Fire)
        {
            curObj = fireBallObj;
            GameObject tFireBall = Instantiate(curObj, tBornPos, transform.rotation);
            FireBall tBall = tFireBall.GetComponent<FireBall>();
            tBall.isRright = transform.localScale.x == -1 ? false : true;
            tBall.Go();
        }
        else if (curElement == Elements.Ground)
        {
            curObj = groundObj;
            if (transform.localScale.x > 0)
                tBornPos += new Vector3(2, 2f, -5);
            else
                tBornPos -= new Vector3(2, -2f, 0);
            Instantiate(curObj, tBornPos, transform.rotation);
        }
        else if (curElement == Elements.Water) {
            curObj = IceBall;
            GameObject tFireBall = Instantiate(curObj, tBornPos, transform.rotation);
            IceBall tBall = tFireBall.GetComponent<IceBall>();
            tBall.isRright = transform.localScale.x == -1 ? false : true;
            tBall.Go();
        }
      
    }



    // Update is called once per frame
    private void Update()
    {
        if (playerAni.GetBool("GetHurt")) {
            return;
        }
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

            if (curElement == Elements.Sword) return;
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
            rig2d.AddForce(new Vector2(speed * Time.deltaTime * 3.5f, 1) * jump, ForceMode2D.Impulse);

        }
    }

    void FixedUpdate()
    {
        if (atk == true || skill == true||playerAni.GetBool("GetHurt")) {
            return;
        }
        if (Input.GetKey(KeyCode.A) )
        {
            //left
            if (grounded == true)
            {
                speed = -10f;
                playerAni.SetFloat("speed", Mathf.Abs(speed));

                rig2d.velocity = Vector2.right * speed * Time.deltaTime;
                transform.Translate(rig2d.velocity, Space.World);
                playerAni.Play("N_Walk");
                playerSp.transform.localScale = new Vector3(-1, 1, 1);
            }
            else {
                transform.Translate(Vector2.left * Time.deltaTime * 8, Space.World);

            }

        }
        else if (Input.GetKey(KeyCode.D) )
        {
            //right
            if (grounded == true)
            {
                speed = 10f;
                playerAni.SetFloat("speed", speed);
                playerSp.transform.localScale = Vector3.one;

                playerAni.Play("N_Walk");
                rig2d.velocity = Vector2.right * speed * Time.deltaTime;
                transform.Translate(rig2d.velocity, Space.World);
            }
            else
            {
                transform.Translate(Vector2.right*Time.deltaTime*8, Space.World);
                //rig2d.AddForce(Time.fixedDeltaTime * Vector2.right * 50, ForceMode2D.Impulse);
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

    public void SetPolyCollider_1() {
        olFrameColliders[0].enabled = true;
    }
    public void SetPolyCollider_2()
    {

        olFrameColliders[0].enabled = false;
        olFrameColliders[1].enabled = true;

    }
    public void SetPolyCollider_3()
    {
        olFrameColliders[1].enabled = false;
        olFrameColliders[2].enabled = true;
    }
    public void SetPolyCollider_J_1()
    {
        olFrameColliders[3].enabled = true;
    }
    public void SetPolyCollider_J_2()
    {

        olFrameColliders[3].enabled = false;
        olFrameColliders[4].enabled = true;
    }
    public void SetPolyCollider_J_3()
    {
        olFrameColliders[4].enabled = false;
        olFrameColliders[5].enabled = true;
    }

}
