using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public float attackSpeed;
    float count = 0;
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;
    public GameObject hitBoxL;
    public GameObject hitBoxR;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    void OnEnable()
    {
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        // Ű���� �Է¹޾Ƽ� Vector2���� �ֱ�
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        count += Time.fixedDeltaTime;
        if (count > attackSpeed)
        {
            count = 0;
            anim.SetTrigger("Attack");
            if (spriter.flipX)
            {
                hitBoxL.SetActive(true);
                Invoke("HitBoxOff", 1f);
            }
            else
            {
                hitBoxR.SetActive(true);
                Invoke("HitBoxOff", 1f);
            }
        }

        // ��� ������ �ӵ��� ���� normalized X �ӵ�
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    void LateUpdate()
    {
        // Speed �Ķ���Ͱ��� vector ��ü�� ũ�Ⱚ���� ����
        anim.SetFloat("Speed", inputVec.magnitude);

        // �¿� ����Ű ��������
        if (inputVec.x != 0)
        {
            // ���� ����Ű�� true
            spriter.flipX = inputVec.x < 0;
        }
    }

    // Collider�� �浹 ���϶� ����
    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
            return;
        // ����ִٸ� �Ʒ� ����

        anim.SetTrigger("Hit");
        GameManager.instance.health -= Time.deltaTime * 10;

        if (GameManager.instance.health <= 0)
        {
            for (int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }

    void HitBoxOff()
    {
        hitBoxL.SetActive(false);
        hitBoxR.SetActive(false);
    }
}
