using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ゲームを管理するクラス
public class GameController : MonoBehaviour {

    private State state;

    private enum State
    {
        INIT = 0,
        PLAY,
        END,
        RESULT,
    }

	// Use this for initialization
	void Start () {
        gotoInit();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void gotoInit()
    {
        state = State.INIT;
        GameObject.Find("IngredientCreator").SendMessage("onInit");
        GameObject.Find("Cannon").SendMessage("onInit");
    }

    // Timer() が呼んでる
    public void gotoEnd()
    {
        state = State.END;
        GameObject.Find("Cannon").SendMessage("onEnd");
        GameObject.Find("UISystem").SendMessage("onEnd");
    }
}
