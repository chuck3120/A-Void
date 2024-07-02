using UnityEngine;

public class Coin : MonoBehaviour
{
    private GameManager gameManager; // GameManager 참조
    private CoinSpawner coinSpawner; // CoinSpawner 참조

    private void Start()
    {
        // 게임 매니저 및 코인 스포너 찾기
        gameManager = FindObjectOfType<GameManager>();
        coinSpawner = FindObjectOfType<CoinSpawner>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어와 충돌 시
        if (collision.CompareTag("Player"))
        {
            CollectCoin(); // 코인 수집
            gameObject.SetActive(false); // 플레이어와 충돌 시 코인을 비활성화
            coinSpawner.SpawnNextCoin(); // 다음 코인을 생성하도록 CoinSpawner에게 알림
        }
    }

    private void CollectCoin()
    {
        // 게임 매니저가 존재할 경우
        if (gameManager != null)
        {
            gameManager.AddCoinScore(10); // 코인 점수 10점 증가
            Debug.Log("Coin Collected"); // 디버그 메시지 출력
        }
        else
        {
            Debug.LogError("GameManager를 찾을 수 없습니다."); // 에러 메시지 출력
        }
    }
}
