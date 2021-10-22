using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedEnemy : EnemyBase
{

    protected override IEnumerator SetDestination()
    {
        while (true)
        {

            switch (enemyState)
            {
                case EnemyState.Wait:
                    // 동작 없음.

                    break;
                case EnemyState.Chase:
                    // 플레이어 쫒아감
                    agent.SetDestination(GameManager.Instance.playerTr.position);

                    break;
                case EnemyState.Scatter:
                    // Scatter 포인트에서 왔다갔다 하는 목표지점 설정.
                    agent.SetDestination(scatterTr.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f)));

                    break;
                case EnemyState.Run:
                    agent.SetDestination(runMatrix.MultiplyPoint3x4(GameManager.Instance.playerTr.position));
                    //Debug.Log(runMatrix);
                    //Debug.Log(runMatrix.MultiplyPoint3x4(GameManager.Instance.playerTr.position));

                    break;
                case EnemyState.Die:
                    agent.SetDestination(GameManager.Instance.enemyRevivePoint.position);

                    break;
            }
            yield return new WaitForSeconds(0.25f);
        }

    }


}