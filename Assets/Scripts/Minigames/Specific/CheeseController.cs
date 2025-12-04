using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseController : MonoBehaviourPun
{
    [Header("Spawn and Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    [Header("Particles")]
    [SerializeField] private GameObject particlesGameobject;

    [Header("Sprites")]
    [SerializeField] private List<Sprite> availableSprites;

    private int score;

    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        int index = Random.Range(0, availableSprites.Count);
        GetComponent<SpriteRenderer>().sprite = availableSprites[index];
        score = (index + 1) * 5;

        transform.position = new Vector3(
            Random.Range(minX, maxX),
            maxY,
            transform.position.z
        );
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        transform.position += Vector3.down * speed * Time.deltaTime;

        if (transform.position.y <= minY)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    public void RPC_HitCheese(Player p)
    {
        ((ReachScoreGameManager)GameManager.Instance).AddScore(p, score);

        Instantiate(particlesGameobject, transform.position, Quaternion.identity);

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(gameObject);
    }
}
