using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : MonoBehaviour {

	public int skillNum;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // スキルを発動！
    public void invoke()
    {
		switch (skillNum)
		{
		case 1:
			Skill.skillFlag[(int)skillID.FIREWORKS] = true;
			break;
		case 2:
			Skill.skillFlag[(int)skillID.THROW_BOMB] = true;
			break;
		default:
			Debug.Log ("must set 1 or 2 to skillNum");	
			break;
		}
    }
}
