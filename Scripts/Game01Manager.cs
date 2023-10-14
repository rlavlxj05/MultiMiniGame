using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.IO;
public class Game01Manager : MonoBehaviour
{
    public GameObject[] Players; // 플레이어
    public Player1[] PlayerList;
    public PlayerManager[] PlayerMg;
    public bool[] Check;
    public List<PlayerManager> script;
    public float launchForce = 10f;


    void Start()
    {
        StartCoroutine(SpawnProjectile());
        StartCoroutine(managerGm());
        Invoke("startGm", 2);
        for (int i = 0; i < Players.Length; i++)
        {
            Players[i].transform.position = new Vector3(0, 0, 0);
        }
    }

    void Update()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        PlayerMg = FindObjectsOfType<PlayerManager>();
    }

    IEnumerator managerGm()
    { 
        yield return new WaitForSeconds(3f);

        while (true)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                Player1 playerScript = Players[i].GetComponent<Player1>();

                if (playerScript != null)
                {
                    Check[i] = playerScript.Check;
                }

                if (Check[i])
                {
                    if (!script.Contains(PlayerMg[i]))
                    {
                        script.Add(PlayerMg[i]);
                    }
                }

                int falseCount = CountFalseCheck();

                if (falseCount == 1)
                {
                    playerScript.Check = true;
                }

                if (falseCount == 0)
                {
                    score();
                    yield return new WaitForSeconds(1f);
                    foreach (GameObject obj in Players)
                    {
                        Destroy(obj);
                    }
                    yield return new WaitForSeconds(1f);
                    Debug.Log("씬이동");
                    SceneManager.LoadScene(2);
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
    int CountFalseCheck()
    {
        int count = 0;

        for (int i = 0; i < Check.Length; i++)
        {
            if (!Check[i])
            {
                count++;
            }
        }

        return count;
    }
    public void score()
    {
        for (int i = script.Count - 1; i >= 0; i--)
        {
            int playerIndex = Array.IndexOf(PlayerMg, script[i]);
            PlayerManager playerManager = script[i];
            if (playerManager != null)
            {
                int score = 3 - playerIndex;
                playerManager.SetScore(score);
            }
        }
    }

    void startGm()
    {
        Check = new bool[Players.Length];
    }
    IEnumerator SpawnProjectile()
    {
        yield return new WaitForSeconds(3f);

        while (true)
        {
            float angle = UnityEngine.Random.Range(30, 60);
            float angle1 = UnityEngine.Random.Range(-40, 40);

            GameObject projectile = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Game01Obj"), new Vector3(0, 1, 14), Quaternion.identity);

            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            Vector3 launchDirection = Quaternion.Euler(angle, angle1, 0) * transform.forward;
            rb.velocity = launchDirection * launchForce;

            Destroy(projectile, 5f);

            yield return new WaitForSeconds(1);
        }
    }

}
