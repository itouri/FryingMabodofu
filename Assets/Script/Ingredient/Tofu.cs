using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tofu : Ingredient
{
	public void create(int genre, int kind, Vector3 initPos, Vector3 initRota, bool isFever)
    {
		base.create(genre, kind, initPos, isFever);
    }
}
