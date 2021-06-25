using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject win;
    public GameObject failed;
    public Sprite pauseIco;
    public Sprite continueIco;
    public static GameManager instance;
    private int i = 0;
  
    void Awake()
    {
        Time.timeScale = 1;
       instance = this;
    }
    public void Win()
    {
        GameObject.Find("AudioSource/UI").GetComponent<AudioManager>().UIAudioWin();

        GameObject.Find("AudioSource/Bgm").GetComponent<AudioManager>().SetMute();

        win.SetActive(true);
    }
    public void Failed()
    {
        GameObject.Find("AudioSource/UI").GetComponent<AudioManager>().UIAudioFail();
        GameObject.Find("AudioSource/Bgm").GetComponent<AudioManager>().SetMute();
        failed.SetActive(true);
    }

    public void OnButtonRetryDown()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
   
    public void OnButtonMenuDown()
    {
        SceneManager.LoadScene(0);
    }


    public void OnPasueButtonDown()
    {
        Time.timeScale = i;
        if (i == 0)
        {
            i++;
            GameObject.Find("MainCanvas/FunctionPanel/PasueAndContinueButton").GetComponent<Image>().sprite = continueIco;
            return;
        }
        if (i == 1)
        {
            i--;
            GameObject.Find("MainCanvas/FunctionPanel/PasueAndContinueButton").GetComponent<Image>().sprite = pauseIco;
            return;
        }
    }

}
