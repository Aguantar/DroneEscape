using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    public GameObject clearMenuUI; // 클리어 메뉴 UI

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 드론이 목표 지점에 도달했는지 확인
        {
            Debug.Log("Player has reached the goal");
            ShowClearMenu(); // 클리어 메뉴 표시
        }
    }

    private void ShowClearMenu()
    {
        clearMenuUI.SetActive(true); // 클리어 메뉴 활성화
        
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // 게임 시간 정상화
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬 다시 로드
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit(); // 게임 종료
    }
}
