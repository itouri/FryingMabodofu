using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fever : MonoBehaviour {

	public Material skyBoxNormal;
	public Material skyBoxFever;

	private GameObject LightNormal;
	private GameObject LightFever;

	public float feverGauge;
	private float nowFeverGauge;

	public float feverTime;
	private float feverTimeRemain;

	private bool isFever = false;

	private Canvas canvas;

	public AudioClip SE_toFever;
	private AudioSource audioSource;

	// Comboによってチャージ量を変えるよ!
	private int[] chargeWithCombo1 = {1,1,1,1,1,1,1};
	private int[] chargeWithCombo2 = {1,2,3,4,5,6,7};
	private int[] chargeWithCombo3 = {1,2,2,3,3,3,4};

	private enum Sky
	{
		NORMAL = 0,
		FEVER,
	}

	// Use this for initialization
	void Start () {
		audioSource = gameObject.GetComponent<AudioSource>();
		switchSky ((int)Sky.NORMAL);
		canvas = Component.FindObjectOfType<Canvas> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isFever)
		{
			feverTimeRemain -= Time.deltaTime;
			float raito = feverTimeRemain / feverTime;
			canvas.SendMessage ("setFeverRate", raito);

			if (feverTimeRemain < 0) {
				endFever ();
                // 0 だとnullだと思われる?
				canvas.SendMessage ("setFeverRate", 0.0f);
			}
		}
	}

	/// <summary>
	/// trueならon falseならoff
	/// </summary>
	/// <param name="light">どのらいとを</param>
	private void switchSky(int sky)
	{
		switch(sky)
		{
		case (int)Sky.NORMAL:
			RenderSettings.skybox = skyBoxNormal;
			transform.Find("LightNormal").gameObject.SetActive(true);
			transform.Find("LightFever").gameObject.SetActive(false);
			break;
		case (int)Sky.FEVER:
			RenderSettings.skybox = skyBoxFever;
			transform.Find("LightNormal").gameObject.SetActive(false);
			transform.Find("LightFever").gameObject.SetActive(true);
			break;
		}
	}

	public void chargeFever(int combo)
	{
		if (combo >= chargeWithCombo3.Length-1)
		{
			combo = chargeWithCombo3.Length-1;
		}
		if (!isFever) {
			nowFeverGauge += chargeWithCombo3[combo-1];
			if (nowFeverGauge >= feverGauge) {
				gotoFever ();
			}
			float raito = nowFeverGauge / feverGauge;
            canvas.SendMessage("setFeverRate", raito);
        }
	}

	public void gotoFever()
	{
		feverTimeRemain = feverTime;
		switchSky ((int)Sky.FEVER);
		isFever = true;

		// 効果音を鳴らす
		audioSource.PlayOneShot(SE_toFever);

		float rate = 3.0f;

		// 各オブジェクトへ指示
		// 具材
		GameObject[] tagobjs = GameObject.FindGameObjectsWithTag("Ingredient");
		foreach (GameObject obj in tagobjs)
		{
			obj.SendMessage ("gotoFever",rate);
		}

		// 生成器
		GameObject.Find ("IngredientCreator").SendMessage ("gotoFever",rate);
		GameObject.Find ("Cannon").SendMessage ("gotoFever");
        Component.FindObjectOfType<Canvas>().SendMessage("gotoFever");
    }

	private void endFever()
	{
		nowFeverGauge = 0;
		switchSky ((int)Sky.NORMAL);
		isFever = false;

		// 各オブジェクトへ指示
		// 具材
		GameObject[] tagobjs = GameObject.FindGameObjectsWithTag("Ingredient");
		foreach (GameObject obj in tagobjs)
		{
			obj.SendMessage ("endFever");
		}

		// 生成器
		GameObject.Find ("IngredientCreator").SendMessage ("endFever");
		GameObject.Find ("Cannon").SendMessage ("endFever");
	}
}
