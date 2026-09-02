using UnityEngine;

public class BoarSpawner : MonoBehaviour
{
    public GameObject boarPrefab;

    [Header("出現設定")]
    public float spawnDistance = 1f;

    [Header("通常時")]
    public float minSpawnInterval = 3f;
    public float maxSpawnInterval = 7f;
    public float normalBoarSpeed = 5f;

    [Header("ラストスパート")]
    public float lastMinSpawnInterval = 1f;
    public float lastMaxSpawnInterval = 3f;
    public float lastBoarSpeed = 8f;

    private float spawnTimer;

    private Timer timer;

    // 前回の状態
    private bool feverStarted = false;
    private bool lastSpurtStarted = false;

    void Start()
    {
        timer = FindFirstObjectByType<Timer>();

        spawnTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void Update()
    {
        if (timer == null)
        {
            timer = FindFirstObjectByType<Timer>();
            return;
        }

        float remainingTime = timer.timeremaining;

        // ==========================================
        // ?? フィーバータイム（残り60～50秒）
        // ==========================================
        if (remainingTime <= 60f && remainingTime > 50f)
        {
            // フィーバー開始時に一度だけ処理
            if (!feverStarted)
            {
                feverStarted = true;

                // イノシシを出さない
                spawnTimer = 999f;
            }

            return;
        }

        // ==========================================
        // ? ラストスパート（残り50秒～0秒）
        // ==========================================
        if (remainingTime <= 50f && remainingTime > 0f)
        {
            // ラストスパート開始時に一度だけ処理
            if (!lastSpurtStarted)
            {
                lastSpurtStarted = true;

                // ラストスパート用の出現間隔を設定
                spawnTimer = Random.Range(
                    lastMinSpawnInterval,
                    lastMaxSpawnInterval
                );
            }

            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0f)
            {
                SpawnBoar();

                // 次の出現もラストスパート用
                spawnTimer = Random.Range(
                    lastMinSpawnInterval,
                    lastMaxSpawnInterval
                );
            }

            return;
        }

        // ==========================================
        // 通常時（残り180～60秒）
        // ==========================================
        if (remainingTime > 60f)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0f)
            {
                SpawnBoar();

                spawnTimer = Random.Range(
                    minSpawnInterval,
                    maxSpawnInterval
                );
            }
        }
    }

    void SpawnBoar()
    {
        Camera cam = Camera.main;

        if (cam == null)
        {
            Debug.LogWarning("Main Cameraが見つかりません");
            return;
        }

        float height = cam.orthographicSize;
        float width = height * cam.aspect;

        Vector2 spawnPosition;
        Vector2 direction;

        // 0 = 上
        // 1 = 下
        // 2 = 左
        // 3 = 右
        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0:
                // 上から出現
                spawnPosition = new Vector2(
                    Random.Range(-width, width),
                    height + spawnDistance
                );

                direction = Vector2.down;
                break;

            case 1:
                // 下から出現
                spawnPosition = new Vector2(
                    Random.Range(-width, width),
                    -height - spawnDistance
                );

                direction = Vector2.up;
                break;

            case 2:
                // 左から出現
                spawnPosition = new Vector2(
                    -width - spawnDistance,
                    Random.Range(-height, height)
                );

                direction = Vector2.right;
                break;

            default:
                // 右から出現
                spawnPosition = new Vector2(
                    width + spawnDistance,
                    Random.Range(-height, height)
                );

                direction = Vector2.left;
                break;
        }

        GameObject boarObject = Instantiate(
            boarPrefab,
            spawnPosition,
            Quaternion.identity
        );

        Boar boar = boarObject.GetComponent<Boar>();

        if (boar != null)
        {
            // 残り50秒以下なら高速化
            if (timer.timeremaining <= 50f)
            {
                boar.moveSpeed = lastBoarSpeed;
            }
            else
            {
                boar.moveSpeed = normalBoarSpeed;
            }

            boar.SetDirection(direction);
        }
    }
}