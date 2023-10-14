using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance = null;
    private Player1 player1;
    private Player2 player2;
    public Transform CamObj;
    public int score = 0;
    PhotonView PV;
    Animation ani;
    internal string playerName;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        PV = GetComponent<PhotonView>();
        ani = GetComponent<Animation>();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (PV.IsMine)
        {
            playerspwn();
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (!PV.IsMine)
            return;

        if (player1 != null)
        {
            player1.Move();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(CamCont());
        if (PV.IsMine)
        {
            Invoke("playerspwn", 1);
        }
    }

    public void playerspwn()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        switch (currentSceneIndex)
        {
            case 2:            
                break;
            case 1:
            case 3:
                GameObject playerObject1 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player1"), Vector3.zero, Quaternion.identity);
                player1 = playerObject1.GetComponent<Player1>();
                break;
            case 4:
                GameObject playerObject2 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player2"), Vector3.zero, Quaternion.identity);
                player2 = playerObject2.GetComponent<Player2>();
                break;
        }
    }

    IEnumerator CamCont()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        switch (currentSceneIndex)
        {
            case 1:
                CamObj.position = new Vector3(10, 0, -11);
                CamObj.rotation = Quaternion.Euler(0, -44, 0);
                break;

            case 2:
                CamObj.position = new Vector3(0, -5, -6);
                CamObj.rotation = Quaternion.Euler(0, 0, 0);
                break;

            case 3:

                CamObj.position = new Vector3(2, 0, 9);
                CamObj.rotation = Quaternion.Euler(-11, -21, 0);
                yield return new WaitForSeconds(1f);
                ani.Play("camOut01");
                break;
            case 4:

                CamObj.position = new Vector3(0, 19, -30);
                CamObj.rotation = Quaternion.Euler(45, 0, 0);
                break;
        }
    }

    private int GetScore()
    {
        return score;
    }

    public void SetScore(int newScore)
    {
        score = newScore;
    }

    void CreateController()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player1"), Vector3.zero, Quaternion.identity);
    }

}