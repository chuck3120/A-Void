using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public bool isPaused { get; private set; } = false; // 게임 일시 정지 여부
    public bool isGameStarted { get; private set; } = false; // 게임 시작 여부
    public GameObject gameOverUI; // 게임 오버 UI
    public TextMeshProUGUI coinScoreText; // 코인 점수를 표시할 TMP Text
    public TextMeshProUGUI weaponScoreText; // 무기 점수를 표시할 TMP Text
    private int coinScore = 0; // 코인 점수
    private int weaponScore = 0; // 무기 점수
    private CoinSpawner coinSpawner; // CoinSpawner 참조
    private WeaponPool weaponPool; // WeaponPool 참조

    private void Start()
    {
        // CoinSpawner와 WeaponPool 찾기
        coinSpawner = FindObjectOfType<CoinSpawner>();
        weaponPool = FindObjectOfType<WeaponPool>();
    }

    // 게임 일시 정지 함수
    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f; // 게임 시간 멈춤
    }

    // 게임 재개 함수
    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f; // 게임 시간 정상화
    }

    // 게임 일시 정지 및 재개 전환 함수
    public void TogglePause()
    {
        if (isPaused)
        {
            Resume(); // 게임 재개
        }
        else
        {
            Pause(); // 게임 일시 정지
        }
    }

    // 게임 시작 함수
    public void StartGame()
    {
        isGameStarted = true;
        Resume(); // 게임 재개
        if (coinSpawner != null)
        {
            coinSpawner.StartSpawning(); // 코인 생성 시작
        }
        else
        {
            Debug.LogError("CoinSpawner를 찾을 수 없습니다.");
        }
        if (weaponPool != null)
        {
            weaponPool.StartSpawning(); // 무기 생성 시작
        }
        else
        {
            Debug.LogError("WeaponPool을 찾을 수 없습니다.");
        }
    }

    // 게임 오버 함수
    public void GameOver()
    {
        Pause(); // 게임 일시 정지
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true); // 게임 오버 UI 활성화
        }
        Debug.Log("Game Over");
        DeactivateAllGameObjects(); // 모든 게임 오브젝트 비활성화
    }

    // 게임 재시작 함수
    public void RestartGame()
    {
        isGameStarted = false;
        Resume(); // 게임 재개
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬 다시 로드
    }

    // 코인 점수 증가 함수
    public void AddCoinScore(int amount)
    {
        coinScore += amount; // 코인 점수 증가
        Debug.Log("Coin Score: " + coinScore);
        UpdateCoinScoreUI(); // 코인 점수 UI 업데이트
    }

    // 무기 점수 증가 함수
    public void Score(int amount)
    {
        weaponScore += amount; // 무기 점수 증가
        Debug.Log("Weapon Score: " + weaponScore);
        UpdateWeaponScoreUI(); // 무기 점수 UI 업데이트
    }

    // 코인 점수 UI 업데이트 함수
    private void UpdateCoinScoreUI()
    {
        if (coinScoreText != null)
        {
            coinScoreText.text = "Coins: " + coinScore; // 코인 점수 텍스트 업데이트
        }
    }

    // 무기 점수 UI 업데이트 함수
    private void UpdateWeaponScoreUI()
    {
        if (weaponScoreText != null)
        {
            weaponScoreText.text = "Score: " + weaponScore; // 무기 점수 텍스트 업데이트
        }
    }

    // 모든 게임 오브젝트 비활성화 함수
    private void DeactivateAllGameObjects()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>(); // 모든 게임 오브젝트 찾기

        foreach (GameObject obj in allObjects)
        {
            if (obj != gameOverUI && obj != this.gameObject) // gameOverUI와 GameManager는 제외
            {
                obj.SetActive(false); // 오브젝트 비활성화
            }
        }
    }
}
