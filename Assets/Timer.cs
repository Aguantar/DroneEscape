using UnityEngine;
using TMPro;
using UnityEngine.UI; // Button을 사용하기 위해 추가
using UnityEngine.SceneManagement; // 씬을 로드하기 위해 필요

public class Timer : MonoBehaviour
{
    public float timeLimit = 300f; // 제한 시간 (초 단위)
    private float currentTime;

    public TextMeshProUGUI timerText; // 시간 표시 TextMeshProUGUI
    public TextMeshProUGUI timeOverText; // "Time Over" 메시지용 TextMeshProUGUI

    public AudioClip timeOverSound; // "Time Over" 사운드
    private AudioSource audioSource; // 오디오 소스

    private bool isGameOver = false; // 게임 종료 상태 플래그

    // 버튼들 추가
    public Button retryButton;  // 게임 재시작 버튼
    public Button quitButton;   // 게임 종료 버튼

    void Start()
    {
        currentTime = timeLimit; // 시작 시 현재 시간을 제한 시간으로 초기화
        UpdateTimerDisplay();

        // Time Over 텍스트 초기화
        if (timeOverText != null)
        {
            timeOverText.gameObject.SetActive(false); // 처음에는 숨김 처리
        }

        // 버튼 초기화 (처음에는 숨김)
        if (retryButton != null && quitButton != null)
        {
            retryButton.gameObject.SetActive(false);  // 재시작 버튼 숨기기
            quitButton.gameObject.SetActive(false);   // 종료 버튼 숨기기
        }

        // AudioSource 초기화
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on this object.");
        }
    }

    void Update()
    {
        if (isGameOver) return; // 게임이 이미 종료된 경우 로직 실행 방지

        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime; // 매 프레임마다 시간 감소
            if (currentTime < 0) currentTime = 0; // 0 이하로 내려가는 것을 방지
            UpdateTimerDisplay();
        }

        if (Mathf.Approximately(currentTime, 0) && !isGameOver) // 0에 근접한 값 처리
        {
            EndGame();
        }
    }

    void UpdateTimerDisplay()
    {
        // 초 단위로 시간을 표시
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // MM:SS 형식으로 시간 표시
    }

    void EndGame()
    {
        if (isGameOver) return; // 게임이 이미 종료된 경우 실행 방지

        isGameOver = true; // 게임 종료 상태 설정
        Debug.Log("Time's up! Game Over");
        PlayTimeOverSound(); // "Time Over" 사운드 재생
        ShowTimeOverMessage();
        ShowButtons();  // 버튼들 활성화
        Time.timeScale = 0; // 게임 멈춤
    }

    void ShowTimeOverMessage()
    {
        if (timeOverText != null)
        {
            timeOverText.gameObject.SetActive(true); // "Time Over" 메시지 표시
            timeOverText.text = "Time Over"; // 메시지 설정
        }
    }

    void PlayTimeOverSound()
    {
        if (audioSource != null && timeOverSound != null)
        {
            audioSource.PlayOneShot(timeOverSound); // "Time Over" 사운드 재생
        }
        else
        {
            Debug.LogWarning("AudioSource or TimeOverSound is not set.");
        }
    }

    // 게임 종료 후 버튼을 보이게 하는 함수
    void ShowButtons()
    {
        if (retryButton != null && quitButton != null)
        {
            retryButton.gameObject.SetActive(true);  // 재시작 버튼 활성화
            quitButton.gameObject.SetActive(true);   // 종료 버튼 활성화
        }
    }

    // 게임 재시작 함수
    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬 재로딩
        Time.timeScale = 1; // 게임 진행 상태로 복원
    }

    // Quit 버튼을 눌렀을 때 게임 종료 함수
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit(); // 게임 종료
    }
}
