using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBlueEnemy : EnemyBase
{
    [SerializeField] private Transform redTransform; // �������� ��ġ

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
                    // �÷��̾��� ���� ���� �������� ������ ����Ī
                    // ������ ��ġ�� �˾ƾ� ��.
                    if (Vector3.Distance(transform.position, GameManager.Instance.playerTr.position) >= playerTargetDist)
                    {
                        Vector3 centerPos = GameManager.Instance.playerTr.position + (GameManager.Instance.playerForwardTr.localPosition * 4);
                        centerPos.x = Mathf.Clamp(centerPos.x, -maxXPosition, maxXPosition);
                        centerPos.z = Mathf.Clamp(centerPos.z, -maxZPosition, maxZPosition);
                        agent.SetDestination((2 * centerPos) - redTransform.position);
                    }
                    // �÷��̾�� ������ �÷��̾ �i�ư�.
                    else
                        agent.SetDestination(GameManager.Instance.playerTr.position);

                    break;
                case EnemyState.Scatter:
                    // Scatter ����Ʈ���� �Դٰ��� �ϴ� ��ǥ���� ����.
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
                    // �÷��̾�� ������ �÷��̾ �i�ư�.
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
