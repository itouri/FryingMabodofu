using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    // bullet prefab
    public GameObject bullet;
    // 爆弾!
    public GameObject bomb;
	public GameObject skyRocket;
	public GameObject tmpSkyRocket;

    public GameObject Trajectory;

    // 弾丸発射点
    public Transform muzzle;

    private AudioSource audioSource;
    public AudioClip SE_maxChage;
    public AudioClip SE_shoot;

    // 弾丸の速度
    public float maxChageTime = 3.0f; // 最大チャージにかかる時間(秒)
    public float maxSpeed = 2000;
    public float chageTime = 0;

    // OnMouseDragで使用
    private Vector3 dragForce;

    private Vector3 startPos;
    private Vector3 endPos;

    private bool chaged = false;

    // ゲーム終了! 弾が打てなくなる
    private bool canShoot = false;

	//
	private Renderer model;
	private Renderer bombModel;

	private bool skl001_throwBomb = false;
	private bool skl002_fireWorks = false;

	private bool isFever = false;

    // Use this for initialization  
    void Start()
    {
        startPos = Vector3.zero;
        endPos = Vector3.zero;
        audioSource = gameObject.GetComponent<AudioSource>();
		model = transform.Find("Model").GetComponent<Renderer>();
		bombModel = transform.Find("BombModel").GetComponent<Renderer>();
		model.enabled = true;
		bombModel.enabled = false;
		isFever = false;
    }

    // Update is called once per frame
    void Update()
    {
		if (Skill.skillFlag[(int)skillID.THROW_BOMB])
		{
			model.enabled = false;
			bombModel.enabled = true;
			skl001_throwBomb = true;
			GameObject.Find("UISystem").SendMessage("skillReset",2);
			Skill.skillFlag [(int)skillID.THROW_BOMB] = false;
		}

		if (Skill.skillFlag[(int)skillID.FIREWORKS])
		{
			model.enabled = false;
			bombModel.enabled = false;
			skl002_fireWorks = true;
			GameObject.Find("UISystem").SendMessage("skillReset",1);
			Skill.skillFlag [(int)skillID.FIREWORKS] = false;

			// 前の弾道を削除
			GameObject[] tagobjs = GameObject.FindGameObjectsWithTag("Trajectory");
			foreach (GameObject obj in tagobjs)
			{
				Destroy(obj);
			}
		}

		if (skl002_fireWorks) {
			
			// ボタンを押したときに誤作動するのを防止
			if (Input.mousePosition.y > Screen.height * 0.2) {
			} else {
				// 大丈夫か?
				return;
			}
			// カメラの座標を取得?
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			// マウスのrayとの衝突地点を取得
			RaycastHit hit;

			// Floorとのみ衝突
			int layerMask = LayerMask.GetMask (new string[] { "Floor" });

			// 左クリックが押されている間
			if (Input.GetMouseButton (0))
			{
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) {
					Transform tmp = transform.Find ("TmpSkyRocket");
					if (tmp) {
						Destroy (tmp.gameObject);
					}
					Debug.Log ("create tmpSkyRockets");
					GameObject tmpSkyRockets = GameObject.Instantiate (tmpSkyRocket) as GameObject;
					tmpSkyRockets.name = "TmpSkyRocket";

					// これの子にする
					tmpSkyRockets.transform.parent = this.transform;

					// Rayの衝突地点に、このスクリプトがアタッチされているオブジェクトを移動させる
					tmpSkyRockets.transform.position = hit.point;
					tmpSkyRockets.transform.position = new Vector3 (hit.point.x, 0, hit.point.z);
				}
			}
				// 左クリックが離れた時
			if (Input.GetMouseButtonUp (0)) {
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) {
					GameObject skyRockets = GameObject.Instantiate (skyRocket) as GameObject;

					// Rayの衝突地点に、このスクリプトがアタッチされているオブジェクトを移動させる
					skyRockets.transform.position = hit.point;
					skyRockets.transform.position = new Vector3 (hit.point.x, 0, hit.point.z);

					// 通常に戻す
					model.enabled = true;
					bombModel.enabled = false;
					skl002_fireWorks = false;
				}
				// 残ってるので消す もしかしてこの時点ではobjectは作られていない? Instantiate と Destroy のタイミングが違う?
				if (transform.Find ("TmpSkyRocket")) {
					Destroy (transform.Find ("TmpSkyRocket").gameObject);
					Debug.Log ("delete tmpSkyRockets");
				}
			}
		}
		else
		{
			shoot();
		}
        
    }

	// 予測弾道を表示
    private void trajectory(Vector3 force)
    {
        // 弾道の表示
        float time = 3.0f;
        float interval = 0.05f;

        // 前の弾道を削除
        GameObject[] tagobjs = GameObject.FindGameObjectsWithTag("Trajectory");
        foreach (GameObject obj in tagobjs)
        {
            Destroy(obj);
        }

        // 最初だけ interval の半分の起動を表示
        GameObject Trajectory2 = GameObject.Instantiate(Trajectory) as GameObject;
        Vector3 trajectPos2 = TrajectoryCalculate.Force(transform.position, force, bullet.GetComponent<Rigidbody>().mass, Physics.gravity, 1, interval / 4);
        Trajectory2.transform.position = trajectPos2;
        Trajectory2.name = "lookAtTrajectory";

        for (float i = 0; i < time; i += interval)
        {
            GameObject Trajectories = GameObject.Instantiate(Trajectory) as GameObject;
            Vector3 trajectPos = TrajectoryCalculate.Force(transform.position, force, bullet.GetComponent<Rigidbody>().mass, Physics.gravity, 1, i);
            Trajectories.transform.position = trajectPos;
        }
    }

    public void shoot()
    {
        // 左クリックされた時
        if (Input.GetMouseButtonDown(0))
        {
			if (Input.mousePosition.y > Screen.height * 0.2) {
				startPos = Input.mousePosition;
			} else {
				//XXX 危険だ
				startPos = Vector3.zero;
			}
        }

		if ( startPos != Vector3.zero )
		{
			// 左クリックが押し続られてる時
			if (Input.GetMouseButton(0))
			{
				if (chageTime + Time.deltaTime < maxChageTime)
				{
					chageTime += Time.deltaTime;
				}
				else
				{
					if (!chaged)
					{
						// Chage完了の音を鳴らす
						audioSource.PlayOneShot(SE_maxChage);
						chaged = true;
					}
				}

				// Fever中なら常に最大チャージ
				if (isFever)
				{
					chageTime = maxChageTime;
					chaged = true;
				}


				// 円状に加速
				float x = 1 - (chageTime / maxChageTime);
				float y = Mathf.Sqrt(1 - x * x);

				float speed = y * maxSpeed;

				// カメラの座標を取得?
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				// よくわかんないけどカメラの座標をベクトルに変換してるんでしょ
				Vector3 dir = ray.direction.normalized;

				endPos = Input.mousePosition;
				dragForce = (startPos - endPos) * 1.5f;
				dragForce.z = dir.z * maxSpeed;
				dragForce *= y;

				// 予測弾道を表示
				trajectory(dragForce);

				// 発射台を今向いてる向きに合わせる
				GameObject lookAt = GameObject.Find("lookAtTrajectory");
				if (lookAt != null)
				{
					this.transform.LookAt(lookAt.transform);
				}
			}

			// 左クリックを離した時
			if (Input.GetMouseButtonUp(0))
			{
				chaged = false;
				chageTime = 0;
				audioSource.PlayOneShot(SE_shoot);

				// ゲーム終了時は打てない
				if (canShoot)
				{
					// 弾丸の複製
					GameObject bullets = GameObject.Instantiate(bullet) as GameObject;

					//if (true)
					if (skl001_throwBomb)
					{
						// skl: 爆弾の複製!
						bullets = GameObject.Instantiate(bomb) as GameObject;

						// 一時的にチャージなし
						chageTime = maxChageTime;

						model.enabled = true;
						bombModel.enabled = false;
						skl001_throwBomb = false;
					}
					// Rigidbodyに力を加えて発射
					bullets.GetComponent<Rigidbody>().AddForce(dragForce);

					// 弾丸の位置を調整
					bullets.transform.position = muzzle.position;
				}
			}
		}
    }

	public void gotoFever()
	{
		isFever = true;
	}

	public void endFever()
	{
		isFever = false;
	}


    // ゲーム初期化 GameController から呼ばれる
    public void onInit()
    {
        canShoot = true;
    }

    // ゲーム終了時 GameController から呼ばれる
    public void onEnd()
    {
        canShoot = false;
    }
}