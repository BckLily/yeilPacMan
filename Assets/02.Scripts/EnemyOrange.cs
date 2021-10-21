using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOrange : EnemyBase
{
    protected override void Start()
    {
        base.Start();
        StartCoroutine(SetType());
    }

    private IEnumerator SetType()
    {
        while (true)
        {
            _type = (EnemyType)Random.Range(0, System.Enum.GetNames(typeof(EnemyType)).Length - 1);

            yield return new WaitForSeconds(15f);
        }
    }

}
