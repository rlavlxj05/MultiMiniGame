using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using System.Linq;

public class Game02Manager : MonoBehaviour
{
    public GameObject[] Players; //플레이어
    public Player2[] PlayerList; //플레이어 스크립트
    public int[] Score; //현재 점수
    public PlayerManager[] PlayerMg; //최종 점수
    public List<PlayerManager> script;
    public float time; //시간

    private void Update()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        Score = new int[Players.Length];
        PlayerList = FindObjectsOfType<Player2>();
        PlayerMg = FindObjectsOfType<PlayerManager>();

        Invoke("Gametime", 2);

        if (time >= 0.0f)
        {
            score();
        }
    }

    void score()
    {
        for (int i = 0; i < Players.Length; i++)
        {
            Player2 player2 = PlayerList[i];
            Score[i] = player2.BodyParts.Count;
        }
    }

    void Gametime()
    {
        time += Time.deltaTime;

        if (Mathf.FloorToInt(time) != Mathf.FloorToInt(time - Time.deltaTime))
        {
            for (int i = 0; i < 1; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-20f, 20f), 0, Random.Range(-20f, 20f));
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Game02Obj"), spawnPosition, Quaternion.identity);
            }
        }
    }
}
