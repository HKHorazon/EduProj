using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RandomImageColor : MonoBehaviour
{
    [SerializeField] private Image targetImage;
    [SerializeField] private float changeInterval = 1f; // 變色間隔秒數

    private Coroutine colorChangeCoroutine;

    public void SetRandomColor()
    {
        if (targetImage == null)
        {
            Debug.LogWarning("targetImage 尚未指定！");
            return;
        }
        Color randomColor = new Color(
            UnityEngine.Random.value,
            UnityEngine.Random.value,
            UnityEngine.Random.value,
            1f // 不改變透明度
        );
        targetImage.color = randomColor;
    }

    public void StartAutoColorChange()
    {
        if (colorChangeCoroutine == null)
        {
            colorChangeCoroutine = StartCoroutine(AutoChangeColor());
        }
    }

    public void StopAutoColorChange()
    {
        if (colorChangeCoroutine != null)
        {
            StopCoroutine(colorChangeCoroutine);
            colorChangeCoroutine = null;
        }
    }

    private IEnumerator AutoChangeColor()
    {
        while (true)
        {
            SetRandomColor();
            yield return new WaitForSeconds(changeInterval);
        }
    }

    private void OnEnable()
    {
        StartAutoColorChange();
    }

    private void OnDisable()
    {
        StopAutoColorChange();
    }
}
