using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class Player2 : MonoBehaviour
{
    public float MoveSpeed = 1;
    public float SteerSpeed = 180;
    public float BodysSpeed = 1;
    public int Gap = 10;

    public List<GameObject> BodyParts = new List<GameObject>();
    private List<Vector3> positionsMistory = new List<Vector3>();

    void Update()
    {
        transform.position += transform.forward * MoveSpeed * Time.deltaTime;

        float steerDirection = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * steerDirection * SteerSpeed * Time.deltaTime);

        positionsMistory.Insert(0, transform.position);

        int index = 0;
        foreach (var body in BodyParts)
        {
            Vector3 point = positionsMistory[Mathf.Min(index * Gap, positionsMistory.Count - 1)];
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * BodysSpeed * Time.deltaTime;
            body.transform.LookAt(point);
            index++;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(collision.gameObject);
            GrowSnake();
        }
    }

  /*  private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Body"))
        {
            //gameObject.SetActive(false);
            foreach (var body in BodyParts)
            {
                body.SetActive(false);
            }
        }
    }*/

    void GrowSnake()
    {
        //GameObject body = Instantiate(BodyPrefab);
        GameObject body = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Body"), new Vector3(0, -5, 0), Quaternion.identity);
        BodyParts.Add(body);
    }
}
