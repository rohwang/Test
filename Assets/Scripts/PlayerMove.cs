using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 10f;        // 좌우 이동 속도

    [Header("지면 체크")]
    public Transform groundCheck;        // 지면 체크용 위치 (플레이어 발 밑에 빈 GameObject)
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;        // 지면으로 인식할 레이어
    private bool isGrounded;

    [Header("점프 설정")]
    public float jumpForce = 12f;        // 점프 힘

    [Header("대쉬 설정")]
    public float dashForce = 24f;        //  대쉬 힘
    public float dashTime = 0.2f;           //  대쉬 지속 시간
    public float dashCoolDown = 1f;       //  대쉬 쿨다운
    private bool canDash = true;
    private bool IsDashing;

    private SpriteRenderer playerSpriteRenderer;
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] TrailRenderer tr;
    [SerializeField] CapsuleCollider2D col;


    void Awake()
    {
        tr.emitting = false;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(IsDashing)
        {
            return;
        }
        Move();
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        Jump();
    }

    private void FixedUpdate()
    {
        if (IsDashing) {
            return;
        }
    }

    private void Move()
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
    }


    private void Jump()
    {
        // 1) 지면 충돌 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 2) 점프 입력 처리
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private IEnumerator Dash()
    {
        float watchDirection = transform.rotation.y;

        canDash = false;                               //  대쉬 중 대쉬 불가능
        IsDashing = true;                              //   대쉬 도중이다.

        col.enabled = false;                           // 무적 시간 시작 

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;                          //   대쉬 중에는 중력의 영향을 받지 않는다.

        {
            if(watchDirection == 0 || watchDirection == -180) //  우측을 볼 때
            {
            rb.linearVelocity = new Vector2(transform.localScale.x * dashForce, 0f);
            }
            else //  좌측을 볼 때
            {
                rb.linearVelocity = new Vector2(-transform.localScale.x * dashForce, 0f);
            }
        }

        tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        tr.emitting = false;

        col.enabled = true;                             //  무적 시간 해제

        rb.gravityScale = originalGravity;              // 중력 영향 시작
        IsDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }
}