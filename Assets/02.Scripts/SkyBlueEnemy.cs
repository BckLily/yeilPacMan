using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBlueEnemy : EnemyBase
{
    [SerializeField] private Transform redTransform; // 빨간색의 위치

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
                    // 플레이어의 조금 앞을 기준으로 빨강과 점대칭
                    // 빨강의 위치를 알아야 함.
                    if (Vector3.Distance(transform.position, GameManager.Instance.playerTr.position) >= playerTargetDist)
                    {
                        Vector3 centerPos = GameManager.Instance.playerTr.position + (GameManager.Instance.playerForwardTr.localPosition * 4);
                        centerPos.x = Mathf.Clamp(centerPos.x, -maxXPosition, maxXPosition);
                        centerPos.z = Mathf.Clamp(centerPos.z, -maxZPosition, maxZPosition);
                        agent.SetDestination((2 * centerPos) - redTransform.position);
                    }
                    // 플레이어와 가까우면 플레이어를 쫒아감.
                    else
                        agent.SetDestination(GameManager.Instance.playerTr.position);

                    break;
                case EnemyState.Scatter:
                    // Scatter 포인트에서 왔다갔다 하는 목표지점 설정.
                    agent.SetDestination(scatterTr.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f)));

                    break;
                case EnemyState.Run:
                    if (Vector3.Distance(transform.position, GameManager.Instance.playerTr.position) >= playerTargetDist)
                    {
                        Vector3 centerPos = GameManager.Instance.playerTr.position + (GameManager.Instance.playerForwardTr.localPosition * 4);
                        centerPos.x = Mathf.Clamp(centerPos.x, -maxXPosition, maxXPosition);
                        centerPos.z = Mathf.Clamp(centerPos.z, -maxZPosition, maxZPosition);
                        agent.SetDestination(runMatrix.MultiplyPoint3x4((2 * centerPos) - redTransform.position));
                    }
                    // 플레이어와 가까우면 플레이어를 쫒아감.
                    else
                        agent.SetDestination(runMatrix.MultiplyPoint3x4(GameManager.Instance.playerTr.position));


                    break;
                case EnemyState.Die:

                    agent.SetDestination(GameManager.Instance.enemyRevivePoint.position);

                    break;
            }
            yield return new WaitForSeconds(0.25f);
        }
    }
}
