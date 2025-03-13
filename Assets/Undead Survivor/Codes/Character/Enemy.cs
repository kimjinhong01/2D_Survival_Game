using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon; // �ִϸ����� ��Ʈ�ѷ�
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait; // ���� FixedUpdate �Ҷ����� ��ٸ�

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        // ������ ����ִٸ� ����

        // ���� �׾��ų� ���� �ִϸ����� ���°� Hit �̸� ���ư���
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec); // �ش� ��ġ�� �̵�
        rigid.velocity = Vector2.zero; // ���� �ӵ��� �̵��� ������ ���� �ʵ���
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        // ������ ����ִٸ� ����

        if (health <= 0)
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
        }

        if (!isLive)
            return;
        // ���� ����ִٸ� ����

        // �÷��̾ ���ʿ� �ִٸ� true, ������
        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable()
    {
        // Ȱ��ȭ�Ǹ� target�� player�� ����
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        // �ִϸ����͸� data Ÿ�Կ� ���� ����
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((!collision.CompareTag("Bullet") || !isLive) && !collision.CompareTag("HitBox"))
            return;

        if (collision.CompareTag("HitBox"))
        {
            health -= 4.5f;
            Debug.Log("�������� ����!");
        }
        else
        {
            health -= collision.GetComponent<Bullet>().damage;
        }
        StartCoroutine(KnockBack());

        if (health > 0)
        {
            anim.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);

            if (GameManager.instance.isLive) // ������ �� �������� ���� �׷� �ȴ��ϰ�
            {
                GameManager.instance.kill++;
                GameManager.instance.GetExp();
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
            }
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait; // ���� �ϳ��� ���� ������ ������
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 1, ForceMode2D.Impulse);
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
