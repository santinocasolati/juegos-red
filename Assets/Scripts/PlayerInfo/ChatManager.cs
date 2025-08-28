using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField chatInput;
    [SerializeField] private Transform chatContentParent;
    [SerializeField] private GameObject chatMessagePrefab;
    [SerializeField] private ScrollRect scrollRect;

    public void SendMessage()
    {
        if (string.IsNullOrEmpty(chatInput.text)) return;

        photonView.RPC("ReceiveMessage", RpcTarget.All, PhotonNetwork.NickName, chatInput.text, PhotonNetwork.LocalPlayer.ActorNumber);
        chatInput.text = "";
    }

    [PunRPC]
    public void ReceiveMessage(string sender, string message, int senderId)
    {
        GameObject msgObj = Instantiate(chatMessagePrefab, chatContentParent);
        ChatMessageUI msgUI = msgObj.GetComponent<ChatMessageUI>();

        bool isLocal = senderId == PhotonNetwork.LocalPlayer.ActorNumber;
        msgUI.SetMessage(sender, message, isLocal);

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        foreach (Transform child in chatContentParent)
            Destroy(child.gameObject);

        chatInput.text = "";
    }
}
