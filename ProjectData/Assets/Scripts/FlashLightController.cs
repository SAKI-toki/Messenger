using UnityEngine;

/// <summary>
/// ジャイロの回転を取得
/// </summary>
public class FlashLightController : MonoBehaviour
{
    [SerializeField, Range(0, 180)]
    float zRotLimitAbs = 15.0f;
    void Update()
    {
        //コントローラーの向きと同じ方向に回転する
        transform.rotation = SwitchGyro.GetGyro(0);
        //Z軸回転の制御
        var eulerAngles = transform.eulerAngles;
        if (eulerAngles.z > zRotLimitAbs &&
        eulerAngles.z < 360 - zRotLimitAbs)
        {
            eulerAngles.z = (eulerAngles.z < 180 ? 1 : -1) * zRotLimitAbs;
        }
        transform.eulerAngles = eulerAngles;
        //後ろにコントローラーが向いたら初期位置に戻す
        if (transform.forward.z < 0)
        {
            transform.rotation = Quaternion.identity;
        }
        //前を向くようにリセット
        if (SwitchInput.GetButtonDown(0, SwitchButton.Up))
            SwitchGyro.SetBaseGyro(0);
    }
}
