using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

// スキルを管理するクラスは static でいいかも
// でもそうすると MonoBehaviour にできない -> オブジェクトにくっつけられない
public class Skill : MonoBehaviour
{
    public static bool[] skillFlag = new bool[(int)skillID.END];

    private Text skill1, skill2;
    public int skl1Recast;
    public int skl2Recast;

    // GUI
    private GameObject Skill1;
    private GameObject Skill2;
    private Button Skill1Button;
    private Button Skill2Button;
    private Text Skill1Text;
    private Text Skill2Text;

    private int skl1Remain, skl2Remain;

    // Use this for initialization
    void Start()
    {
        skl1Remain = skl1Recast;
        skl2Remain = skl2Recast;

        Skill1 = transform.Find("Skill1").gameObject;
        Skill1Button = Skill1.transform.Find("Skill1Button").GetComponent<Button>();
        Skill1Text = Skill1.transform.Find("Skill1Button/Text").GetComponent<Text>();
        Skill1Button.interactable = false;

        Skill2 = transform.Find("Skill2").gameObject;
        Skill2Button = Skill2.transform.Find("Skill2Button").GetComponent<Button>();
        Skill2Text = Skill2.transform.Find("Skill2Button/Text").GetComponent<Text>();
        Skill2Button.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        checkSkill();
    }


    /// <summary>
    /// スキルゲージを貯める関数　とても色々なところから呼ばれる
    /// </summary>
    /// <param name="pattern"> </param>
    /// <param name="increValue"> 増加量 </param>
    /// 
    public void chargeSkill(int[] arg)
    {
        int pattern = arg[0];
        int increValue = arg[1];

        // こっから2つにスキルに司令をだす
        skl1Remain--;
        skl2Remain--;
    }

    public void checkSkill()
    {
        if (skl1Remain <= 0)
        {
            Skill1Button.interactable = true;
            Skill1Text.text = "OK!";
        } else
        {
            Skill1Text.text = new string('■', skl1Remain);
        }

        if (skl2Remain <= 0)
        {
            Skill2Button.interactable = true;
            Skill2Text.text = "OK!";
        }
        else
        {
            Skill2Text.text = new string('■', skl2Remain);
        }
    }

    // スキルを使ったら戻す 1か2だけだよ！
    public void skillReset(int num)
    {
        switch (num)
        {
            case 1:
                skl1Remain = skl1Recast;
                Skill1Button.interactable = false;
                break;

            case 2:
                skl2Remain = skl2Recast;
                Skill2Button.interactable = false;
                break;

            default:
                Debug.Log("skill Reset out of range!");
                break;
        }
    }
}
