using TMPro;
using UnityEngine;

public class ChatMessageUI : MonoBehaviour
{
    [SerializeField] private TMP_Text senderText;
    [SerializeField] private TMP_Text messageText;

    public void SetMessage(string sender, string message, bool isLocalPlayer)
    {
        senderText.text = sender;
        messageText.text = message;

        senderText.color = isLocalPlayer ? Color.green : Color.red;
    }
}
