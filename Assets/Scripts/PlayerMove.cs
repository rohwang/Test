using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 10f;         // 좌우 이동 속도

    [Header("지면 체크")]
    public Transform groundCheck;        // 지면 체크용 위치 (플레이어 발 밑에 빈 GameObject)
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;        // 지면으로 인식할 레이어

    [Header("점프 설정")]
    public float jumpForce = 12f;        // 점프 힘

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer playerSpriteRenderer;

    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 1) 좌우 이동
        float horiz = Input.GetAxisRaw("Horizontal");  // -1, 0, +1
        rb.linearVelocity = new Vector2(horiz * moveSpeed, rb.linearVelocity.y);
        //rb.AddForce(Vector2.right * horiz * moveSpeed, ForceMode2D.Force);


        anim.SetBool("Walk", horiz != 0);

            Vector3 rot = transform.rotation.eulerAngles;
        if (horiz < 0)
        {
            transform.rotation = Quaternion.Euler(rot.x, 180 * (horiz < 0 ? 1 : 0), rot.z);
        }
        else if (horiz > 0)
        {
            transform.rotation = Quaternion.Euler(rot.x, 0, rot.z);
        }

            Jump();
    }

    // 에디터에서 지면 체크 영역을 시각화
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    void Jump()
    {
        // 1) 지면 충돌 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 2) 점프 입력 처리
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
