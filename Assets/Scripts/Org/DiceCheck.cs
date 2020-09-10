using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCheck : MonoBehaviour
{
    [SerializeField] SpawnDice spawnDice;
    [SerializeField] GameManeger gameManeger;
    [SerializeField] MusicManeger musicManeger;
    GameObject dice;
    Die_d6 die;
    Rigidbody rb;

    List<int> diceNum = new List<int>();
    List<int> deleteNum = new List<int>();
    [SerializeField] int hp1 = 20;
    [SerializeField] int hp2 = 20;
    public int damage = 0;
    int[] sums;

    


    public void CheckingDice()
    {
        CheckDiceNum();
        SameDice();
        DmgCheck();
        DeleteDice();
        gameManeger.moved = true;
        gameManeger.GameLoop();
    }

    void CheckDiceNum() //ダイスの面を一つづつ調べてdiceNumに保存
    {
        for (int i = 0; i < spawnDice.dices.Length; i++)
        {
            die = spawnDice.dices[i].GetComponent<Die_d6>();
            diceNum.Add(die.value);
        }
    }

    

    void SameDice() //diceNumを調べて同じ数がいくつあるかsumsに保存。0は不明。
    {
        sums = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < diceNum.Count; i++)
        {
            switch (diceNum[i])
            {
                case 0:
                    sums[0] += 1;
                    break;
                case 1:
                    sums[1] += 1;
                    break;
                case 2:
                    sums[2] += 1;
                    break;
                case 3:
                    sums[3] += 1;
                    break;
                case 4:
                    sums[4] += 1;
                    break;
                case 5:
                    sums[5] += 1;
                    break;
                case 6:
                    sums[6] += 1;
                    break;
            }
        }

        //diceNum削除
        diceNum.Clear();
    }

    void DmgCheck()  //同じ数字が2個以上あればダメージを与え、その数字をdeleteNumに保存する
    {
        damage = 0;
        for (int i = 0; i < sums.Length; i++)
        {
            if (sums[i] >= 2)
            {
                damage += i * (sums[i]-1);
                musicManeger.DamageSound();
                deleteNum.Add(i);
                /*//同じ数字のサイコロはプレイヤーの手持ちに戻る
                if (gameManeger.turn == 1)
                {
                    gameManeger.p1has += sums[i];
                }
                else if(gameManeger.turn == -1)
                {
                    gameManeger.p2has += sums[i];
                }*/
            }
        }
    }


    void DeleteDice() //２つ以上ある数字のサイコロを検索して削除
    {
        musicManeger.DeleteSound();
        for (int i = 0; i < spawnDice.dices.Length; i++)
        {
            die = spawnDice.dices[i].GetComponent<Die_d6>();
            for(int t = 0; t < deleteNum.Count; t++)
            {
                if(deleteNum[t] == die.value)
                {
                    Destroy(spawnDice.dices[i]);
                }
            }
        }

        //deleteNumの削除
        deleteNum.Clear();
    }
}
