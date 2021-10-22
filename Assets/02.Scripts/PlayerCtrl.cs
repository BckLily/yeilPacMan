using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle = 0,
    Invincibility,
}


public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    enum PlayerDir
    {
        Up = 0,
        Down,
        Left,
        Right
    }

    [SerializeField] List<Transform> playerDirList = new List<Transform>();
    [SerializeField] PlayerState playerState => GameManager.Instance.playerState;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.playerTr = GetComponent<Transform>();
        GameManager.Instance.playerForwardTr = playerDirList[(int)PlayerDir.Up];
        GameManager.Instance.playerBackTr = playerDirList[(int)PlayerDir.Down];
        moveSpeed = 5.5f;
    }

    private void LateUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(h) == 1)
        {
            GameManager.Instance.playerForwardTr = playerDirList[(int)((h > 0) ? PlayerDir.Right : PlayerDir.Left)];
            GameManager.Instance.playerBackTr = playerDirList[(int)((h > 0) ? PlayerDir.Left : PlayerDir.Right)];

            v = 0f;
        }
        else if (Mathf.Abs(v) == 1)
        {
            GameManager.Instance.playerForwardTr = playerDirList[(int)((v > 0) ? PlayerDir.Up : PlayerDir.Down)];
            GameManager.Instance.playerBackTr = playerDirList[(int)((v > 0) ? PlayerDir.Down : PlayerDir.Up)];
        }

        Vector3 moveDir = new Vector3(h, 0f, v);
        moveDir.Normalize();

        transform.Translate(moveDir * Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ENEMY"))
        {
            switch (playerState)
            {
                case PlayerState.Idle:
                    GameManager.Instance.GameOver();

                    break;
                case PlayerState.Invincibility:
                    other.GetComponent<EnemyBase>().EnemyDie();

                    break;
            }
        }

    }


}
