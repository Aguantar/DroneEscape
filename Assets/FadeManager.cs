using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public Image blackoutPanel; // Panel�� Image ������Ʈ
    public float fadeDuration = 0.5f; // ���̵� ���� �ð�
    public float blackoutDuration = 3.0f; // ���� ���� �ð�
    public float intervalDuration = 20.0f; // ���� �ֱ� (20��)

    private bool isFading = false; // ���̵� �� ����

    void Start()
    {
        if (blackoutPanel == null)
        {
            Debug.LogError("Blackout Panel is not assigned in the Inspector!");
        }
        else
        {
            StartCoroutine(RepeatBlackout()); // ���� �ݺ� ����
        }
    }

    /// <summary>
    /// ȭ���� ��Ӱ� ����ϴ�.
    /// </summary>
    public void FadeToBlack()
    {
        if (!isFading)
        {
            StartCoroutine(Fade(0f, 1f)); // ������ ����������
        }
    }

    /// <summary>
    /// ȭ���� ��� ����ϴ�.
    /// </summary>
    public void FadeToClear()
    {
        if (!isFading)
        {
            StartCoroutine(Fade(1f, 0f)); // �������� ��������
        }
    }

    /// <summary>
    /// ���� ȿ���� �ݺ������� �����մϴ�.
    /// </summary>
    private IEnumerator RepeatBlackout()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalDuration - fadeDuration); // �ֱ��� ��� (20�� - ���̵� �ð�)

            FadeToBlack(); // ȭ�� ����
            yield return new WaitForSeconds(blackoutDuration); // ���� ���� ���� (2��)

            FadeToClear(); // ȭ�� ��� ����
            yield return new WaitForSeconds(fadeDuration); // ���̵� �ƿ� �ð� ���
        }
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        isFading = true;
        float elapsedTime = 0f;

        Color panelColor = blackoutPanel.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            blackoutPanel.color = new Color(panelColor.r, panelColor.g, panelColor.b, alpha);
            yield return null;
        }

        blackoutPanel.color = new Color(panelColor.r, panelColor.g, panelColor.b, endAlpha);
        isFading = false;
    }
}
