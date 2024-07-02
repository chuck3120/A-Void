using UnityEngine;
using System.Collections.Generic;

public class WeaponPool : MonoBehaviour
{
    public GameObject[] weaponPrefabs; // 여러 종류의 무기 프리팹 배열
    public int poolSize = 10; // 풀의 크기
    public float spawnInterval = 1.0f; // 생성 간격
    public float spawnHeight = 5.2f; // 고정된 y축 위치
    public float spawnRangeMinX = -2.5f; // x축 최소값
    public float spawnRangeMaxX = 2.4f; // x축 최대값

    private List<GameObject> weaponPool; // 무기 풀 리스트
    private float spawnTimer; // 생성 타이머
    private GameManager gameManager; // GameManager 참조
    private bool isSpawning = false; // 무기 생성 여부

    void Start()
    {
        // 무기 풀 초기화
        weaponPool = new List<GameObject>();
        // 게임 매니저 찾기
        gameManager = FindObjectOfType<GameManager>();

        // 풀 크기만큼 무기 프리팹 인스턴스 생성 후 비활성화
        for (int i = 0; i < poolSize; i++)
        {
            GameObject weapon = Instantiate(GetRandomWeaponPrefab());
            weapon.SetActive(false);
            weaponPool.Add(weapon);
        }
    }

    void Update()
    {
        // 게임이 일시 정지된 상태 또는 게임이 시작되지 않은 상태에서는 무기를 생성하지 않음
        if (!isSpawning || (gameManager != null && (!gameManager.isGameStarted || gameManager.isPaused)))
        {
            return;
        }

        // 생성 타이머 갱신
        spawnTimer += Time.deltaTime;

        // 생성 타이머가 생성 간격을 넘었을 경우 무기 생성
        if (spawnTimer >= spawnInterval)
        {
            SpawnWeapon();
            spawnTimer = 0; // 타이머 초기화
        }
    }

    // 무기 프리팹 중 무작위로 선택
    GameObject GetRandomWeaponPrefab()
    {
        int index = Random.Range(0, weaponPrefabs.Length);
        return weaponPrefabs[index];
    }

    // 무기 생성 시작
    public void StartSpawning()
    {
        Debug.Log("무기 생성 시작");
        isSpawning = true; // 생성 플래그 설정
        spawnTimer = 0; // 생성 타이머 초기화
    }

    // 무기 생성 중지
    public void StopSpawning()
    {
        Debug.Log("무기 생성 중지");
        isSpawning = false; // 생성 플래그 해제
    }

    // 무기 생성
    void SpawnWeapon()
    {
        // 풀에서 비활성화된 무기 찾기
        foreach (GameObject weapon in weaponPool)
        {
            if (!weapon.activeInHierarchy)
            {
                // 무작위 x축 위치 설정
                float spawnX = Random.Range(spawnRangeMinX, spawnRangeMaxX);
                // 무기 위치 설정
                weapon.transform.position = new Vector3(spawnX, spawnHeight, 0);
                // 무기 회전 설정 (Z축 회전 값을 180도로 설정)
                weapon.transform.rotation = Quaternion.Euler(0, 0, 180);
                // 무기 활성화
                weapon.SetActive(true);
                Debug.Log("Weapon Spawned at X: " + spawnX);
                break; // 무기 하나 생성 후 종료
            }
        }
    }
}
