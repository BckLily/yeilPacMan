using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get { return instance; }
    }

    public PlayerState playerState = PlayerState.Idle;

    public Transform redEnemyTr;

    public Transform playerTr;
    public Transform playerForwardTr;
    public Transform playerBackTr;

    public Transform enemyRevivePoint;

    [SerializeField] Text scoreText;
    private float score = 0f;
    [SerializeField] Text remainLifeText;
    private int remainLife = 4;

    public float InvincibilityTime = 5.5f;

    [SerializeField] Transform gameOverPanel;
    [SerializeField] Transform gameClearPanel;

    public List<ItemBase> itemList;
    public int remainItem => itemList.Count;

    private void Init()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this.gameObject);
    }

    private void Awake()
    {
        Init();
        SetScore();
        //SetRemainLife();
    }

    private void OnEnable()
    {
        Time.timeScale = 1f;
        var items = GameObject.FindGameObjectsWithTag("ITEM");
        foreach (var item in items)
        {
            itemList.Add(item.GetComponent<ItemBase>());
        }
    }

    private void Update()
    {

        if (gameOverPanel.gameObject.activeSelf && Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
            Time.timeScale = 1f;
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE_WIN
            Application.Quit();
#endif
        }

        //Vector3 vector3 = new Vector3(10f, 10f, 10f);
        //Matrix4x4 matrix4X4 = new Matrix4x4(new Vector3(-1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, -1), Vector3.zero);
        //matrix4X4.MultiplyPoint3x4(vector3);

        //Debug.Log(matrix4X4.MultiplyPoint3x4(vector3));
        //Debug.Log(vector3);
        //Debug.Log(matrix4X4);
    }

    public void IncScore(float _score)
    {
        score += _score;
        SetScore();
    }

    private void SetScore()
    {
        scoreText.text = string.Format($"{score:N0}");
    }

    public void DecRemainLife()
    {
        remainLife -= 1;
        SetRemainLife();
    }

    private void SetRemainLife()
    {
        remainLifeText.text = string.Format($"{remainLife.ToString()}");
    }


    public void OverPowerPlayer()
    {
        // 플레이어를 무적 처리
        playerState = PlayerState.Invincibility;
        // 적들 도망가게
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("ENEMY");

        foreach (var enemy in enemies)
        {
            enemy.GetComponent<EnemyBase>().EnemyRun();
        }

        Invoke("IdlePlayer", InvincibilityTime);
    }


    public void IdlePlayer()
    {
        playerState = PlayerState.Idle;
    }

    public void GameOver()
    {
        gameOverPanel.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GameClear()
    {
        gameClearPanel.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }



}
