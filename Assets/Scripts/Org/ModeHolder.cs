using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeHolder : MonoBehaviour
{
    public int gameMode = 0;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
