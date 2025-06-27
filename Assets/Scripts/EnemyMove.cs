using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Transform player;
    private Animator anim;
    private Rigidbody2D rb;
    private float currentSpeed;
    private Enemy enemy;

    [Header("�ν� ����")]
    public float recognizeDistance = 10;
    private bool recognizedPlayer; //  �÷��̾ �νĵǾ��°�

    [Header("�̵� ����")]
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

        // �÷��̾��� ��ġ�� ���� �̵�
        if (canMove && player != null && recognizedPlayer && !enemy.isAttack)
        {
            MoveTowardsPlayer();
        }
        else if (player == null)
        {
            Debug.Log("�÷��̾��� ���簡 �������� ����");
        }
        else if (!recognizedPlayer)
        {
            Debug.Log("�÷��̾ �ν� ���� �ٱ��� ����");
        }
    }

    private void RecognizePlayer () //  �÷��̾ �ν� ���� ���� �ִ��� Ȯ���մϴ�.
    {
        float distance = (player.position - transform.position).magnitude;

        if (player.position == null)
        {
            Debug.Log("�÷��̾��� ��ġ Ȯ�� �Ұ���, �������� ���Դϴ�.");
        }
        if (distance < recognizeDistance)    //  �÷��̾ �ν� ���� ���� ���� ���� üũ��.
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


        // �̵��� �Ÿ� ���
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }


    // �� �信�� �ν� ���� �ð�ȭ
    private void OnDrawGizmosSelected()
    {
        float distance = (player.position - transform.position).magnitude;

        if (player == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, recognizeDistance);
    }
}
