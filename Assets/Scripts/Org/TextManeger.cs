using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextManeger : MonoBehaviour
{
    [SerializeField] Text nowTurn;
    [SerializeField] Text p1;
    [SerializeField] Text p2;
    [SerializeField] Text center;
    [SerializeField] GameManeger gameManeger;
    [SerializeField] SpawnDice spawnD;
    [SerializeField] Image gauge;
    Color red = new Color(1, 0, 0, 1);
    Color blue = new Color(0, 0, 1, 1);

    void Start()
    {
        center.text = "Start!!";
        Invoke("DelCenter", 3);
    }

    void DelCenter()
    {
        center.text = "";
    }

    void Update()
    {
        TurnMng();
        PlayersMng();
        gauge.rectTransform.offsetMax = new Vector2(-1*(600 - spawnD.power * 10),0);
    }

    void TurnMng()
    {
        if(gameManeger.turn == 1)
        {
            nowTurn.color = red;
            nowTurn.text = "Player1";
        }
        else if (gameManeger.turn == -1)
        {
            nowTurn.color = blue;
            nowTurn.text = "Player2";
        }
    }

    void PlayersMng()
    {
        p1.text = "Player1\nHP : " + gameManeger.p1HP + "\n残り:" + gameManeger.p1has + "個";
        p2.text = "Player2\nHP : " + gameManeger.p2HP + "\n残り:" + gameManeger.p2has + "個";
    }

    public void P1Lose()
    {
        center.color = blue;
        center.text = "Winner Player2!";
    }

    public void P2Lose()
    {
        center.color = red;
        center.text = "Winner Player1!";
    }
}
