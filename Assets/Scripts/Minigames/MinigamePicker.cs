using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MinigamePicker : MonoBehaviourPun
{
    [SerializeField] private MinigameCatalogSO minigameCatalog;
    [SerializeField] private float spinDuration = 6f;
    [SerializeField] private int extraSpins = 10;
    [SerializeField] private RectTransform wheel;
    [SerializeField] public GameObject segmentPrefab;
    [SerializeField] public WheelSegmentController selectedNotification;

    private bool spinning = false;

    private void Awake()
    {
        GenerateWheel();
    }

    public void GenerateWheel()
    {
        foreach (Transform child in wheel)
            Destroy(child.gameObject);

        int count = minigameCatalog.minigames.Count;
        float fillAmount = 1f / count;
        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            MinigameSO minigame = minigameCatalog.minigames[i];

            GameObject segment = Instantiate(segmentPrefab, wheel);
            segment.name = $"Segment_{i}_{minigame.displayName}";

            Image image = segment.GetComponent<Image>();
            if (image != null)
            {
                image.type = Image.Type.Filled;
                image.fillMethod = Image.FillMethod.Radial360;
                image.fillOrigin = 0;
                image.fillAmount = fillAmount;
            }

            segment.transform.localRotation = Quaternion.Euler(0, 0, -i * angleStep);
            segment.transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 45 * (count - 1));
            segment.GetComponent<WheelSegmentController>().SetData(minigame.icon);
        }
    }

    public void PickMinigame()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        int randomIndex = Random.Range(0, minigameCatalog.minigames.Count);
        photonView.RPC("RPC_SpinToValue", RpcTarget.All, randomIndex);
    }

    [PunRPC]
    public void RPC_SpinToValue(int targetIndex)
    {
        if (spinning) return;
        spinning = true;

        if (targetIndex < 0) targetIndex = 0;

        StartCoroutine(SpinWheel(targetIndex, spinDuration));
    }

    private IEnumerator SpinWheel(int targetIndex, float duration)
    {
        yield return new WaitForSeconds(1);

        int segmentCount = minigameCatalog.minigames.Count;
        float anglePerSegment = 360f / segmentCount;
        float startRotation = wheel.eulerAngles.z;

        float segmentCenterOffset = anglePerSegment / 2f;

        float endRotation = startRotation + (360f * extraSpins) + (targetIndex * anglePerSegment) - segmentCenterOffset - 90f;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float easedT = 1f - Mathf.Pow(1f - t, 3);
            float z = Mathf.Lerp(startRotation, endRotation, easedT);
            wheel.eulerAngles = new Vector3(0, 0, z);
            yield return null;
        }

        wheel.eulerAngles = new Vector3(0, 0, endRotation);
        spinning = false;

        MinigameSO selectedMinigame = minigameCatalog.minigames[targetIndex];
        selectedNotification.SetData(selectedMinigame.icon);
        selectedNotification.transform.parent.gameObject.SetActive(true);

        yield return new WaitForSeconds(1);

        PhotonNetwork.LoadLevel(selectedMinigame.scene);
    }

}
