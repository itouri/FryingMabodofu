using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour
{
    public float remainingTime;
    private float remainTime;

    public float radius;

	private int combo = 0;

    private MeshRenderer render;

	private GameObject sound;

    // Use this for initialization
    void Start () {
		// 最初の大きさはゼロ
		this.transform.localScale = new Vector3(0, 0, 0);
        remainTime = remainingTime;
        render = GetComponent<MeshRenderer>();
		sound = GameObject.Find ("Sound");
    }
	
	// Update is called once per frame
	void Update () {
        remainTime -= Time.deltaTime;

        float raito = remainTime / remainingTime;

        Color nc = render.material.color;
        Color col = new Color(nc.r, nc.g, nc.b, raito); 
        render.material.color = col;

        float scale = radius * (1 - raito);
        this.transform.localScale = new Vector3(scale, scale, scale);

        if ( remainTime < 0 )
        {
            Destroy(this.gameObject);
        }
    }

	void OnCollisionEnter(Collision collision)
	{
		string tag = collision.gameObject.tag;
		if (tag == "Ingredient")
		{
			// rb.AddForce(new Vector3(0, 2.0f, 6.0f), ForceMode.Impulse);
			collision.gameObject.SendMessage("setBullet",this.gameObject);
		}
	}

	public void addCombo()
	{
		this.combo++;
		playCombo();
		Debug.Log("combo:"+combo);
	}

	public void playCombo()
	{
		if (this.combo > 7) sound.SendMessage("playCombo",8);
		else sound.SendMessage("playCombo",this.combo);
		GameObject.Find ("/Fever").SendMessage("chargeFever",this.combo);
	}
}
