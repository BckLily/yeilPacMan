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
        // ���ھ� ����
        GameManager.Instance.IncScore(score);

    }

}
