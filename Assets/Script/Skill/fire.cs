using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire : MonoBehaviour {

	private  Rigidbody rb;
	public GameObject blast;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update () {
		// 落下し始めたら爆発
		if (rb.velocity.y < -1) 
		{
			GameObject blasts = GameObject.Instantiate(blast) as GameObject;
			blasts.transform.position = this.transform.position;
			// GameObject.Find ("Sound").SendMessage ("playBomb");
			Destroy(this.gameObject);
		}
	}
}

