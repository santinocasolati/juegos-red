using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowWater : MonoBehaviour
{
    public Transform water;
    public float offsetY;
    public float smoothSpeed = .5f;

    private Vector3 targetPos;

    void LateUpdate()
    {
        if (water == null) return;

        targetPos = new Vector3(transform.position.x, water.position.y + offsetY, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
    }
}
