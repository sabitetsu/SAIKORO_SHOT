using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    [SerializeField] SpawnDice spawnD;
    [SerializeField] DiceCheck diceC;
    [SerializeField] GameManeger gameM;
    GameObject modeHolder;
    ModeHolder modeH;

    int mode = 1;
    int changeError = 0;
    bool moving = false;

    private void Start() //2人モードならこのオブジェクトを削除
    {
        modeHolder = GameObject.Find("Mode");
        modeH = modeHolder.GetComponent<ModeHolder>();
        if(modeH.gameMode == 2)
        {
            Destroy(this);
        }
        else
        {
            //StartCoroutine("TurnLoop");
        }
    }

    /*IEnumerator TurnLoop()
    {
        while(moving == false)
        {
            yield return new WaitForSeconds(1);
            TurnCheck();
        }
    }*/

    private void Update()
    {
        if(moving == false)
        {
            if(gameM.turn == -1)
            {
                Debug.Log("NPCのターン");
                moving = true;
                StartCoroutine("MyTurn");
            }
        }
    }

    /*void TurnCheck()
    {
        if (gameM.turn == -1)
        {
            moving = true;
            StartCoroutine("MyTurn");
        }
    }*/

    IEnumerator MyTurn()
    {
        spawnD.RollDice();
        Debug.Log("SET");
        yield return new WaitForSeconds(Random.Range(0.5f,5));
        spawnD.TapTarget();
        Debug.Log("SHOT");
        yield return new WaitForSeconds(2);
        gameM.ChangeButton();
        Debug.Log("CHANGE");
        yield return new WaitForSeconds(0.1f);
        ChangeOrNot();
    }

    void ChangeOrNot()
    {
        changeError += 1;
        /*if(changeError > 10)
        {
            Debug.Log("ERROR");
            changeError = 0;
            StartCoroutine("MyTurn");
        }*/

        if(gameM.turn == 1)
        {
            changeError = 0;
            moving = false;
            //StartCoroutine("TurnLoop");
        }
        else
        {
            Invoke("Changing", 0.1f);
        }

    }

    void Changing()
    {
        gameM.ChangeButton();
        ChangeOrNot();
    }
}
