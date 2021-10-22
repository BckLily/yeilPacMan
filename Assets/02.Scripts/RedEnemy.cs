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
                    // ���� ����.

                    break;
                case EnemyState.Chase:
                    // �÷��̾� �i�ư�
                    agent.SetDestination(GameManager.Instance.playerTr.position);

                    break;
                case EnemyState.Scatter:
                    // Scatter ����Ʈ���� �Դٰ��� �ϴ� ��ǥ���� ����.
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