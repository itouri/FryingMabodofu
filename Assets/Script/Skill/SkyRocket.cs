using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyRocket : MonoBehaviour {

	public int remainShoots;
	private int nowRemainShoots;

	public float radius;

	public float interval;
	private float passedTime;

	public GameObject fire;



	// Use this for initialization
	void Start () {
		passedTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		passedTime += Time.deltaTime;
		if (passedTime > interval)
		{
			passedTime = 0f;
			if (remainShoots <= 0) {
				Destroy (this.gameObject);
			} else {
				remainShoots--;
				shoot ();
			}

		}
	}

	// 花火を打ち上げる!
	private void shoot()
	{
		Vector3 fireVec = new Vector3 (0, 600, 0);
		GameObject fires = GameObject.Instantiate(fire) as GameObject;

		// 弾丸の位置を調整
		fires.transform.position = this.transform.position;

		// Rigidbodyに力を加えて発射
		fires.GetComponent<Rigidbody>().AddForce(fireVec);
	}
}
