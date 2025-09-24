using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRise : MonoBehaviourPun
{
    public float speed = 1f;
    public float acceleration = 0.025f;

    private bool started = false;

    public void StartRising()
    {
        started = true;
    }

    public void StopRising()
    {
        started = false;
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (!started) return;

        transform.Translate(Vector2.up * speed * Time.deltaTime);
        speed += acceleration * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!started) return;

        if (collision.gameObject.TryGetComponent(out PlayersHealth health))
        {
            health.Damage(1);
        }
    }
}
