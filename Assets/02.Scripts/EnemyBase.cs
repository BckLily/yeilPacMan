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
    [SerializeField] protected Vector3 targetPos; // chase 상태일 때 목표지점의 vector3 좌표
    [SerializeField] protected Transform scatterTr; // Scatter 상태일 때 목표지점의 Transform
    [SerializeField] protected EnemyState enemyState = EnemyState.Wait; // 처음엔 대기 상태로 시작한다.
    [SerializeField] protected EnemyType enemyType; // 에너미 종류
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Collider _collider;
    [SerializeField] protected MeshRenderer _renderer;
    [SerializeField] protected float playerTargetDist;
    [SerializeField] protected float stateMaintainTime; // 상태를 유지하는 시간

    protected float maintainTime = 0f;

    protected float maxXPosition;
    protected float maxZPosition;

    [SerializeField] protected Matrix4x4 runMatrix = new Matrix4x4();

    [SerializeField] protected Color preMaterialColor;


    /// <summary>
    /// Enemy의 상태를 변경하는데 사용하는 코루틴
    /// </summary>
    /// <returns></returns>
    protected IEnumerator EnemyStateChange()
    {
        while (true)
        {
            switch (enemyState)
            {
                case EnemyState.Wait:
                    // waitTime이 끝나면 Scatter 상태로 변경
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
                    // 16초 뒤에 Scatter 상태로 변경
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
                    // 4초 뒤에 Chase 상태로 변경
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
                    // 플레이어의 무적 상태?가 끝나면 chase 상태로 변경
                    if (GameManager.Instance.playerState == PlayerState.Idle)
                        enemyState = EnemyState.Chase;

                    break;
                case EnemyState.Die:
                    // EnemyRespawn위치에 도착하면 chase 상태로 변경
                    // 사망 상태가 되면 Collider를 껐다가 나중에 다시 켜야함.
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
    /// 컴포넌트를 받아오고 있음.
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
    /// 에너미의 상태 변경을 하고 있음.
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
    /// 주기적으로 목표지를 설정하는 코루틴.
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

        // 머테리얼 값 변경해주는 과정 필요함.

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
