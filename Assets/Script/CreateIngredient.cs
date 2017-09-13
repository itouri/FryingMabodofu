using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateIngredient : MonoBehaviour
{
    public GameObject normalDofu;
    public GameObject kinuDofu;
    public GameObject koyaDofu;

    public GameObject normalGreenOnion;

    public GameObject normalMeet;

    public GameObject normalSpice;

    public float xRange;
    public float yRangeUpper;
    public float yRangeDownner;

    public float xRotaRange;
    public float yRotaRangeUpper;
    public float yRotaRangeDownner;

    public float initZ = 25.0f;

    // 具材の初期配置で使用
    public float startZ;
    public float intervalZ;

    public float interval = 3.0f;
	private float normalInterval;
    private float time;

    // 次に具材を出現させるエリア decideInitPos()で使用
    private int createArea = 0;

    // クラスを別にして共通で使えるようにした方がいい
   
	private bool isFever;

    // Use this for initialization
    void Start()
    {
		normalInterval = interval;
		isFever = false;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time > interval)
        {
            time = 0;
            createIngredient(initZ);
        }
    }

    // 具材を生成
    private void createIngredient(float initZ)
    {
        // 位置が重ならないように調整
        Vector3 initPos = decideInitPos(initZ);

        // 初期の回転
        Vector3 initRota = new Vector3(Random.Range(-xRotaRange, xRotaRange),
                                      Random.Range(yRotaRangeDownner, yRotaRangeUpper),
                                      0);

        int kindNum = (int)Random.Range(0.0f, 1000f);
        kindNum %= (int)Kind.SPICE + 1;

        // 一応再抽選（必要ない?）
        int VarietyNum = (int)Random.Range(0.0f, 1000f);

        GameObject Ingre = null;

        //HACKME このswitch文もっと頭良く書けないだろうか
        switch (kindNum)
        {
            case (int)Kind.TOFU:
                //VarietyNum %= (int)TOFU.END;
                VarietyNum %= (int)TOFU.NORMAL+1;
                switch (VarietyNum)
                {
                    case (int)TOFU.NORMAL: Ingre = GameObject.Instantiate(normalDofu) as GameObject; break;
                    //case (int)TOFU.KINU: Ingre = GameObject.Instantiate(kinuDofu) as GameObject; break;
                    //case (int)TOFU.KOYA: Ingre = GameObject.Instantiate(koyaDofu) as GameObject; break;
                }
                Tofu t = Ingre.GetComponent<Tofu>();
				t.create((int)Kind.TOFU, VarietyNum, initPos, initRota, isFever);
                break;

            case (int)Kind.GREEN_ONION:
                VarietyNum %= (int)GREEN_ONION.END;
                switch (VarietyNum)
                {
                    case (int)GREEN_ONION.NORMAL: Ingre = GameObject.Instantiate(normalGreenOnion) as GameObject; break;
                }
                GreenOnion g = Ingre.GetComponent<GreenOnion>();
				g.create((int)Kind.GREEN_ONION, VarietyNum, initPos, initRota, isFever);
                break;

            case (int)Kind.MEAT:
                VarietyNum %= (int)MEAT.END;
                switch (VarietyNum)
                {
                    case (int)MEAT.NORMAL: Ingre = GameObject.Instantiate(normalMeet) as GameObject; break;
                }
                Meet m = Ingre.GetComponent<Meet>();
				m.create((int)Kind.MEAT, VarietyNum, initPos, initRota, isFever);
                break;

            case (int)Kind.SPICE:
                VarietyNum %= (int)SPICE.END;
                switch (VarietyNum)
                {
                    case (int)SPICE.NORMAL: Ingre = GameObject.Instantiate(normalSpice) as GameObject; break;
                }
                Spice s = Ingre.GetComponent<Spice>();
				s.create((int)Kind.SPICE, VarietyNum, initPos, initRota, isFever);
                break;

                //case (int)Kind.CHINA_CONDIMENT:
                //    kindNum %= (int)TOFU.END;
                //    switch (kindNum)
                //    {
                //        case (int)TOFU.NORMAL:
                //            Ingre = GameObject.Instantiate(momenDofu) as GameObject;
                //            break;
                //    }
                //    Tofu t = Ingre.GetComponent<Tofu>();
                //    t.create((int)Kind.TOFU, kindNum, initPos);
                //    break;
        }
        // Ingre.transform.position = initPos; // 何？この行
    }

    // 出現位置をなるべく重ならないように決定する
    // 出現範囲を左下、左上、右下、右上の４エリアに区切って、各エリアに順番に出現させる
    private Vector3 decideInitPos(float initZ)
    {
        Vector2 pos = Vector3.zero;

        float yRange = ( yRangeUpper - yRangeDownner) / 2;

        createArea++;
        switch ( createArea )
        {
            // 左下
            case 0:
                pos = new Vector2(Random.Range(-xRange, 0),
                                  Random.Range(yRangeDownner, yRangeUpper - yRange));
                break;
            // 左上
            case 1:
                pos = new Vector2(Random.Range(-xRange, 0),
                                  Random.Range(yRangeDownner + yRange, yRangeUpper));
                break;
            // 右下
            case 2:
                pos = new Vector2(Random.Range(0, xRange),
                                  Random.Range(yRangeDownner, yRangeUpper - yRange));
                break;
            // 右上
            case 3:
                pos = new Vector2(Random.Range(0, xRange),
                                  Random.Range(yRangeDownner + yRange, yRangeUpper));
                createArea = -1; //FIXME!
                break;
        }

        // やっぱり重なるので zもばらけさせよう
        // initZ += Random.Range(-1.5f, 1.5f);
        Vector3 retPos = new Vector3(pos.x, pos.y, initZ);
        return retPos;
    }

	public void gotoFever(float rate)
	{
		interval /= rate;
		isFever = true;
	}

	public void endFever()
	{
		interval = normalInterval;
		isFever = false;
	}

    public void onInit()
    {
        // ゲームの開始時に具材を初期配置する
        // このやりかただとゲーム中の具材密度と差がある
        for (; startZ < initZ; startZ += intervalZ)
        {
            createIngredient(startZ);
        }
    }
}
