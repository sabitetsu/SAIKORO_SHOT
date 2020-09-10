using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManeger : MonoBehaviour
{
    [SerializeField] DiceCheck diceCheck;
    [SerializeField] SpawnDice SpawnDice;
    [SerializeField] TextManeger textMng;
    [SerializeField] MusicManeger music;
    [SerializeField] GameObject dSix;
    
    public static int player1 = 1;
    public static int player2 = -1;
    public int p1has = 10; //持ちサイコロの数
    public int p2has = 10;
    public int p1HP = 20; //HP
    public int p2HP = 20;
    public int turn = player1;
    public bool moved = false;
    public bool turnEnd = false;

    void Start() //開始時にサイコロを振る
    {
        Instantiate(dSix, new Vector3(0,10,0), Random.rotation);
    }

    public void GameLoop()
    {
        Dameged();
        CheckLose();
    }

    public void ChangeButton() //PASSで呼び出される
    {
        if (moved)
        {
            ChangeTurn();
        }
    }

    public void ChangeTurn()
    {
        if (turnEnd)
        {
            turn *= -1;
            moved = false;
            turnEnd = false;
        }
    }

    void CheckLose()
    {
        if(p1HP <= 0)
        {
            textMng.P1Lose();
            music.WineerSound();
            Invoke("ReStart",5);
        }
        else if(p2HP <= 0)
        {
            textMng.P2Lose();
            music.WineerSound();
            Invoke("ReStart", 5);
        }
        else if (p1has <= 0)
        {
            textMng.P1Lose();
            music.WineerSound();
            Invoke("ReStart", 5);
        }
        else if (p2has <= 0)
        {
            textMng.P2Lose();
            music.WineerSound();
            Invoke("ReStart", 5);
        }
    }

    void Dameged()
    {
        turnEnd = true;
        if (turn == player1)
        {
            p2HP -= diceCheck.damage;
        }
        else if (turn == player2)
        {
            p1HP -= diceCheck.damage;
        }

        if(diceCheck.damage > 0)
        {
            ChangeTurn();
        }
    }

    void ReStart()
    {
        SceneManager.LoadScene("Battle");
    }
}
