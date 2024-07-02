using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ButtonController : MonoBehaviour
{
    public Button pauseButton;
    public Button playButton;
    public GameManager gameManager; // GameManager 스크립트를 참조
    public TextMeshProUGUI countdownText; // TMP 텍스트를 참조
    public PlayerController playerController; // PlayerController 스크립트를 참조

    void Start()
    {
        // 초기 설정: Pause 버튼 활성화, Play 버튼 비활성화
        pauseButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false); // Countdown 텍스트 비활성화

        // 버튼 클릭 이벤트 리스너 추가
        pauseButton.onClick.AddListener(PauseGame);
        playButton.onClick.AddListener(StartCountdown);
    }

    void PauseGame()
    {
        // 게임을 일시 정지하고 버튼 상태 변경
        gameManager.Pause();
        pauseButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
        playerController.StopMoving();
    }

    public void StartCountdown()
    {
        // 카운트다운 동안 일시 정지 상태로 유지
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        // 카운트다운 동안 게임 일시 정지
        gameManager.Pause();

        // Countdown 텍스트 활성화
        countdownText.gameObject.SetActive(true);
        countdownText.text = "Get Ready!";

        // 1초 대기
        yield return new WaitForSecondsRealtime(1f);

        // 3, 2, 1 카운트다운
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }

        // 카운트다운 완료 후 텍스트 비활성화
        countdownText.gameObject.SetActive(false);

        // 게임 재개
        gameManager.Resume();
        pauseButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);

        // 플레이어 이동 시작
        playerController.StartMoving();
    }
}
