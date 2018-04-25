using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum enemyType
{
    Fire = 1,
    Ice,
    Ground,
    Other,
}
public class GameController : MonoBehaviour {

    static GameController instance;
    public PlayerController playerCtrl;
    public Enemy[] enemys;
    public int enemyNum=0;
    public GameObject title_A;
    public GameObject title_E;
    public GameObject Over;
    public GameObject Win;

    public static GameController Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        enemyNum = 0;
    }
    public void NextEnemy() {
        enemyNum++;
        

        Debug.Log("in next enemy" + enemyNum);
    }
    void Start()
    {
        iTween.FadeTo(title_A, iTween.Hash("alpha", 0, "time", 1.5f));
        iTween.FadeTo(title_E, iTween.Hash("alpha", 0, "time", 1.5f));
    }

    public void GameOver() {
        iTween.ScaleTo(Over, iTween.Hash("scale", Vector3.one*3, "time", 1.5f, "ignoretimescale", true));
        playerCtrl.gameObject.SetActive(false);
        Invoke("ReloadAll",2f);
        //Time.timeScale = 0;
        
    }

    void ReloadAll() {
        Debug.Log("Reload");
        SceneManager.LoadScene("Scene/Main");
    }
    public void WinGame() {
        iTween.ScaleTo(Win, iTween.Hash("scale", Vector3.one * 3, "time", 1.5f, "ignoretimescale", true));
        playerCtrl.gameObject.SetActive(false);
        Invoke("ReloadAll", 2f);
    }
    void Update()
    {
        if (enemyNum > 3) return;
        if (enemys[enemyNum].gameObject.transform.position.x - playerCtrl.transform.position.x < 12)
        {
            if (enemys[enemyNum].transform.localScale.x == -1)
            {
                enemys[enemyNum].SetState(1);//enemy warning
            }
        }
        else if(enemys[enemyNum].gameObject.transform.position.x - playerCtrl.transform.position.x >20) {
                enemys[enemyNum].SetState(0);
        }
    }


}
