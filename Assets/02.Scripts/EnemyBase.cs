using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Chase = 0,
    Run,
    Die,
}

public enum EnemyType
{
    Red,
    Pink,
    SkyBlue,
    Orange,
}


public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected EnemyState _state = EnemyState.Chase;
    [SerializeField] protected EnemyType _type = EnemyType.Red;
    protected NavMeshAgent agent;
    [SerializeField] protected Vector3 destinationPos;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    protected void Update()
    {
        SetEnemyState();
        SetDesination(_type);

    }

    protected void SetEnemyState()
    {
        switch (GameManager.Instance.playerState)
        {
            case PlayerState.Idle:
                _state = EnemyState.Chase;

                break;
            case PlayerState.Invincibility:
                _state = EnemyState.Run;

                break;
        }
    }


    protected void Movement()
    {
        switch (_state)
        {
            case EnemyState.Chase:
                agent.SetDestination(destinationPos);

                break;
            case EnemyState.Run:
                if (Vector3.Distance(destinationPos, Vector3.zero) < 5f)
                {
                    Vector3 pos = transform.position - GameManager.Instance.playerTr.position;
                    destinationPos = transform.position + pos;
                }
                else
                {
                    destinationPos = -GameManager.Instance.playerTr.position;
                }

                break;
            case EnemyState.Die:
                Time.timeScale = 0f;
                destinationPos = GameManager.Instance.enemyRevivePoint.position;
                break;
        }
    }


    protected void SetDesination(EnemyType enemyType)
    {


        switch (enemyType)
        {
            case EnemyType.Red:
                destinationPos = GameManager.Instance.playerTr.position;

                break;
            case EnemyType.Pink:
                destinationPos = GameManager.Instance.playerForwardTr.position;

                break;
            case EnemyType.SkyBlue:
                Vector3 pos = GameManager.Instance.playerTr.position - GameManager.Instance.redEnemyTr.position;
                destinationPos = GameManager.Instance.playerTr.position + pos.normalized;
                if (destinationPos.x <= -14f) { destinationPos.x = -14f; }
                else if (destinationPos.x >= 14f) { destinationPos.x = 14f; }
                if (destinationPos.z <= -11.5f) { destinationPos.z = -11.5f; }
                else if (destinationPos.z >= 11.5f) { destinationPos.z = 11.5f; }

                break;
                //case EnemyType.Orange:
                //    Movement((EnemyType)Random.Range(0, System.Enum.GetNames(typeof(EnemyType)).Length - 1));
                //    break;
        }
        if (agent.remainingDistance <= 15f)
        {
            Movement();
        }


    }



}
