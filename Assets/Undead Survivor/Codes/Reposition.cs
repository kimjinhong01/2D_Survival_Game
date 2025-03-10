using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    // Collider ������ ����
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area")) // CompareTag : �±� ��
            return;
        // Area�� ����ٸ� �Ʒ� ��ɾ� ����

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position; // Ÿ�ϸ� ��ġ
        
        switch (transform.tag)
        {
            case "Ground": // Area�� Ÿ�ϸ��� ����ٸ�
                float diffX = playerPos.x - myPos.x; // �¿� �Ÿ�
                float diffY = playerPos.y - myPos.y; // ���� �Ÿ�
                float dirX = diffX < 0 ? -1 : 1; // �¿� ����
                float dirY = diffY < 0 ? -1 : 1; // ���� ����
                diffX = Mathf.Abs(diffX); // Abs : ����
                diffY = Mathf.Abs(diffY);

                if (diffX > diffY) // �¿� �Ÿ��� ���� �Ÿ����� �ָ�
                {
                    // Ÿ���� �¿�� �ű��, Translate : �ش� ���͸�ŭ �̵�
                    transform.Translate(Vector3.right * dirX * 80);
                }
                if (diffY > diffX) // ���� �Ÿ��� �¿� �Ÿ����� �ָ�
                {
                    // Ÿ���� ���Ϸ� �ű��
                    transform.Translate(Vector3.up * dirY * 80);
                }
                break;
        }
    }
}
