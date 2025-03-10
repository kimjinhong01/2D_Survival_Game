using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionEnemy : MonoBehaviour
{
    Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area Enemy"))
            return;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position; // �� ��ġ

        switch (transform.tag)
        {
            case "Enemy":
                if (coll.enabled) // collider�� Ȱ��ȭ�Ǿ� �ִٸ�
                {
                    /* �ӽ� ����
                    Vector3 dist = playerPos - myPos; // �÷��̾���� �Ÿ�
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0); // ���� ��ġ
                    transform.Translate(ran + dist * 2); // ���� ��ġ + �Ÿ� x 2 ��ŭ �̵���Ų��
                    */

                    this.GetComponent<Enemy>().health = 0;
                }
                break;
        }
    }
}
