using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public bool canMove = true;

    public Transform player;
    public Transform transformplayer;

    [SerializeField] private bool recognizedPlayer; //  �÷��̾ �νĵǾ��°�
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

        // �÷��̾��� ��ġ�� ���� �̵�
        if (canMove && player != null && recognizedPlayer) {
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

        if (distance < 10)    //  �÷��̾ �ν� ���� ���� ���� ���� üũ��.
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
