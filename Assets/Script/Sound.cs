using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour
{
	public AudioClip SE_1;
	public AudioClip SE_2;
	public AudioClip SE_3;
	public AudioClip SE_4;
	public AudioClip SE_5;
	public AudioClip SE_6;
	public AudioClip SE_7;
	public AudioClip SE_over7;

	public AudioClip SE_bomb;

	private AudioClip[] SE_combos = new AudioClip[10];
	private AudioSource audioSource;

	// Use this for initialization
	void Start ()
	{
		audioSource = gameObject.GetComponent<AudioSource>();
		SE_combos[1] = SE_1;
		SE_combos[2] = SE_2;
		SE_combos[3] = SE_3;
		SE_combos[4] = SE_4;
		SE_combos[5] = SE_5;
		SE_combos[6] = SE_6;
		SE_combos[7] = SE_7;
		SE_combos[8] = SE_over7;
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public void playCombo(int i)
	{
		audioSource.Stop();
		audioSource.PlayOneShot(SE_combos[i]);
	}

	public void playBomb()
	{
		audioSource.PlayOneShot(SE_bomb);
	}
}

