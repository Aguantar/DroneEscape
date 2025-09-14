using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ExitConfirmationManager : MonoBehaviour
{
    // 확인 패널 UI 오브젝트
    public GameObject confirmationPanel;

    // 버튼들: 다음 스테이지로 가는 버튼과 메인 메뉴로 가는 버튼
    public Button continueButton;
    public Button outButton;

    // 다음 씬과 스테이지 선택 페이지 씬 이름
    public string nextSceneName = "m1";   // 다음 스테이지 씬 이름
    public string stagePageSceneName = "stagepage"; // 스테이지 선택 페이지 씬 이름

    // 패널 활성화 시 재생할 오디오 관련 변수
    public AudioClip panelOpenSound;        // 활성화 사운드 클립
    private AudioSource audioSource;        // 오디오 소스 컴포넌트

    // Trigger 중복 실행 방지를 위한 플래그
    private bool hasTriggered = false;

    void Start()
    {
        // AudioSource 초기화 및 오류 체크
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on this object.");
        }

        // 버튼 클릭 이벤트와 함수 연결
        if (continueButton != null)
        {
            Time.timeScale = 1f; // 게임 시간 초기화
            continueButton.onClick.AddListener(LoadNextStage); // 다음 스테이지로 이동
        }

        if (outButton != null)
        {
            outButton.onClick.AddListener(LoadStageSelection); // 메인 메뉴로 이동
        }

        // 시작 시 확인 패널과 버튼들을 비활성화
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
        // 확인 패널이 활성화된 상태에서 키보드 입력 처리
        if (confirmationPanel != null && confirmationPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Y)) // Y 키 입력 시
            {
                Debug.Log("Y 키 입력: 다음 스테이지로 이동");
                LoadNextStage(); // 다음 스테이지 로드 함수 호출
            }
            if (Input.GetKeyDown(KeyCode.N)) // N 키 입력 시
            {
                Debug.Log("N 키 입력: 메인 메뉴로 이동");
                LoadStageSelection(); // 메인 메뉴 로드 함수 호출
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 트리거에 진입하고 중복 실행이 아닌 경우
        if (other.CompareTag("Player") && !hasTriggered)
        {
            Debug.Log("Player entered the trigger!");
            hasTriggered = true; // 중복 실행 방지를 위해 플래그 설정
            StartCoroutine(ActivatePanelWithDelay()); // 딜레이 후 패널 활성화
        }
    }

    private IEnumerator ActivatePanelWithDelay()
    {
        yield return new WaitForSeconds(0.1f); // 짧은 딜레이 적용
        ShowConfirmationPanel(); // 확인 패널 활성화
    }

    // 확인 패널을 표시하고 관련 UI를 활성화하는 함수
    public void ShowConfirmationPanel()
    {
        if (confirmationPanel != null && !confirmationPanel.activeSelf)
        {
            confirmationPanel.SetActive(true); // 패널 활성화
            Debug.Log("Confirmation Panel Activated.");

            if (continueButton != null)
            {
                continueButton.gameObject.SetActive(true); // 계속 버튼 활성화
            }
            if (outButton != null)
            {
                outButton.gameObject.SetActive(true); // 나가기 버튼 활성화
            }

            PlayPanelSound(); // 패널 활성화 사운드 재생

            Time.timeScale = 0f; // 게임 일시 정지
        }
    }

    // 패널 활성화 시 사운드를 재생하는 함수
    private void PlayPanelSound()
    {
        if (audioSource != null && panelOpenSound != null)
        {
            audioSource.PlayOneShot(panelOpenSound); // 사운드 클립 재생
        }
        else
        {
            Debug.LogWarning("AudioSource or PanelOpenSound is not set.");
        }
    }

    // 'Continue' 버튼 클릭 시 호출되어 다음 스테이지로 이동하는 함수
    public void LoadNextStage()
    {
        Debug.Log($"Loading next scene: {nextSceneName}");
        SceneManager.LoadScene(nextSceneName); // 다음 스테이지 씬 로드
    }

    // 'Out' 버튼 클릭 시 호출되어 스테이지 선택 페이지로 이동하는 함수
    public void LoadStageSelection()
    {
        Debug.Log($"Loading stage selection scene: {stagePageSceneName}");
        SceneManager.LoadScene(stagePageSceneName); // 스테이지 선택 씬 로드
    }

    // 패널을 닫고 Trigger 플래그를 초기화하는 함수
    public void ClosePanel()
    {
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(false); // 패널 비활성화
        }
        hasTriggered = false; // Trigger 초기화
    }
}
