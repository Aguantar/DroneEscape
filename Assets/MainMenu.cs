using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // "New Game" ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void OnNewGameButtonClicked()
    {
        // "�� ���� ����" �α׸� ����ϰ� ù ��° �������� ��(stage1open)���� �̵�
        Debug.Log("�� ���� ����");
        SceneManager.LoadScene("stage1open");
    }

    // "Tutorial" ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void OnTutorialButtonClicked()
    {
        // "Ʃ�丮��" �α׸� ����ϰ� Ʃ�丮�� ��(m00)���� �̵�
        Debug.Log("Ʃ�丮��");
        SceneManager.LoadScene("m00");
    }

    // "Explain" ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void OnExplainButtonClicked()
    {
        // ù ��° ���� ������ ��(explain_1)���� �̵�
        SceneManager.LoadScene("explain_1");
    }

    // "Next Page" ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void OnExplainNextPageButtonClicked()
    {
        // ���� ���� ������ ��(explainScenes)���� �̵�
        SceneManager.LoadScene("explainScenes");
    }

    // "Back to Main Page" ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void OnExplainBackPageButtonClicked()
    {
        // ���� �޴� ��(mainpage)���� �̵�
        SceneManager.LoadScene("mainpage");
    }

    // "Back to First Page" ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void OnExplainBack2PageButtonClicked()
    {
        // ù ��° ���� ������ ��(explain_1)���� �̵�
        SceneManager.LoadScene("explain_1");
    }

    // "Back to Main Menu" ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void OnBackToMenuButtonClicked()
    {
        // ���� �޴� ��(mainpage)���� �̵�
        SceneManager.LoadScene("mainpage");
    }

    // "Quit" ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void OnQuitButtonClicked()
    {
        // �����Ϳ��� ���� ���̸� �÷��� ����, �׷��� ������ ���� ����
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // "Stage 1" ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void OnStage1ButtonClicked()
    {
        // "stage1" �α׸� ����ϰ� ù ��° �������� ��(m1)���� �̵�
        Debug.Log("stage1");
        SceneManager.LoadScene("m1");
    }

    // "Stage 2" ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void OnStage2ButtonClicked()
    {
        // "stage2" �α׸� ����ϰ� �� ��° �������� ��(m2)���� �̵�
        Debug.Log("stage2");
        SceneManager.LoadScene("m2");
    }

    // "Stage 3" ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void OnStage3ButtonClicked()
    {
        // "stage3" �α׸� ����ϰ� �� ��° �������� ��(m3)���� �̵�
        Debug.Log("stage3");
        SceneManager.LoadScene("m3");
    }

    // "Stage 4" ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void OnStage4ButtonClicked()
    {
        // "stage4" �α׸� ����ϰ� �� ��° �������� ��(m4)���� �̵�
        Debug.Log("stage4");
        SceneManager.LoadScene("m4");
    }
}
