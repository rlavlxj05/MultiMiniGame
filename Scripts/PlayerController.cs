using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private float speed;
    public int score = 0;

    public float moveX;
    public float moveZY;

    Rigidbody rd;
    Vector3 moveVec;
    PhotonView PV;
    //Animator ani;

    private void Awake()
    {
        //ani = GetComponent<Animator>();
        rd = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
    
    void Update()
    {
        if (!PV.IsMine)
            return;
        //Move();
    }

    public int GetScore()
    {
        return score;
    }

    public void SetScore(int newScore)
    {
        score = newScore;
    }
    public void Move()
    {
        if (PV.IsMine)
        {
            float _moveDirX = Input.GetAxisRaw("Horizontal");
            float _moveDirZ = Input.GetAxisRaw("Vertical");

            moveVec = new Vector3(_moveDirX, 0, _moveDirZ).normalized;

            transform.position += moveVec * speed * Time.deltaTime;

            transform.LookAt(transform.position + moveVec);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            EliminatePlayer();
            PV.RPC("EliminatePlayer", RpcTarget.All);
        }
    }

    IEnumerator Launch()
    {
        float maxRandomAngle = 70f;
        float launchForce = 90f;
        Vector3 launchDirection = Quaternion.Euler(0, -maxRandomAngle, maxRandomAngle) * transform.forward;
        rd.AddForce(launchDirection * launchForce, ForceMode.Impulse);

        yield return new WaitForSeconds(1f);

        Debug.Log("탈락!");
        gameObject.SetActive(false);
    }

    [PunRPC]
    public void EliminatePlayer()
    {
        StartCoroutine(Launch());
    }
}