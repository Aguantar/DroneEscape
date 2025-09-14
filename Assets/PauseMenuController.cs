using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    // Pause Menu UI 오브젝트를 참조
    public GameObject pauseMenuUI;

    // 게임이 일시정지 상태인지 여부를 나타내는 플래그
    private bool isPaused = false;

    void Update()
    {
        // ESC 키 입력으로 Pause 상태를 전환
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame(); // 게임 재개
            }
            else
            {
                PauseGame(); // 게임 일시정지
            }
        }
    }

    // 게임을 재개하는 함수
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Pause Menu를 비활성화
        Time.timeScale = 1f; // 게임 시간을 정상 속도로 설정
        isPaused = false; // 일시정지 상태 해제
    }

    // 게임을 일시정지하는 함수
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true); // Pause Menu를 활성화
        Time.timeScale = 0f; // 게임 시간을 멈춤
        isPaused = true; // 일시정지 상태 활성화
    }

    // 현재 게임을 다시 시작하는 함수
    public void RestartGame()
    {
        Time.timeScale = 1f; // 게임 시간을 정상 속도로 설정
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 활성화된 씬을 다시 로드
    }

    // 메인 페이지로 이동하는 함수
    public void QuitGame0()
    {
        SceneManager.LoadScene("mainpage"); // 메인 페이지 씬 로드
    }

    // 스테이지 1 오프닝 씬으로 이동하는 함수
    public void QuitGame()
    {
        SceneManager.LoadScene("stage1open"); // 스테이지 1 오프닝 씬 로드
    }

    // 스테이지 2 오프닝 씬으로 이동하는 함수
    public void QuitGame1()
    {
        SceneManager.LoadScene("stage2open"); // 스테이지 2 오프닝 씬 로드
    }

    // 스테이지 3 오프닝 씬으로 이동하는 함수
    public void QuitGame2()
    {
        SceneManager.LoadScene("stage3open"); // 스테이지 3 오프닝 씬 로드
    }

    // 스테이지 4 오프닝 씬으로 이동하는 함수
    public void QuitGame3()
    {
        SceneManager.LoadScene("stage4open"); // 스테이지 4 오프닝 씬 로드
    }
}
