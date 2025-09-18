using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelSegmentController : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private TMP_Text label;
    [SerializeField] private bool usesAnimation;

    private void OnEnable()
    {
        if (!usesAnimation) return;

        transform.localScale = new Vector3(.5f, .5f, .5f);

        StartCoroutine(OpenAnimation());
    }

    public void SetData(Sprite icon, string text)
    {
        img.sprite = icon;
        label.text = text;
    }

    private IEnumerator OpenAnimation()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.one;

        float elapsed = 0f;

        while (elapsed < .5f)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / .5f);

            float easedT = 1f - Mathf.Pow(1f - t, 3);

            transform.localScale = Vector3.Lerp(startScale, endScale, easedT);

            yield return null;
        }

        transform.localScale = endScale;
    }
}
