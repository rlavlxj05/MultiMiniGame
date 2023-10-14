using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class test : MonoBehaviour
{
    public PlayerManager[] Playerscript;
    public GameObject[] Players;

    public Transform ScoreContent;
    public Slider ScorePrefab;
    public Transform NameContent;
    public GameObject NamePrefab;
    Slider[] scoreObject;

    public float[] maxscore; // 현재 점수
    public float[] minscore; // 초기점수
    public bool[] counting; // 체크

    private void Start()
    {
        Playerscript = FindObjectsOfType<PlayerManager>();
        Players = Players = GameObject.FindGameObjectsWithTag("Manager");
        Invoke("score", 2);
    }

    private void Update()
    {
        for (int i = 0; i < minscore.Length; i++)
        {
            minscore[i] += Time.deltaTime;

            if (minscore[i] >= maxscore[i])
            {
                minscore[i] = maxscore[i];
                counting[i] = true;
                if (!counting[i])
                {
                    Debug.Log("씬이동");
                    //SceneManager.LoadScene(3);
                }
            }
            scoreObject[i].value = minscore[i];
        }
    }

    void score()
    {
        maxscore = new float[Playerscript.Length];
        minscore = new float[Playerscript.Length];
        counting = new bool[Playerscript.Length];
        scoreObject = new Slider[Playerscript.Length];

        for (int i = 0; i < Playerscript.Length; i++)
        {
            counting[i] = false; // 카운팅 시작
            maxscore[i] = Playerscript[i].score;
            scoreObject[i] = Instantiate(ScorePrefab, ScoreContent);
        }

        Player[] players = PhotonNetwork.PlayerList;
        foreach (Player player in players)
        {
            GameObject nameObject = Instantiate(NamePrefab, NameContent);
            Text text = nameObject.GetComponent<Text>();
            text.text = player.NickName;
        }
    }
}