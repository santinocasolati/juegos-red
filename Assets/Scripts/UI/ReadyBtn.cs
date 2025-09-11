using Photon.Pun;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReadyBtn : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text text;

    private Button button;

    private bool isReady = false;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ToggleReady);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        isReady = false;
        text.text = "READY?";

        Hashtable props = new Hashtable
            {
                { "Ready", isReady }
            };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public void ToggleReady()
    {
        isReady = !isReady;

        Hashtable props = new Hashtable
            {
                { "Ready", isReady }
            };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        text.text = isReady ? "CANCEL" : "READY?";
    }
}
