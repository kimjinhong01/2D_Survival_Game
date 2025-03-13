using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float levelTime;

    int level;
    float timer;

    void Awake()
    {
        // �ڽ��� ������ ��� �ڽ��� Transform ������Ʈ�� ������
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
        // 120�� / 6�� ���Ͷ�� -> 20�� ���� level�� �ö󰣴�
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        timer += Time.deltaTime;
        // �ּҰ�(�ε��� ���� ���ſ�), �Ҽ��� ������ Int�� ��ȯ(���� �ð� / ���� �ٲ�� �ð�)
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnData.Length - 1);

        if (timer > spawnData[level].spawnTime)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        // enemy�� 0��°�� �ֱ� ������ Get(0)
        GameObject enemy = GameManager.instance.pool.Get(0);
        // ��������Ʈ �߿� �� ������ �������� ���� (0�� �ڱ� �ڽ��̶� 1����)
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        // enemy�� ���� ������ �´� spawnData ����
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

// spawnData Ŭ���� ����, �ۿ��� ���� �����ϰ�
[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}