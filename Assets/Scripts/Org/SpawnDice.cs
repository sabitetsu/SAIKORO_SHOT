using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDice : MonoBehaviour
{
    #region Variables
    [SerializeField] GameObject diceRed;
    [SerializeField] GameObject diceBlue;
    [SerializeField] DiceCheck diceCheck;
    [SerializeField] GameManeger gameManeger;
    [SerializeField] GameObject marker;
    [SerializeField] MusicManeger musicManeger;
    public GameObject[] dices;
    GameObject dice;
    GameObject LineObj;
    Rigidbody rb;
    Transform markerTF;

    Vector3 pos1 = new Vector3(-10, 10, 0);
    Vector3 pos2 = new Vector3(10, 10, 0);
    Vector3 targetPos;
    Vector3 powerVector;
    Vector3 force;

    public float power = 0;
    float direct = 2;
    int errorCount = 0;
    GameObject errorDice;
    Vector3 errorPos;

    bool startTap = false;
    bool startRoll = false;
    bool ready = true;
    bool confirm = false;
    bool tapped = false;

    Die_d6 zeroOrNot;
    bool zero = false;
    List<GameObject> zeroDices = new List<GameObject>();
    Vector3 zeroPos;
    Vector3 outCheck;
    bool destroyDice = false;

    #endregion

    void Start() //markerのインスタンスを生成し、地下に隠す
    {
        marker = Instantiate(marker, new Vector3(0,-10,0), Quaternion.identity);
        markerTF = marker.transform;
    }

    public void RollDice() //SETで呼び出される dice生成、空中で止まる
    {
        if (ready) //readyがtrueであれば開始
        {
            //Debug.Log("SPAWN");
            musicManeger.SpawnSound();
            ready = false;
            gameManeger.turnEnd = false;
            //markerを真ん中にセット
            musicManeger.MarkSound();
            targetPos = new Vector3(0, 0.5f, 0);
            if (gameManeger.turn == 1) //player1
            {
                gameManeger.p1has -= 1;
                dice = Instantiate(diceRed, pos1, Random.rotation);
                rb = dice.GetComponent<Rigidbody>();
                rb.useGravity = false;
                TargetLine(pos1,targetPos);
            }
            else if (gameManeger.turn == -1)
            {
                gameManeger.p2has -= 1;
                dice = Instantiate(diceBlue, pos2, Random.rotation);
                rb = dice.GetComponent<Rigidbody>();
                rb.useGravity = false;
                TargetLine(pos2, targetPos);
            }
            
            markerTF.position = targetPos;
            startRoll = true;
            startTap = true;
        }
    }

    public void TapTarget() //Shotボタンで呼び出される
    {
        if(marker.transform.position.y >= 0)
        {
            tapped = true;
        }
        if (tapped)
        {
            startTap = false;
            startRoll = false;
            tapped = false;
            powerVector = new Vector3(power, power, power);
            AddPower();
        }
    }

    void Update() //ダイスを投げる強さを決める
    {
        if (startTap)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 50.0f))
                {
                    if(hit.collider.gameObject.tag != "Out")
                    {
                        musicManeger.MarkSound();
                        targetPos = hit.point;
                        markerTF.position = targetPos;
                        if (gameManeger.turn == 1)
                        {
                            Destroy(LineObj);
                            TargetLine(pos1,targetPos);
                        }
                        else if(gameManeger.turn == -1)
                        {
                            Destroy(LineObj);
                            TargetLine(pos2,targetPos);
                        }
                        
                    }
                }
            }
        }
        if (startRoll)
        {
            power += direct;
            if (power > 59 || power < 1)
            {
                direct *= -1;
            }
        }
    }


    void AddPower() //方向と力を調整して投げる
    {
        //Debug.Log("SHOT");
        if (targetPos.y < 1)
        {
            targetPos.y = -10;
        }
        else
        {
            targetPos.y -= 10;
            //powerVector.y = 1; //targetのyが1以上ならyはtargetPosのまま
        }

        if (gameManeger.turn == 1)
        {
            targetPos.x += 10;
        }
        else if (gameManeger.turn == -1)
        {
            targetPos.x -= 10;
        }
        musicManeger.ShotSound();
        force = Vector3.Scale(targetPos, powerVector);
        rb.AddForce(force, ForceMode.Impulse);
        rb.useGravity = true;
        markerTF.position = new Vector3(0, -10, 0);
        Destroy(LineObj);
        FindDices();

    }

    void FindDices()
    {
        //Debug.Log("FIND");
        dices = GameObject.FindGameObjectsWithTag("Dice");
        Invoke("CheckStop", 0.1f);
    }

    public void CheckStop()
    {
        //Debug.Log("CHECK");
        errorCount += 1;
        if(errorCount > 5) //5秒経っても動きが止まらない場合、動いてたダイスを上に投げる
        {
            errorPos = errorDice.transform.position; //動いてるダイスの座標を調べる
            rb = errorDice.GetComponent<Rigidbody>();
            rb.AddForce(new Vector3(errorPos.x,100,errorPos.z), ForceMode.Impulse);
            errorCount = 0;

            if(errorPos.y < 0 || errorPos.y > 100) //奈落あるいは空中にある場合は削除して相手のターン
            {
                Destroy(errorDice);
                gameManeger.ChangeTurn();
                ready = true;
                return;
            }
        }
        for(int i = 0; i < dices.Length; i++)
        {
            rb = dices[i].GetComponent<Rigidbody>();
            if (rb.IsSleeping())
            {
                continue;
            }
            //動いてるものがあれば呼び出される
            errorDice = dices[i]; //動いてるダイスを保存
            Invoke("CheckStop", 1);
            return;
        }
        //全て止まっている場合
        errorCount = 0;
        ZeroCheck();
    }

    void ZeroCheck()
    {
        //Debug.Log("ZERO");
        for (int i = 0; i < dices.Length; i++)
        {
            //目が0(判定不能)かだけ調べる
            zeroOrNot = dices[i].GetComponent<Die_d6>(); 
            if(zeroOrNot.value == 0)
            {
                zeroDices.Add(dices[i]);
                zero = true;
            }

            //ステージ外か調べる
            outCheck = dices[i].transform.position;
            if(outCheck.x < -15 || outCheck.x > 15)
            {
                Destroy(dices[i]);
                destroyDice = true;
            }
        }
        if (destroyDice)
        {
            destroyDice = false;
            Invoke("FindDices", 1);
        }
        else if (zero)
        {
            ReRoll();
        }
        else
        {
            ready = true;
            diceCheck.CheckingDice();
        }
    }

    void ReRoll() //不明なダイスの振り直し
    {
        //Debug.Log("REROLL");
        musicManeger.RerollSound();
        for (int i = 0; i < zeroDices.Count; i++)
        {
            zeroPos = zeroDices[i].transform.position; // 不明なダイスの座標を調べる
            rb = zeroDices[i].GetComponent<Rigidbody>();
            rb.AddForce(new Vector3(zeroPos.x, 200, zeroPos.z), ForceMode.Impulse);
        }
        zeroDices.Clear();
        zero = false;
        CheckStop();
    }

    void TargetLine(Vector3 startVec,Vector3 endVec)
    {

        LineObj = new GameObject("Line");
        LineRenderer lRend = LineObj.AddComponent<LineRenderer>();
        lRend.positionCount = 2;
        lRend.startWidth = 0.5f;
        lRend.endWidth = 0.5f;
        //Vector3 startVec = new Vector3(-10.0f, 10.0f, 0.0f);
        //Vector3 endVec = new Vector3(0.0f, 0.0f, 0.0f);
        lRend.SetPosition(0, startVec);
        lRend.SetPosition(1, endVec);
    }

}
