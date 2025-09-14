using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro를 사용하려면 필요
using UnityEngine.SceneManagement; // 씬을 로드하기 위해 필요

public class HealthManager : MonoBehaviour
{
    public UnityEngine.UI.Image[] healthUnits; // 체력 이미지 배열
    public TextMeshProUGUI timeOverText; // "GAME OVER" 메시지 TextMeshProUGUI
    public Button resetButton; // 게임 재시작 버튼
    public Button quitButton;  // 게임 종료 버튼
    public AudioClip gameOverSound; // 게임 오버 사운드
    private AudioSource audioSource; // 오디오 소스

    private int currentHealth;

    void Start()
    {
        currentHealth = healthUnits.Length; // 체력 초기화 (예: 5개)
        audioSource = GetComponent<AudioSource>(); // 오디오 소스 초기화

        // Time Over 메시지 초기화 (기본적으로 숨김)
        if (timeOverText != null)
        {
            timeOverText.gameObject.SetActive(false); // 게임 오버 메시지 숨김
        }

        // 게임 오버 시 버튼들 숨기기
        if (resetButton != null && quitButton != null)
        {
            resetButton.gameObject.SetActive(false); // Reset 버튼 숨기기
            quitButton.gameObject.SetActive(false);  // Quit 버튼 숨기기
        }

        // Reset 버튼 클릭 이벤트 코드에서 연결 (필요한 경우)
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetGame);
        }
    }

    public void DecreaseHealth()
    {
        if (currentHealth > 0)
        {
            currentHealth--; // 체력 감소
            healthUnits[currentHealth].enabled = false; // 체력 표시 이미지 비활성화

            if (currentHealth == 0)
            {
                EndGame(); // 체력이 0이면 게임 종료 처리
            }
        }
    }

    public void IncreaseHealth()
    {
        if (currentHealth < healthUnits.Length)
        {
            healthUnits[currentHealth].enabled = true; // 체력 표시 이미지 활성화
            currentHealth++; // 체력 증가
        }
    }

    void EndGame()
    {
        Debug.Log("Health is zero! Game Over");

        PlayGameOverSound(); // 게임 오버 사운드 재생
        ShowTimeOverMessage(); // "GAME OVER" 메시지 표시
        ShowButtons(); // Reset 및 Quit 버튼 표시
        Time.timeScale = 0; // 게임 일시 정지
    }

    void PlayGameOverSound()
    {
        if (audioSource != null && gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound); // 게임 오버 사운드 재생
        }
        else
        {
            Debug.LogWarning("AudioSource or GameOverSound is not set.");
        }
    }

    void ShowTimeOverMessage()
    {
        if (timeOverText != null)
        {
            timeOverText.gameObject.SetActive(true); // "GAME OVER" 메시지 활성화
            timeOverText.text = "GAME OVER"; // 텍스트 설정
        }
    }

    void ShowButtons()
    {
        // Reset 버튼과 Quit 버튼을 활성화
        if (resetButton != null && quitButton != null)
        {
            resetButton.gameObject.SetActive(true); // Reset 버튼 활성화
            quitButton.gameObject.SetActive(true);  // Quit 버튼 활성화
        }
    }

    // 게임 재시작 함수
    void ResetGame()
    {
        // 현재 씬을 다시 로드하여 게임을 재시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1; // 게임을 정상 진행 상태로 복원
    }

    // Quit 버튼을 눌렀을 때 씬 이동 함수 (Inspector에서 설정 가능하도록 public으로 변경)
    public void QuitGame(string sceneName)
    {
        Debug.Log($"Going to {sceneName}...");
        SceneManager.LoadScene(sceneName); // Inspector에서 전달받은 씬 이름으로 이동
    }
}
