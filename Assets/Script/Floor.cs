using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
    /*
	// Update is called once per frame
	void Update () {
		
	}
    */

    void OnCollisionEnter(Collision collision)
    {
		// 爆弾だったら勝手に消えてくれるので何もしない
		// 壁との判定はprojectの方で設定してる
		if (collision.gameObject.name != "Bomb")
        {
            //if (collision.gameObject.tag == "Bullet")
            //{
            //    collision.gameObject.SendMessage("playCombo");
            //}
            // 画面外として消す
            Destroy(collision.gameObject);
        }
    }
}
