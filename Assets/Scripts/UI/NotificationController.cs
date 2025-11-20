using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationController : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("notification"))
        {
            text.text = PlayerPrefs.GetString("notification");
        }
        else
        {
            Close();
        }
    }

    public void Close()
    {
        PlayerPrefs.DeleteKey("notification");
        gameObject.SetActive(false);
    }
}
