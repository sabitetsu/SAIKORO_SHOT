using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] ModeHolder modeH;
    [SerializeField] GameObject Buttons;
    [SerializeField] GameObject Rules;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void StartButton(int mode)
    {
        if(mode == 1)
        {
            modeH.gameMode = 1;
            SceneManager.LoadScene("Battle");
        }
        else if(mode == 2)
        {
            modeH.gameMode = 2;
            SceneManager.LoadScene("Battle");
        }
        else
        {
            Buttons.SetActive(false);
            Rules.SetActive(true);
        }
    }

    public void DelRules()
    {
        Rules.SetActive(false);
        Buttons.SetActive(true);
    }
}
