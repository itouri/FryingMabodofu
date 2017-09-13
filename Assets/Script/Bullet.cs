using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    // 弾を生成してからしばらくは見えなくする
    public float invisibleTime;
    private float existTime = 0;

    private int combo = 0;

    private Rigidbody rb;
    private Vector3 vel;

	private GameObject sound;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
		sound = GameObject.Find ("Sound");
    }

    // Update is called once per frame
    void Update () {
        // 設定時間こえたらたまを表示する
        this.existTime += Time.deltaTime;
        if ( this.existTime > this.invisibleTime )
        {
            gameObject.GetComponent<Renderer>().enabled = true;
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        vel = rb.velocity;

        string tag = collision.gameObject.tag;
        if (tag == "Ingredient")
        {
            
            // rb.AddForce(new Vector3(0, 2.0f, 6.0f), ForceMode.Impulse);
            collision.gameObject.SendMessage("setBullet",this.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //rb.velocity = vel;
    }

    public void addCombo()
    {
        this.combo++;
        playCombo();
        // Debug.Log("combo:"+combo);
    }

    public void playCombo()
    {
		if (this.combo > 7) sound.SendMessage("playCombo",8);
		else sound.SendMessage("playCombo",this.combo);

		GameObject.Find ("/Fever").SendMessage("chargeFever",this.combo);
    }
}
