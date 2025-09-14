using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public Image blackoutPanel; // Panel의 Image 컴포넌트
    public float fadeDuration = 0.5f; // 페이드 지속 시간
    public float blackoutDuration = 3.0f; // 암전 지속 시간
    public float intervalDuration = 20.0f; // 암전 주기 (20초)

    private bool isFading = false; // 페이드 중 여부

    void Start()
    {
        if (blackoutPanel == null)
        {
            Debug.LogError("Blackout Panel is not assigned in the Inspector!");
        }
        else
        {
            StartCoroutine(RepeatBlackout()); // 암전 반복 시작
        }
    }

    /// <summary>
    /// 화면을 어둡게 만듭니다.
    /// </summary>
    public void FadeToBlack()
    {
        if (!isFading)
        {
            StartCoroutine(Fade(0f, 1f)); // 투명에서 불투명으로
        }
    }

    /// <summary>
    /// 화면을 밝게 만듭니다.
    /// </summary>
    public void FadeToClear()
    {
        if (!isFading)
        {
            StartCoroutine(Fade(1f, 0f)); // 불투명에서 투명으로
        }
    }

    /// <summary>
    /// 암전 효과를 반복적으로 실행합니다.
    /// </summary>
    private IEnumerator RepeatBlackout()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalDuration - fadeDuration); // 주기적 대기 (20초 - 페이드 시간)

            FadeToBlack(); // 화면 암전
            yield return new WaitForSeconds(blackoutDuration); // 암전 상태 유지 (2초)

            FadeToClear(); // 화면 밝기 복원
            yield return new WaitForSeconds(fadeDuration); // 페이드 아웃 시간 대기
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
