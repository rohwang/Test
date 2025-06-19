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
        // �÷��̾��� ��ġ�� ���� �̵�
        if (canMove) {
            MoveTowardsPlayer();
        }
    }


    public void MoveTowardsPlayer()
    {
        // �÷��̾�� ���� ���� ���� ���
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


        // �̵��� �Ÿ� ���
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }
}
