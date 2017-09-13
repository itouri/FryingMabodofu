using UnityEngine;
using System.Collections;

public class CookingListMasterTable : MasterTableBase<CookingListMaster>
{
	// Resources/ 以下のパスを書く
	private static readonly string FilePath = "CSV/CookingList";
	public void Load() { Load(FilePath); }
}

public class CookingListMaster : MasterBase
{
	// CSVファイルの第一行を下の名前に合わせないと行けない
	public int ID { get; private set; }
	public string Name { get; private set; }

    // 必要具材
    //public int[] Ingre { get; private set; }
    public int Tofu { get; private set; }
    public int GreenOnion { get; private set; }
    public int Meet { get; private set; }
    public int Spice { get; private set; }

    public int Value { get; private set; }
	public int Score { get; private set; }
}