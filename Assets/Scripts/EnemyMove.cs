using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public bool canMove = true;

    public Transform player;
    public Transform transformplayer;

    [SerializeField] private bool recognizedPlayer; //  플레이어가 인식되었는가
    private Animator anim;
    public float moveSpeed = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        anim = GetComponent<Animator>();
        canMove = true;
    }
    void Update()
    {
        RecognizePlayer();

        // 플레이어의 위치를 향해 이동
        if (canMove && player != null && recognizedPlayer) {
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

        if (distance < 10)    //  플레이어가 인식 범위 내에 있을 때를 체크함.
        {
            recognizedPlayer = true;
        }
        else
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
        anim.SetBool("Run", true);


        // 이동할 거리 계산
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }
}
