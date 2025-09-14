using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // "New Game" 버튼 클릭 시 호출되는 함수
    public void OnNewGameButtonClicked()
    {
        // "새 게임 시작" 로그를 출력하고 첫 번째 스테이지 씬(stage1open)으로 이동
        Debug.Log("새 게임 시작");
        SceneManager.LoadScene("stage1open");
    }

    // "Tutorial" 버튼 클릭 시 호출되는 함수
    public void OnTutorialButtonClicked()
    {
        // "튜토리얼" 로그를 출력하고 튜토리얼 씬(m00)으로 이동
        Debug.Log("튜토리얼");
        SceneManager.LoadScene("m00");
    }

    // "Explain" 버튼 클릭 시 호출되는 함수
    public void OnExplainButtonClicked()
    {
        // 첫 번째 설명 페이지 씬(explain_1)으로 이동
        SceneManager.LoadScene("explain_1");
    }

    // "Next Page" 버튼 클릭 시 호출되는 함수
    public void OnExplainNextPageButtonClicked()
    {
        // 다음 설명 페이지 씬(explainScenes)으로 이동
        SceneManager.LoadScene("explainScenes");
    }

    // "Back to Main Page" 버튼 클릭 시 호출되는 함수
    public void OnExplainBackPageButtonClicked()
    {
        // 메인 메뉴 씬(mainpage)으로 이동
        SceneManager.LoadScene("mainpage");
    }

    // "Back to First Page" 버튼 클릭 시 호출되는 함수
    public void OnExplainBack2PageButtonClicked()
    {
        // 첫 번째 설명 페이지 씬(explain_1)으로 이동
        SceneManager.LoadScene("explain_1");
    }

    // "Back to Main Menu" 버튼 클릭 시 호출되는 함수
    public void OnBackToMenuButtonClicked()
    {
        // 메인 메뉴 씬(mainpage)으로 이동
        SceneManager.LoadScene("mainpage");
    }

    // "Quit" 버튼 클릭 시 호출되는 함수
    public void OnQuitButtonClicked()
    {
        // 에디터에서 실행 중이면 플레이 중지, 그렇지 않으면 게임 종료
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // "Stage 1" 버튼 클릭 시 호출되는 함수
    public void OnStage1ButtonClicked()
    {
        // "stage1" 로그를 출력하고 첫 번째 스테이지 씬(m1)으로 이동
        Debug.Log("stage1");
        SceneManager.LoadScene("m1");
    }

    // "Stage 2" 버튼 클릭 시 호출되는 함수
    public void OnStage2ButtonClicked()
    {
        // "stage2" 로그를 출력하고 두 번째 스테이지 씬(m2)으로 이동
        Debug.Log("stage2");
        SceneManager.LoadScene("m2");
    }

    // "Stage 3" 버튼 클릭 시 호출되는 함수
    public void OnStage3ButtonClicked()
    {
        // "stage3" 로그를 출력하고 세 번째 스테이지 씬(m3)으로 이동
        Debug.Log("stage3");
        SceneManager.LoadScene("m3");
    }

    // "Stage 4" 버튼 클릭 시 호출되는 함수
    public void OnStage4ButtonClicked()
    {
        // "stage4" 로그를 출력하고 네 번째 스테이지 씬(m4)으로 이동
        Debug.Log("stage4");
        SceneManager.LoadScene("m4");
    }
}
