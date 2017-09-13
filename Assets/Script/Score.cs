using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Score : MonoBehaviour
{
    public float cookingUITime;
    private float nowCookingUITime;

    private float feverUITime = 0.8f;
    private float nowFeverUITime;

    // 配列で管理したほうがいい?
    private int[] storedIngre = new int[(int)Kind.END - 1];

    private int score;
    private int money;

    // とっーても配列にしたい。でも Text の初期化の仕方わからないん
    private Text tofu, gOnion, meet, spice;
    private Text textSocre;
    private Text textMoney;
    private Text textFever; // 現在未使用

    private Slider sliderFever;

    private Button buttonCooking;
    private Text textCooking;

    private GameObject oneMore;
    private GameObject gotoCity;

    //FIXME スキルは別枠の方がいいな でも今のところはScoreと混同
    private Text textSkill;

	// 通信用GameObject
	private GameObject fever;

	// 料理CSVの保存先
	private CookingListMasterTable cookingListMT = new CookingListMasterTable();

    //
    private GameObject order;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < (int)Kind.END - 1; i++)
        {
            storedIngre[i] = 0;
        }

        score = 0;
        tofu = transform.Find("Ingre/TOFU").GetComponent<Text>();
        gOnion = transform.Find("Ingre/NEGI").GetComponent<Text>();
        meet = transform.Find("Ingre/MEET").GetComponent<Text>();
        spice = transform.Find("Ingre/SPICE").GetComponent<Text>();
        textSocre = transform.Find("Score").GetComponent<Text>();
        textMoney = transform.Find("Money").GetComponent<Text>();
        textFever = transform.Find("FEVER").GetComponent<Text>();
        sliderFever = transform.Find("SliderFever").GetComponent<Slider>();
        sliderFever.value = 0;

        buttonCooking = transform.Find("Cooking").GetComponent<Button>();
        textCooking = buttonCooking.transform.Find("Text").GetComponent<Text>();
        nowCookingUITime = 0.01f;
        nowFeverUITime = 0.01f;

        order = transform.Find("Order").gameObject;

        oneMore = transform.Find("oneMore").gameObject;
        oneMore.SetActive(false);

        gotoCity = transform.Find("gotoCity").gameObject;
        gotoCity.SetActive(false);

		// "Ferver"だとうまくFindできない。おそらくこれがcomponentだから
		fever = GameObject.Find ("/Fever");

		//料理一覧CSVを読み込んで保存
		cookingListMT.Load();
    }

    // Update is called once per frame
    void Update()
    {
        // 具材獲得状況を更新
        updateIngreStatusUI();
        // updateでやることかなぁ？
        // cooking();
        //
        cookingUI();

        //
        if (nowFeverUITime > 0)
        {
            nowFeverUITime -= Time.deltaTime;
            float rate = nowFeverUITime / feverUITime;
            Color col = textFever.color;
            textFever.color = new Color(col.r, col.g, col.b, rate);
        }
    }

    public void addScore(int score)
    {
        this.score += score;
        textSocre.text = this.score.ToString();
    }

    public void addMoney(int money)
    {
        this.money += money;
        textMoney.text = "￥"+this.money.ToString();
    }

    // 唯一そとから呼ばれる関数
    // SendMessage で呼ばれるので 引数はint arrayのみ
    // [0]:genre [1]:kind
    public void store(int[] ingreAry)
    {
        int genre = ingreAry[0];
        int kind = ingreAry[1];
        storedIngre[genre]++;
        string str = "";
        foreach (int i in storedIngre)
        {
            str += " : " + i.ToString();
        }
        // Debug.Log(str);

        int[] destArray = new int[2];
        destArray[0] = 0; // pattarn
        destArray[1] = 1; // value
        Component.FindObjectOfType<Canvas>().SendMessage("chargeSkill", destArray);
        // fever.SendMessage("chargeFever", 1.0f);

        // orderにも具材を追加
        order.SendMessage("addIngre", genre);
    }

    // private

    // 具材獲得状況を更新 tofu: ■ ■ ■ みたいな感じで表示する
    private void updateIngreStatusUI()
    {
        tofu.text = new string('■', storedIngre[(int)Kind.TOFU]);
        gOnion.text = new string('■', storedIngre[(int)Kind.GREEN_ONION]);
        meet.text = new string('■', storedIngre[(int)Kind.MEAT]);
        spice.text = new string('■', storedIngre[(int)Kind.SPICE]);
    }

    // 集めた具材をスコアなどへ変換
    private void cooking()
    {
        //foreach (var cookingMaster in cookingListMT.All)
        //{
        //    // ああ　なんて汚いんだろう
        //    if (cookingMaster.Tofu > storedIngre[(int)Kind.TOFU])
        //        continue;
        //    if (cookingMaster.GreenOnion > storedIngre[(int)Kind.GREEN_ONION])
        //        continue;
        //    if (cookingMaster.Meet > storedIngre[(int)Kind.MEAT])
        //        continue;
        //    if (cookingMaster.Spice > storedIngre[(int)Kind.SPICE])
        //        continue;

        //    // ここまで来たらすべての具材が揃ってるってこと
        //    // レシピ分の具材を消費
        //    storedIngre[(int)Kind.TOFU        ] -= cookingMaster.Tofu;
        //    storedIngre[(int)Kind.GREEN_ONION ] -= cookingMaster.GreenOnion;
        //    storedIngre[(int)Kind.MEAT        ] -= cookingMaster.Meet;
        //    storedIngre[(int)Kind.SPICE       ] -= cookingMaster.Spice;

        //    addScore(cookingMaster.Score);
        //    addMoney(cookingMaster.Value);
        //    cookingUI(cookingMaster.Name, cookingMaster.Value);

        //    Timer.addTime(1.0f);

        //    Debug.Log("COOKING!:"+cookingMaster.Name+" MONEY!:"+ cookingMaster.Value);
        //}
    }

    // UIを徐々に透明にする
    private void cookingUI()
    {
        if (nowCookingUITime > 0)
        {
            nowCookingUITime -= Time.deltaTime;
            float rate = nowCookingUITime / cookingUITime;
            Color col = buttonCooking.image.color;
            buttonCooking.image.color = new Color(col.r, col.g, col.b, rate);
            col = textCooking.color;
            textCooking.color = new Color(col.r, col.g, col.b, rate);
        }
    }

    private void cookingUI(string name, int value)
    {
        nowCookingUITime = cookingUITime;
        textCooking.text = name+" ￥"+value;
    }

	public void setFeverRate(float rate=0)
	{
		sliderFever.value = rate;
	}

    public void gotoFever()
    {
        nowFeverUITime = feverUITime;
    }

    public void onEnd()
    {
        oneMore.SetActive(true);
        gotoCity.SetActive(true);
    }
}
