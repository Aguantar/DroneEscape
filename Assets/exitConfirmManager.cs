using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ExitConfirmationManager : MonoBehaviour
{
    // Ȯ�� �г� UI ������Ʈ
    public GameObject confirmationPanel;

    // ��ư��: ���� ���������� ���� ��ư�� ���� �޴��� ���� ��ư
    public Button continueButton;
    public Button outButton;

    // ���� ���� �������� ���� ������ �� �̸�
    public string nextSceneName = "m1";   // ���� �������� �� �̸�
    public string stagePageSceneName = "stagepage"; // �������� ���� ������ �� �̸�

    // �г� Ȱ��ȭ �� ����� ����� ���� ����
    public AudioClip panelOpenSound;        // Ȱ��ȭ ���� Ŭ��
    private AudioSource audioSource;        // ����� �ҽ� ������Ʈ

    // Trigger �ߺ� ���� ������ ���� �÷���
    private bool hasTriggered = false;

    void Start()
    {
        // AudioSource �ʱ�ȭ �� ���� üũ
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on this object.");
        }

        // ��ư Ŭ�� �̺�Ʈ�� �Լ� ����
        if (continueButton != null)
        {
            Time.timeScale = 1f; // ���� �ð� �ʱ�ȭ
            continueButton.onClick.AddListener(LoadNextStage); // ���� ���������� �̵�
        }

        if (outButton != null)
        {
            outButton.onClick.AddListener(LoadStageSelection); // ���� �޴��� �̵�
        }

        // ���� �� Ȯ�� �гΰ� ��ư���� ��Ȱ��ȭ
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(false);
        }
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(false);
        }
        if (outButton != null)
        {
            outButton.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Ȯ�� �г��� Ȱ��ȭ�� ���¿��� Ű���� �Է� ó��
        if (confirmationPanel != null && confirmationPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Y)) // Y Ű �Է� ��
            {
                Debug.Log("Y Ű �Է�: ���� ���������� �̵�");
                LoadNextStage(); // ���� �������� �ε� �Լ� ȣ��
            }
            if (Input.GetKeyDown(KeyCode.N)) // N Ű �Է� ��
            {
                Debug.Log("N Ű �Է�: ���� �޴��� �̵�");
                LoadStageSelection(); // ���� �޴� �ε� �Լ� ȣ��
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾ Ʈ���ſ� �����ϰ� �ߺ� ������ �ƴ� ���
        if (other.CompareTag("Player") && !hasTriggered)
        {
            Debug.Log("Player entered the trigger!");
            hasTriggered = true; // �ߺ� ���� ������ ���� �÷��� ����
            StartCoroutine(ActivatePanelWithDelay()); // ������ �� �г� Ȱ��ȭ
        }
    }

    private IEnumerator ActivatePanelWithDelay()
    {
        yield return new WaitForSeconds(0.1f); // ª�� ������ ����
        ShowConfirmationPanel(); // Ȯ�� �г� Ȱ��ȭ
    }

    // Ȯ�� �г��� ǥ���ϰ� ���� UI�� Ȱ��ȭ�ϴ� �Լ�
    public void ShowConfirmationPanel()
    {
        if (confirmationPanel != null && !confirmationPanel.activeSelf)
        {
            confirmationPanel.SetActive(true); // �г� Ȱ��ȭ
            Debug.Log("Confirmation Panel Activated.");

            if (continueButton != null)
            {
                continueButton.gameObject.SetActive(true); // ��� ��ư Ȱ��ȭ
            }
            if (outButton != null)
            {
                outButton.gameObject.SetActive(true); // ������ ��ư Ȱ��ȭ
            }

            PlayPanelSound(); // �г� Ȱ��ȭ ���� ���

            Time.timeScale = 0f; // ���� �Ͻ� ����
        }
    }

    // �г� Ȱ��ȭ �� ���带 ����ϴ� �Լ�
    private void PlayPanelSound()
    {
        if (audioSource != null && panelOpenSound != null)
        {
            audioSource.PlayOneShot(panelOpenSound); // ���� Ŭ�� ���
        }
        else
        {
            Debug.LogWarning("AudioSource or PanelOpenSound is not set.");
        }
    }

    // 'Continue' ��ư Ŭ�� �� ȣ��Ǿ� ���� ���������� �̵��ϴ� �Լ�
    public void LoadNextStage()
    {
        Debug.Log($"Loading next scene: {nextSceneName}");
        SceneManager.LoadScene(nextSceneName); // ���� �������� �� �ε�
    }

    // 'Out' ��ư Ŭ�� �� ȣ��Ǿ� �������� ���� �������� �̵��ϴ� �Լ�
    public void LoadStageSelection()
    {
        Debug.Log($"Loading stage selection scene: {stagePageSceneName}");
        SceneManager.LoadScene(stagePageSceneName); // �������� ���� �� �ε�
    }

    // �г��� �ݰ� Trigger �÷��׸� �ʱ�ȭ�ϴ� �Լ�
    public void ClosePanel()
    {
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(false); // �г� ��Ȱ��ȭ
        }
        hasTriggered = false; // Trigger �ʱ�ȭ
    }
}
