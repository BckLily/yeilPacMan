using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Red = 0,
    Pink,
    SkyBlue,
    Orange,
}

public enum EnemyState
{
    Wait = -1,
    Chase = 0,
    Scatter,
    Run,
    Die,

}

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent agent; // navmeshagent
    [SerializeField] protected Vector3 targetPos; // chase ������ �� ��ǥ������ vector3 ��ǥ
    [SerializeField] protected Transform scatterTr; // Scatter ������ �� ��ǥ������ Transform
    [SerializeField] protected EnemyState enemyState = EnemyState.Wait; // ó���� ��� ���·� �����Ѵ�.
    [SerializeField] protected EnemyType enemyType; // ���ʹ� ����
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Collider _collider;
    [SerializeField] protected MeshRenderer _renderer;
    [SerializeField] protected float playerTargetDist;
    [SerializeField] protected float stateMaintainTime; // ���¸� �����ϴ� �ð�

    protected float maintainTime = 0f;

    protected float maxXPosition;
    protected float maxZPosition;

    [SerializeField] protected Matrix4x4 runMatrix = new Matrix4x4();

    [SerializeField] protected Color preMaterialColor;


    /// <summary>
    /// Enemy�� ���¸� �����ϴµ� ����ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    protected IEnumerator EnemyStateChange()
    {
        while (true)
        {
            switch (enemyState)
            {
                case EnemyState.Wait:
                    // waitTime�� ������ Scatter ���·� ����
                    yield return null;

                    if (enemyState == EnemyState.Die) { continue; }

                    if (maintainTime >= stateMaintainTime)
                    {
                        maintainTime -= stateMaintainTime;
                        enemyState = EnemyState.Scatter;
                        stateMaintainTime = 4f;
                    }

                    break;
                case EnemyState.Chase:
                    // 16�� �ڿ� Scatter ���·� ����
                    yield return null;

                    if (enemyState == EnemyState.Die) { continue; }

                    if (maintainTime >= stateMaintainTime)
                    {
                        maintainTime -= stateMaintainTime;
                        enemyState = EnemyState.Scatter;
                        stateMaintainTime = 4f;
                    }
                    break;
                case EnemyState.Scatter:
                    // 4�� �ڿ� Chase ���·� ����
                    yield return null;

                    if (enemyState == EnemyState.Die) { continue; }

                    if (maintainTime >= stateMaintainTime)
                    {
                        maintainTime -= stateMaintainTime;
                        enemyState = EnemyState.Chase;
                        stateMaintainTime = 16f;
                    }

                    break;
                case EnemyState.Run:
                    // �÷��̾��� ���� ����?�� ������ chase ���·� ����
                    if (GameManager.Instance.playerState == PlayerState.Idle)
                        enemyState = EnemyState.Chase;

                    break;
                case EnemyState.Die:
                    // EnemyRespawn��ġ�� �����ϸ� chase ���·� ����
                    // ��� ���°� �Ǹ� Collider�� ���ٰ� ���߿� �ٽ� �Ѿ���.
                    moveSpeed = 16f;
                    agent.speed = moveSpeed;

                    if (Vector3.Distance(this.transform.position, GameManager.Instance.enemyRevivePoint.position) <= 0.75f)
                    {
                        enemyState = EnemyState.Chase;
                        moveSpeed = 5f;
                        agent.speed = moveSpeed;
                        _renderer.material.color = preMaterialColor;
                    }

                    break;
            }
            yield return null;
        }
    }

    /// <summary>
    /// ������Ʈ�� �޾ƿ��� ����.
    /// </summary>
    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _collider = GetComponent<Collider>();
        _renderer = GetComponent<MeshRenderer>();

        runMatrix = new Matrix4x4(new Vector3(-1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, -1), Vector3.zero);
        //Debug.Log(runMatrix);
    }

    /// <summary>
    /// ���ʹ��� ���� ������ �ϰ� ����.
    /// </summary>
    protected virtual void OnEnable()
    {
        StartCoroutine(EnemyStateChange());
        StartCoroutine(SetDestination());
        moveSpeed = 5f;
        agent.speed = moveSpeed;
        playerTargetDist = 6.5f;
        maxXPosition = 12.75f;
        maxZPosition = 10.25f;

        preMaterialColor = _renderer.material.color;
    }


    /// <summary>
    /// �ֱ������� ��ǥ���� �����ϴ� �ڷ�ƾ.
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerator SetDestination();

    protected void Update()
    {
        targetPos = agent.destination;
        maintainTime += Time.deltaTime;
    }

    public void EnemyRun()
    {
        StartCoroutine(RunState());

        // ���׸��� �� �������ִ� ���� �ʿ���.

    }

    protected IEnumerator RunState()
    {
        float time = 0f;

        if (enemyState == EnemyState.Die) { yield break; }

        _renderer.material.color = new Color(0.75f, 0.75f, 0.75f);

        while (time <= GameManager.Instance.InvincibilityTime)
        {
            time += Time.deltaTime;
            if (enemyState == EnemyState.Die) { yield break; }
            enemyState = EnemyState.Run;
            yield return null;
        }

        _renderer.material.color = preMaterialColor;

        //EnemyChase();
    }

    protected void EnemyChase()
    {
        enemyState = EnemyState.Chase;
    }

    public void EnemyDie()
    {
        _renderer.material.color = new Color(0.25f, 0.25f, 0.25f, 0.1f);

        enemyState = EnemyState.Die;
    }

}
