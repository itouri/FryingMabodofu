using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonOneMore : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void restart()
    {
        // こうしてしまえばすべて最初からになる
        SceneManager.LoadScene("Game");
    }
}
