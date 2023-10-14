using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
public class LobbyManager : MonoBehaviourPunCallbacks
{
    public GameObject StartButton;
    public GameObject[] player;

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartButton.SetActive(true);
        }
        else
        {
            StartButton.SetActive(false);
        }
    }

    public void Button2()
    {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        player = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject obj in player)
        {
            Destroy(obj); // °´Ã¼ Á¦°Å
        }
        yield return new WaitForSeconds(1f);
        int randomScene = Random.Range(3, 4);
        SceneManager.LoadScene(randomScene);
    }
}
