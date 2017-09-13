using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum skillKind
{
    SHOOT_TOFU = 0,           // 豆腐
    SHOOT_GREEN_ONION,       // ネギ
    SHOOT_MEAT,               // 肉
    SHOOT_SPICE,              // 豆板醤とか
    SHOOT_CONDIMENT,    // 中華の素 Cookdo とかのこと
    SHOOT_ANYTHING,
    PASSED_TIME,
    END,
}

// 現状考えているスキル
public enum skillID
{
    THROW_BOMB = 0,         // 爆弾を投げる
    FIREWORKS,              // 打ち上げ花火
    RAZER,                  // レーザーをぶっ放す
    TIME_EXTEND,            // 時間を追加
    PROMOTE_INGRE_CREATE,   // 生成される具材を増やす
    GOTO_FEVER,             // FEVERモードへ移行
    END
}

public class skl
{
    // 必要具材数
    int tofu, grren_onion, meat, spice;

    // 発動条件
    int pattern;
}

public class skl_ThrowBomb : skl
{
    float radius; // 爆発半径
    int remain; // 繰り返す数
}

public class skl_Fireworks : skl
{
    float radius; // 爆発半径
    int remain; // 繰り返す数
}

public class skl_razer : skl
{
    float radius; // 爆発半径
}


