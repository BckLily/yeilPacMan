using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreItem : ItemBase
{
    private float score;

    private void Awake()
    {
        score = 10f;
    }

    protected override void GetItem()
    {
        // 스코어 증가
        GameManager.Instance.IncScore(score);

    }

}
