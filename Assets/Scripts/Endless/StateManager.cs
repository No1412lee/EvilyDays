using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[SerializeField]
public class StateData
{
    public float hp = 100;
    public int hunger = 100;
    public int iq = 500;
    public int iqMax = 500;
}
public class StateManager : MonoBehaviour {

    public static StateData major;
    public static StateManager instance;
    private float totalTime = 0;
    public float hungerPer;
    public float hpReducePer;
    public float hpUpPer;
    public int whenHpUp;
    public int whenHpReduce;
    public Text hpText;
    public Text hungerText;
    public Text iqText;
    public Slider hpSlider;
    public Slider hungerSlider;
    public Slider iqSlider;

    void Awake()
    {
        instance = this;
        major = new StateData();
    }
	// Use this for initialization
	void Start ()
    {
      //  major = new StateData();
        major.hunger = GameDataManager.gameData.major.hunger;
        ChangeHunger(0);
        major.hp = GameDataManager.gameData.major.hp;
        ChangeLife(0);
        major.iqMax = GameDataManager.gameData.major.iqMax;
        ChangeIQ(GameDataManager.gameData.major.iq);
        BuildManager.money = major.iq;
        StartCoroutine("Timer1");
        StartCoroutine("Timer2");
    }
	
	// Update is called once per frame


    public IEnumerator Timer1()
    {
        yield return new WaitForSeconds(hungerPer);
        ChangeHunger(-10);
        StartCoroutine("Timer1");
    }

    public IEnumerator Timer2()
    {
        yield return new WaitForSeconds(1);
        if (major.hunger <= whenHpReduce)
            ChangeLife(-hpReducePer);
        if (major.hunger >= whenHpUp)
            ChangeLife(hpUpPer);
        StartCoroutine("Timer2");
    }


    public void ChangeLife(float change)
    {
        major.hp += change;
        if (major.hp > 100)
            major.hp = 100;
        if (major.hp <= 0)
        {
            major.hp = 0;
            GameManager.instance.Failed();
        }
        hpSlider.value = major.hp / 100;
        hpText.text =(int)major.hp + "/100";
        if (major.hp <= 0)
            GameManager.instance.Failed();
        return;
    }

    public void ChangeHunger(int change)
    {
        major.hunger += change;
        if (major.hunger > 100)
            major.hunger = 100;
        if (major.hunger < 0)
            major.hunger = 0;
        hungerSlider.value = (float)major.hunger / 100;
        hungerText.text = major.hunger + "/100";
        return;
    }

    public void ChangeIQ(int iq)
    {
        major.iq = iq;
        iqSlider.value = (float)major.iq / major.iqMax;
        iqText.text = major.iq + "/" + major.iqMax;
        return;
    }
}
