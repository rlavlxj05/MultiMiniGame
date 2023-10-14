using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player1 : MonoBehaviour
{
    public static Player1 Instance;
    private PlayerManager playerManager;
    public bool Check;

    PhotonView PV;
    Rigidbody rd;
    Animator Ani;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        rd = GetComponent<Rigidbody>();
        Instance = this;
    }

    public void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");
        float speed = 3;
        Vector3 moveVec = new Vector3(_moveDirX, 0, _moveDirZ).normalized;

        transform.position += moveVec * speed * Time.deltaTime;

        transform.LookAt(transform.position + moveVec);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            PV.RPC("Launch", RpcTarget.All);
            Check = true;
        }
    }

    [PunRPC]
    public void Launch()
    {
        float maxRandomAngle = 70f;
        float launchForce = 90f;
        Vector3 launchDirection = Quaternion.Euler(0, -maxRandomAngle, maxRandomAngle) * transform.forward;
        rd.AddForce(launchDirection * launchForce, ForceMode.Impulse);
    }
}
