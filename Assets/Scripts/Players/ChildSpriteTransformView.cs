using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildSpriteTransformView : MonoBehaviour, IPunObservable
{
    [Header("Sync Options")]
    public bool syncPosition;
    public bool syncRotation;
    public bool syncScale;

    [Header("References")]
    public Transform childSprite;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private Vector3 targetScale;

    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();

        if (childSprite == null)
            childSprite = transform;
    }

    private void Update()
    {
        if (!pv.IsMine)
        {
            if (syncPosition)
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);

            if (syncRotation)
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

            if (syncScale)
                childSprite.localScale = targetScale;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (syncPosition)
                stream.SendNext(transform.position);

            if (syncRotation)
                stream.SendNext(transform.rotation);

            if (syncScale)
                stream.SendNext(childSprite.localScale);
        }
        else
        {
            if (syncPosition)
                targetPosition = (Vector3)stream.ReceiveNext();

            if (syncRotation)
                targetRotation = (Quaternion)stream.ReceiveNext();

            if (syncScale)
                targetScale = (Vector3)stream.ReceiveNext();
        }
    }
}
