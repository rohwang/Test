using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public bool canMove = true;

    public Transform player;
    public Transform transformplayer;

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
        // 플레이어의 위치를 향해 이동
        if (canMove) {
            MoveTowardsPlayer();
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
