using System.Collections; // 必須引入此命名空間以使用 Coroutine (協程)
using UnityEngine;

public class PlayerFlightController : MonoBehaviour
{
    [Header("飛行移動設定")]
    public float forwardSpeed = 10f;
    public float turnSpeed = 8f;

    [Header("音效設定")]
    public AudioSource audioSource;
    public AudioClip flightSound;   // 飛行引擎聲
    public AudioClip crashSound;    // 撞擊聲
    public AudioClip passSound;     // 通過聲

    private Rigidbody rb;
    private Vector3 startPosition;     // 儲存起點座標
    private Quaternion startRotation;  // 儲存起點角度
    private bool isCrashing = false;   // 紀錄是否處於撞擊停頓狀態

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 自動獲取同物件身上的 AudioSource
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // 記錄遊戲開啟時飛機的位置與旋轉角度（作為起點）
        startPosition = transform.position;
        startRotation = transform.rotation;

        // 播放背景飛行聲
        if (audioSource != null && flightSound != null)
        {
            audioSource.clip = flightSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void Update()
    {
        // 如果正在撞擊停頓狀態，鎖定玩家操作
        if (isCrashing) return;

        // 讀取鍵盤輸入控制飛行
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, vertical, 1f).normalized;
        transform.Translate(moveDirection * forwardSpeed * Time.deltaTime, Space.Self);
    }

    // 當與「實體障礙物」發生碰撞時觸發
    private void OnCollisionEnter(Collision collision)
    {
        // 撞擊到 Tag 為 Obstacle 且目前不處於撞擊停頓狀態時觸發
        if (collision.gameObject.CompareTag("Obstacle") && !isCrashing)
        {
            // 啟動協程處理撞擊音效與延遲
            StartCoroutine(CrashRoutine());
        }
    }

    // 處理撞擊停頓與返回起點的協程 (Coroutine)
    IEnumerator CrashRoutine()
    {
        isCrashing = true; // 設定為撞擊狀態（暫停 WASD 操控）

        float waitTime = 1.0f; // 預設等待秒數

        // 1. 播放撞擊音效並獲取音效長度
        if (audioSource != null && crashSound != null)
        {
            audioSource.PlayOneShot(crashSound);
            waitTime = crashSound.length; // 自動計算該音效檔案播完需要的秒數
        }

        // 2. 暫停執行以下程式碼，等待音效播完
        yield return new WaitForSeconds(waitTime);

        // 3. 音效播完後，重置飛機回到起點
        ResetToStart();

        isCrashing = false; // 解除撞擊狀態，恢復操控
    }

    // 重置飛機狀態至起點
    void ResetToStart()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;

        // 清除剛體加速度，避免回起點後暴衝
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    // 穿過過關觸發點 (Trigger)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PassPoint") && !isCrashing)
        {
            if (audioSource != null && passSound != null)
            {
                audioSource.PlayOneShot(passSound);
            }
        }
    }
}