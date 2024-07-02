using UnityEngine;
using System.Collections.Generic;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab; // 코인 프리팹
    public int poolSize = 10; // 코인 풀의 크기
    public float spawnInterval = 1.0f; // 생성 간격 추가
    public float spawnHeight = 5.2f; // 고정된 y축 위치
    public float spawnRangeMinX = -2.5f; // x축 최소값
    public float spawnRangeMaxX = 2.4f; // x축 최대값

    private List<GameObject> coinPool; // 코인 풀 리스트
    private float spawnTimer; // 코인 생성 타이머
    private GameManager gameManager; // GameManager 참조
    private bool isSpawning = false; // 코인 생성 여부

    void Start()
    {
        // 코인 풀 초기화
        coinPool = new List<GameObject>();
        // 게임 매니저 찾기
        gameManager = FindObjectOfType<GameManager>();

        // 풀 크기만큼 코인 프리팹 인스턴스 생성 후 비활성화
        for (int i = 0; i < poolSize; i++)
        {
            GameObject coin = Instantiate(coinPrefab);
            coin.SetActive(false);
            coinPool.Add(coin);
        }
    }

    void Update()
    {
        // 게임이 일시 정지된 상태 또는 게임이 시작되지 않은 상태에서는 코인을 생성하지 않음
        if (!isSpawning || (gameManager != null && (!gameManager.isGameStarted || gameManager.isPaused)))
        {
            return;
        }

        // 생성 타이머 갱신
        spawnTimer += Time.deltaTime;

        // 생성 타이머가 생성 간격을 넘었을 경우 코인 생성
        if (spawnTimer >= spawnInterval)
        {
            SpawnCoin();
            spawnTimer = 0; // 타이머 초기화
        }
    }

    // 코인 생성 시작
    public void StartSpawning()
    {
        Debug.Log("코인 생성 시작");
        isSpawning = true; // 생성 플래그 설정
        spawnTimer = 0; // 생성 타이머 초기화
    }

    // 코인 생성 중지
    public void StopSpawning()
    {
        Debug.Log("코인 생성 중지");
        isSpawning = false; // 생성 플래그 해제
    }

    // 다음 코인 생성
    public void SpawnNextCoin()
    {
        // 풀에서 비활성화된 코인 찾기
        foreach (GameObject coin in coinPool)
        {
            if (!coin.activeInHierarchy)
            {
                // 무작위 x축 위치 설정
                float spawnX = Random.Range(spawnRangeMinX, spawnRangeMaxX);
                // 코인 위치 설정
                coin.transform.position = new Vector3(spawnX, spawnHeight, 0);
                // 코인 활성화
                coin.SetActive(true);
                Debug.Log("Coin Spawned at X: " + spawnX);
                break; // 코인 하나 생성 후 종료
            }
        }
    }

    // 코인 생성 호출
    void SpawnCoin()
    {
        SpawnNextCoin();
    }
}
