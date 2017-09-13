using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OrderList : MonoBehaviour
{
    private Text textName;
    private Text textIngre;

    private string strName, strIngre;
    private int[] needIngre = new int[4];

    private char[] wMarks = {'□', '○', '△', '▽'};
    private char[] bMarks = {'■', '●', '▲', '▼'};


    // Use this for initialization
    void Start () {
        textName = transform.Find("Name").GetComponent<Text>();
        textName.text = strName;
        textIngre = transform.Find("Ingredient").GetComponent<Text>();
        textIngre.text = strIngre;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void setName (string str)
    {
        strName = str;
    }

    void setIngre (int[] args)
    {
        string str = "";

        str += new string('□',  args[(int)Kind.TOFU       ]);
        str += new string('○',  args[(int)Kind.GREEN_ONION]);
        str += new string('△', args[(int)Kind.MEAT       ]);
        str += new string('▽', args[(int)Kind.SPICE      ]);

        strIngre = str;

        args.CopyTo(needIngre,0);
    }

    void updateUI(int[] storedIngre)
    {
        string str = "";
        // 具材の種類でループ
        for (int i=0; i<4; i++)
        {
            if ( storedIngre[i] >= needIngre[i] )
            {
                str += new string(bMarks[i], needIngre[i]);
            }
            else
            {
                str += new string(bMarks[i], storedIngre[i]);
                str += new string(wMarks[i], needIngre[i] - storedIngre[i]);
            }  
        }
        textIngre.text = str;
    }
}
