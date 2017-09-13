using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float maxTime;
    public static float time;
    private Text text;

    private bool isEnd;

    private AudioSource audioSource;

    public AudioClip SE_1;
    public AudioClip SE_2;
    public AudioClip SE_3;
    public AudioClip SE_4;
    public AudioClip SE_5;
    public AudioClip SE_finish;

    private AudioClip[] SE_counts = new AudioClip[10];

    // Use this for initialization
    void Start()
    {
        onInit();
        text = GetComponent<Text>();

		time += 0.99f;

        audioSource = gameObject.GetComponent<AudioSource>();
        SE_counts[0] = SE_finish;
        SE_counts[1] = SE_1;
        SE_counts[2] = SE_2;
        SE_counts[3] = SE_3;
        SE_counts[4] = SE_4;
        SE_counts[5] = SE_5;
    }

    // Update is called once per frame
    void Update()
    {
        float oldTime = time;
        time -= Time.deltaTime;

        // カウントダウン
        if (  1 < time && time < 6.0f
            && ( (int)oldTime != (int)time) )
        {
            audioSource.PlayOneShot(SE_counts[(int)time]);
        }

        int timeInt = (int)time; // なんかなぁ....

        // GameController クラスでやることにしたよ
        if (time < 1)
        {
            if (!isEnd)
            {
                audioSource.PlayOneShot(SE_counts[0]);
                GameObject.Find("GameController").SendMessage("gotoEnd");
                text.text = timeInt.ToString();
                isEnd = true;
            }
        } else
        {
            text.text = timeInt.ToString();
        }
    }

    public static void addTime(float value)
    {
        if (time > 0)
        {
            time += value;
        }
    }

    void onInit()
    {
        time = maxTime;
        isEnd = false;
    }
}
