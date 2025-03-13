using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // �����յ��� ������ ����
    public GameObject[] prefabs;

    // Ǯ ����� �ϴ� ����Ʈ��
    List<GameObject>[] pools;

    void Awake()
    {
        // �������� �� ���� Ǯ�� �� ��
        pools = new List<GameObject>[prefabs.Length];

        // ����Ʈ�� �ִ� ��� ������ �ʱ�ȭ
        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    // ���� ������Ʈ�� ��ȯ�ϴ� �Լ�
    public GameObject Get(int index)
    {
        // ����ִ� ������Ʈ �ϳ��� �����ϴ� ����
        GameObject select = null;

        // ������ Ǯ�� ��� (��Ȱ��ȭ ��) �ִ� ���ӿ�����Ʈ ����
        foreach (GameObject item in pools[index]) // �迭, ����Ʈ���� �����͸� ���������� �����ϴ� �ݺ���
        {
            // ���빰 ������Ʈ�� ��Ȳ��ȭ(��� ����)���� Ȯ��
            if (!item.activeSelf)
            {
                // �߰��ϸ� select ������ �Ҵ�
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // �� ã������
        if (!select)
        {
            // ���Ӱ� �����ϰ� select ������ �Ҵ�
            select = Instantiate(prefabs[index], transform); // ���� ������Ʈ�� �����Ͽ� �����ϴ� �Լ�
            pools[index].Add(select); // ������ ������Ʈ�� �ش� ������Ʈ Ǯ ����Ʈ�� Add �Լ��� �߰�
        }

        return select;
    }
}
