using UnityEngine;

public class Weapon : MonoBehaviour
{
    private GameManager gameManager; // GameManager 참조

    private void Start()
    {
        // 게임 매니저 찾기
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        // 무기가 화면 아래로 벗어나면 비활성화
        if (transform.position.y < -10f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어와 충돌 시 게임 오버
        if (collision.CompareTag("Player"))
        {
            GameOver();
        }
        // Ground 태그를 가진 오브젝트와 충돌 시 점수 증가 및 무기 비활성화
        else if (collision.CompareTag("Ground"))
        {
            gameManager.Score(1); // 점수 10점 증가
            gameObject.SetActive(false); // 무기 비활성화
        }
    }

    private void GameOver()
    {
        // 게임 매니저가 존재할 경우 게임 오버 처리
        if (gameManager != null)
        {
            gameManager.GameOver();
        }
        else
        {
            Debug.LogError("GameManager를 찾을 수 없습니다."); // 에러 메시지 출력
        }
        gameObject.SetActive(false); // 충돌 시 무기를 비활성화
    }
}
