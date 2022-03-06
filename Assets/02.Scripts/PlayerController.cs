using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 점프
    public float jumpForce = 700f;
    // 사망오디오 클립
    public AudioClip deathClip;

    //누적 점프
    int jumpCount = 0;
    // 바닥확인
    bool isGrounded = false;
    // 사망상태
    bool isDead = false;
    // 이동 리지드바디
    Rigidbody2D playerRigidbody;
    // 오디오소스
    AudioSource playerAudio;
    // 애니메이터 
    Animator animator;

    void Start()
    {
        // 초기화 + 변수 할당
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;

        if(Input.GetMouseButtonDown(0) && jumpCount < 2)
        {
            jumpCount++;
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(new Vector2(0, jumpForce));
            playerAudio.Play();
            //transform.Translate(new Vector2(0f, 5f));
            //transform.position = new Vector2(0f, 5f);
        }
        else if(Input.GetMouseButtonUp(0) && playerRigidbody.velocity.y > 0)
        {
            playerRigidbody.velocity = playerRigidbody.velocity * 0.5f;
        }

        animator.SetBool("Grounded", isGrounded);
        Debug.Log(transform.position.y);
    }

    void Die()
    {
        animator.SetTrigger("Die");
        playerAudio.clip = deathClip;
        playerAudio.Play();
        playerRigidbody.velocity = Vector2.zero;
        isDead = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;
            jumpCount =  0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Dead" && !isDead)
        {
            Die();
        }
    }
}
