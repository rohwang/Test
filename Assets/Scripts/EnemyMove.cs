using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Transform player;
    private Animator anim;
    private Rigidbody2D rb;
    private float currentSpeed;
    private Enemy enemy;

    [Header("인식 설정")]
    public float recognizeDistance = 10;
    private bool recognizedPlayer; //  플레이어가 인식되었는가

    [Header("이동 설정")]
    public float moveSpeed = 1f;
    public bool canMove = true;


    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canMove = true;

        currentSpeed = rb.linearVelocity.x;
    }
    void Update()
    {
        RecognizePlayer();

        if(currentSpeed == 0)
        {
            anim.SetBool(name = "Run", false);
            anim.SetTrigger(name = "Idle");
        }
        else if(currentSpeed != 0)
        {
            anim.SetBool(name="Run", true);
        }

        // 플레이어의 위치를 향해 이동
        if (canMove && player != null && recognizedPlayer && !enemy.isAttack)
        {
            MoveTowardsPlayer();
        }
        else if (player == null)
        {
            Debug.Log("플레이어의 존재가 감지되지 않음");
        }
        else if (!recognizedPlayer)
        {
            Debug.Log("플레이어가 인식 범위 바깥에 있음");
        }
    }

    private void RecognizePlayer () //  플레이어가 인식 범위 내에 있는지 확인합니다.
    {
        float distance = (player.position - transform.position).magnitude;

        if (player.position == null)
        {
            Debug.Log("플레이어의 위치 확인 불가능, 포지션이 널입니다.");
        }
        if (distance < recognizeDistance)    //  플레이어가 인식 범위 내에 있을 때를 체크함.
        {
            recognizedPlayer = true;
        }
        else if (distance >= recognizeDistance)
        {
            recognizedPlayer = false;
        }
    }


    public void MoveTowardsPlayer()
    {
        // 플레이어와 적의 방향 벡터 계산
        Vector3 rot = transform.rotation.eulerAngles;

        Vector2 direction = (player.position - transform.position).normalized;
        if (direction.x > 0)
        {
            transform.rotation = Quaternion.Euler(rot.x, 180, rot.z);
        }
        else if (direction.x < 0)
        {
            transform.rotation = Quaternion.Euler(rot.x, 0, rot.z);
        }


        // 이동할 거리 계산
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }


    // 씬 뷰에서 인식 범위 시각화
    private void OnDrawGizmosSelected()
    {
        float distance = (player.position - transform.position).magnitude;

        if (player == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, recognizeDistance);
    }
}
