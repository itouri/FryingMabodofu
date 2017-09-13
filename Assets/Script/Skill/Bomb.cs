using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public GameObject blast;

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        GameObject blasts = GameObject.Instantiate(blast) as GameObject;
        blasts.transform.position = this.transform.position;
		GameObject.Find ("Sound").SendMessage ("playBomb");
        Destroy(this.gameObject);

        string tag = collision.gameObject.tag;
        if (tag == "Ingredient")
        {
            // rb.AddForce(new Vector3(0, 2.0f, 6.0f), ForceMode.Impulse);
            // collision.gameObject.SendMessage("setBullet", this.gameObject);
        }
    }
}
