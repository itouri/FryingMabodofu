using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour {

    public float speed;
	private float normalSpeed;

    // 何かに当たったか
    protected bool hitted = false;

    protected int combo = 0;

    protected int genre;
    protected int kind;

    protected Rigidbody rb;

    protected GameObject bullet; // 当てられた弾

	protected void create(int genre, int kind, Vector3 initPos, bool isFever)
    {
        this.genre = genre;
        this.kind = kind;
        this.transform.position = initPos;

		normalSpeed = speed;
		if (isFever)
		{
			gotoFever (3.0f);
		}

        // 豆腐まで回転し始めるとオブジェクトの統一感がなさすぎる
        // this.transform.Rotate(initRota);
    }

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();

		// create()でやってるからいらない?
		// normalSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 direction = new Vector3(0, 0, -speed);

        if (!hitted)
        {
            this.transform.position += direction * Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;
        if (tag != "Wall")
        {
            if (!hitted)
            {
                // 重力オン
                rb.useGravity = true;
                hitted = true;
                int[] destArray = new int[2];
                destArray[0] = genre;
                destArray[1] = kind;
                Component.FindObjectOfType<Canvas>().SendMessage("store", destArray);
                //GameObject.Find("UISystem").SendMessage("store", destArray);
                //Score.store(genre, kind);
                if (tag == "Bullet")
                {
                    // 何かやることあるか?
                    // bulletがsetbulletを呼んでいる
                }
                else if (tag == "Ingredient")
                {
                    // 未vs未　具材は手に入るけどcomboはなしにしよう
                }
            }
            else
            {
                // 未vs済 ヒット済なら未ヒットに弾情報をセット
                if (this.bullet != null && tag == "Ingredient" ) // hitted で bullet が null は起こり得ないけど一応チェック
                {
                    collision.gameObject.SendMessage("setBullet", bullet);
                }
            }
        }
    }

    // どこからcallされてるのかわからなくなるね
    public void setBullet(GameObject bullet)
    {
        // bulletがないならaddCombo
        // if ( !hitted ) // これだと初ヒットでもhittedになってからsetBulletが呼ばれるときがある
        if (this.bullet == null)
        {
            this.bullet = bullet;
            bullet.SendMessage("addCombo");
        }
    }

	public void gotoFever(float rate)
	{
		this.speed *= rate;
	}

	public void endFever()
	{
		this.speed = normalSpeed;
	}
}
