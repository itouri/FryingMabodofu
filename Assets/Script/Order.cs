using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class sOrderList
{
    public int id;
    public GameObject order;
}

public class Order : MonoBehaviour {

    // 注文UIのプレハブ
    public GameObject orderUI;

    // UIの位置
    public float startX;
    public float startY;
    public float margin;
    // 高さを図るのがめんどうだから直打ち
    private float height = 60f;

    //-- private --//

    const int MAX_ORDER = 3;

    List<sOrderList> orderList = new List<sOrderList>();

    // 配列で管理したほうがいい?
    private int[] storedIngre = new int[(int)Kind.END - 1];

    // 料理CSVの保存先
    private CookingListMasterTable cookingListMT = new CookingListMasterTable();
    private List<CookingListMaster> cookingList;

    // Use this for initialization
    void Start () {
        cookingListMT.Load();
        cookingList = new List<CookingListMaster>(cookingListMT.All);

        // 注文リストを作成
        for (int i=0; i < MAX_ORDER; i++)
        {
            // CSVの番号に合わせるため+1する
            int cookingID = (int)Random.Range(0, 10000) % MAX_ORDER + 1;
            makeOrderUI(cookingID);
        }
    }
	
	// Update is called once per frame
	void Update () {
        updateListUI();
        cooking();
	}

    // 受け取った料理idをもとに注文UIを作成
    void makeOrderUI(int cookingID)
    {
        cookingID--;
        if ( orderList.Count >= MAX_ORDER )
        {
            Debug.Log("注文リストに入り切らない注文を追加しようとしてるよ！");
            return;
        }

        sOrderList oList = new sOrderList();
        oList.id = cookingID;
        oList.order = Instantiate(orderUI);
        // これの子にする なんか transform.parentに入れる方法だと警告がでるから変えた。
        oList.order.transform.SetParent(this.transform,false);
        oList.order.GetComponent<RectTransform>().localPosition = new Vector3(startX, startY - (height + margin) * orderList.Count, 0);

        Debug.Log(cookingList[cookingID].Name);
        oList.order.SendMessage("setName", cookingList[cookingID].Name);
        int[] arg =
        {
            cookingList[cookingID].Tofu,
            cookingList[cookingID].GreenOnion,
            cookingList[cookingID].Meet,
            cookingList[cookingID].Spice
        };
        oList.order.SendMessage("setIngre", arg);
        orderList.Add(oList);
    }

    // 注文IDをもとに注文リストから削除
    void removeOrderUI(int orderID)
    {
        // まずDestroy
        Destroy(orderList[orderID].order);
        Debug.Log("RemoveAt"+orderID);
        orderList.RemoveAt(orderID);

        // UIを作り直す
        for (int i = 0; i<2; i++)
        {
            orderList[i].order.GetComponent<RectTransform>().localPosition = new Vector3(startX, startY - (height + margin) * i, 0);
        }
        
        // 消したらすぐ新しいのを作る
        // Start()と同じことやってるんだよなー
        int id = Random.Range(0, 10000) % MAX_ORDER + 1;

        makeOrderUI(id);
    }

    // 集めた具材をスコアなどへ変換
    private void cooking()
    {
        bool flag = false;
        int i = -1;
        foreach (sOrderList order in orderList)
        {
            i++;
            CookingListMaster cooking = cookingList[order.id];

            // ああ　なんて汚いんだろう
            if (cooking.Tofu > storedIngre[(int)Kind.TOFU])
                continue;
            if (cooking.GreenOnion > storedIngre[(int)Kind.GREEN_ONION])
                continue;
            if (cooking.Meet > storedIngre[(int)Kind.MEAT])
                continue;
            if (cooking.Spice > storedIngre[(int)Kind.SPICE])
                continue;

            flag = true;

            // ここまで来たらすべての具材が揃ってるってこと
            // レシピ分の具材を消費
            storedIngre[(int)Kind.TOFU] -= cooking.Tofu;
            storedIngre[(int)Kind.GREEN_ONION] -= cooking.GreenOnion;
            storedIngre[(int)Kind.MEAT] -= cooking.Meet;
            storedIngre[(int)Kind.SPICE] -= cooking.Spice;

            //addScore(cooking.Score);
            //addMoney(cooking.Value);
            //cookingUI(cooking.Name, cooking.Value);

            Timer.addTime(1.0f);

            Debug.Log("COOKING!:" + cooking.Name + " MONEY!:" + cooking.Value);
            break;
        }
        if (flag)
        {
            Debug.Log(i);
            removeOrderUI(i);
        }
    }

    // 外部からのコール
    public void addIngre(int genre)
    {
        storedIngre[genre]++;
        updateListUI();
    }

    public void updateListUI()
    {
        foreach (var oL in orderList)
        {
            oL.order.SendMessage("updateUI", storedIngre);
        }
    }

}
