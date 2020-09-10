using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTest : MonoBehaviour
{
    public int value;
    private Die_d6 die;
    // Use this for initialization
    void Start()
    {
        //最初にランダムで回転させる
        transform.rotation = Random.rotation;
        die = gameObject.GetComponent<Die_d6>();
    }

    // Update is called once per frame
    void Update()
    {
        value = die.value;
        Debug.Log(value);
    }
}
