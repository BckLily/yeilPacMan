using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeEnemy : EnemyBase
{
    [SerializeField] private Transform redTransform;

    private IEnumerator EnemyTypeChange()
    {
        while (true)
        {
            // Orange�� ������ 3������ Ÿ���� ����Ѵ�.
            enemyType = (EnemyType)Random.Range(0, System.Enum.GetNames(typeof(EnemyType)).Length - 1);
            yield return new WaitForSeconds(30);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(EnemyTypeChange());
    }


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

                    switch (enemyType)
                    {
                        case EnemyType.Red:
                            // �÷��̾� �i�ư�
                            agent.SetDestination(GameManager.Instance.playerTr.position);

                            break;
                        case EnemyType.Pink:
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
                            // �Ÿ��� ������ �÷��̾�� ����.
                            else
                                agent.SetDestination(GameManager.Instance.playerTr.position);

                            break;
                        case EnemyType.SkyBlue:
                            // �÷��̾��� ���� ���� �������� ������ ����Ī
                            // ������ ��ġ�� �˾ƾ� ��.
                            if (Vector3.Distance(transform.position, GameManager.Instance.playerTr.position) >= playerTargetDist)
                            {
                                Vector3 centerPos = GameManager.Instance.playerTr.position + (GameManager.Instance.playerForwardTr.localPosition * 4);
                                Vector3 destination = (2 * centerPos) - redTransform.position;
                                destination.x = Mathf.Clamp(destination.x, -maxXPosition, maxXPosition);
                                destination.z = Mathf.Clamp(destination.z, -maxZPosition, maxZPosition);
                                agent.SetDestination(destination);
                            }
                            // �÷��̾�� ������ �÷��̾ �i�ư�.
                            else
                                agent.SetDestination(GameManager.Instance.playerTr.position);


                            break;
                    }

                    break;
                case EnemyState.Scatter:
                    // Scatter ����Ʈ���� �Դٰ��� �ϴ� ��ǥ���� ����.
                    agent.SetDestination(scatterTr.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f)));

                    break;
                case EnemyState.Run:
                    switch (enemyType)
                    {
                        case EnemyType.Red:
                            agent.SetDestination(runMatrix.MultiplyPoint3x4(GameManager.Instance.playerTr.position));

                            break;
                        case EnemyType.Pink:
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
                        case EnemyType.SkyBlue:
                            if (Vector3.Distance(transform.position, GameManager.Instance.playerTr.position) >= playerTargetDist)
                            {
                                Vector3 centerPos = GameManager.Instance.playerTr.position + (GameManager.Instance.playerForwardTr.localPosition * 4);
                                Vector3 destination = (2 * centerPos) - redTransform.position;
                                destination.x = Mathf.Clamp(destination.x, -maxXPosition, maxXPosition);
                                destination.z = Mathf.Clamp(destination.z, -maxZPosition, maxZPosition);
                                agent.SetDestination(runMatrix.MultiplyPoint3x4(destination));
                            }
                            else
                                agent.SetDestination(runMatrix.MultiplyPoint3x4(GameManager.Instance.playerTr.position));


                            break;
                    }

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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(targetPos, 0.5f);
    }

}
