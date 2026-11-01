using UnityEngine;

public class WindmillRotator : MonoBehaviour
{
    [Header("旋轉速度 (度/秒)")]
    // 預設繞著 Z 軸每秒旋轉 90 度，可在 Inspector 視窗微調 X, Y, Z
    public Vector3 rotationSpeed = new Vector3(0f, 0f, 90f);

    void Update()
    {
        // 依照設定的速度與時間，對物件自身 (Space.Self) 進行持續旋轉
        transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);
    }
}