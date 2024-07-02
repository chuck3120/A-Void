using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f; // 플레이어 이동 속도
    private bool isMoving = false; // 플레이어 이동 여부
    private bool isMovingLeft = true; // 플레이어 이동 방향
    public Animator animator; // 플레이어 애니메이터

    private float leftBound = -2.4f; // 왼쪽 경계
    private float rightBound = 2.5f; // 오른쪽 경계

    void Start()
    {
        // 애니메이터가 설정되지 않은 경우 컴포넌트에서 찾기
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        // 터치 및 마우스 입력 처리
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            // 클릭 시 이동 방향 전환
            isMovingLeft = !isMovingLeft;

            // 이동을 시작한 적이 없다면 이동 시작
            if (!isMoving)
            {
                StartMoving();
            }
        }

        // 이동 처리
        if (isMoving)
        {
            // 이동 방향에 따라 위치 및 스케일 변경
            if (isMovingLeft)
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
                transform.localScale = new Vector3(1, 1, 1); // 왼쪽으로 이동할 때 원래 방향
            }
            else
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                transform.localScale = new Vector3(-1, 1, 1); // 오른쪽으로 이동할 때 x축 반전
            }

            // 플레이어의 위치를 경계 내로 제한하고 경계에 도달하면 이동 방향 전환
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, leftBound, rightBound);

            if (clampedPosition.x <= leftBound)
            {
                isMovingLeft = false;
                transform.localScale = new Vector3(-1, 1, 1); // 오른쪽으로 이동할 때 x축 반전
            }
            else if (clampedPosition.x >= rightBound)
            {
                isMovingLeft = true;
                transform.localScale = new Vector3(1, 1, 1); // 왼쪽으로 이동할 때 원래 방향
            }

            transform.position = clampedPosition;
        }

        // 애니메이터 파라미터 업데이트
        if (!isMoving)
        {
            animator.SetBool("isRunning", false); // 플레이어가 이동하지 않을 때 애니메이션 비활성화
        }
    }

    // 플레이어 이동 시작
    public void StartMoving()
    {
        isMoving = true; // 이동 플래그 설정
        animator.SetBool("isRunning", true); // 애니메이션 활성화
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.StartGame(); // 게임 매니저에 게임 시작 알림
        }
    }

    // 플레이어 이동 중지
    public void StopMoving()
    {
        isMoving = false; // 이동 플래그 해제
        animator.SetBool("isRunning", false); // 애니메이션 비활성화
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.Pause(); // 게임 매니저에 게임 일시 정지 알림
        }
    }
}
