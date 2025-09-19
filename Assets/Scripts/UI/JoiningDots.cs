using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class JoiningDots : MonoBehaviour
{
    [SerializeField] private TMP_Text joiningText;
    [SerializeField] private float interval = 0.25f;

    private string baseText = "Connecting";
    private Coroutine loopCoroutine;

    private void OnEnable()
    {
        loopCoroutine = StartCoroutine(LoopDots());
    }

    private void OnDisable()
    {
        if (loopCoroutine != null)
            StopCoroutine(loopCoroutine);
    }

    private IEnumerator LoopDots()
    {
        int dotCount = 0;

        while (true)
        {
            joiningText.text = baseText + new string('.', dotCount);

            dotCount = (dotCount + 1) % 4;

            yield return new WaitForSeconds(interval);
        }
    }
}
