using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkEnemy : EnemyBase
{

    protected override IEnumerator SetDestination()
    {
        while (true)
        {

            switch (enemyState)
            {
                case EnemyState.Wait:
                    // ���� ����.

                    break;
                case EnemyState.Chase:
                    // �÷��̾� ���� ����.
                    // �Ÿ��� �ָ� �÷��̾��� ���� ����

                    if (Vector3.Distance(transform.position, GameManager.Instance.playerTr.position) >= playerTargetDist)
                        agent.SetDestination(new Vector3(Mathf.Clamp(GameManager.Instance.playerForwardTr.position.x
                                                         + (GameManager.Instance.playerForwardTr.localPosition.x * 6),
                                                         -maxXPosition, maxXPosition),
                                                         GameManager.Instance.playerForwardTr.position.y,
                                                         Mathf.Clamp(GameManager.Instance.playerForwardTr.position.z
                                                         + (GameManager.Instance.playerForwardTr.localPosition.z * 6),
                                                         -maxZPosition, maxZPosition)));
                    // �Ÿ��� ������ �÷��̾�� ����.` 
                    else
                        agent.SetDestination(GameManager.Instance.playerTr.position);

                    break;
                case EnemyState.Scatter:
                    // Scatter ����Ʈ���� �Դٰ��� �ϴ� ��ǥ���� ����.
                    agent.SetDestination(scatterTr.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f)));

                    break;
                case EnemyState.Run:
                    if (Vector3.Distance(transform.position, GameManager.Instance.playerTr.position) >= playerTargetDist)
                        agent.SetDestination(runMatrix.MultiplyPoint3x4(new Vector3(Mathf.Clamp(GameManager.Instance.playerForwardTr.position.x
                                                                                    + (GameManager.Instance.playerForwardTr.localPosition.x * 6),
                                                                                    -maxXPosition, maxXPosition),
                                                                                    GameManager.Instance.playerForwardTr.position.y,
                                                                                    Mathf.Clamp(GameManager.Instance.playerForwardTr.position.z
                                                                                    + (GameManager.Instance.playerForwardTr.localPosition.z * 6),
                                                                                    -maxZPosition, maxZPosition))));
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

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(targetPos, 0.5f);
    }


}
