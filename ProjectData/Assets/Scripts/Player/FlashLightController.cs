using UnityEngine;

/// <summary>
/// ジャイロの回転を取得
/// </summary>
public class FlashLightController : MonoBehaviour
{
    [SerializeField]
    Vector3 rotLimitAbs = new Vector3();
    void Update()
    {
        var prevRotation = transform.localRotation;
        //コントローラーの向きと同じ方向に回転する
        transform.localRotation = SwitchGyro.GetGyro(0);
        var eulerAngles = transform.localEulerAngles;
        //X軸回転の制御
        if (eulerAngles.x > rotLimitAbs.x &&
        eulerAngles.x < 360 - rotLimitAbs.x)
        {
            eulerAngles.x = (eulerAngles.x < 180 ? 1 : -1) * rotLimitAbs.x;
        }
        //Y軸回転の制御
        if (eulerAngles.y > rotLimitAbs.y &&
        eulerAngles.y < 360 - rotLimitAbs.y)
        {
            eulerAngles.y = (eulerAngles.y < 180 ? 1 : -1) * rotLimitAbs.y;
        }
        //Z軸回転の制御
        if (eulerAngles.z > rotLimitAbs.z &&
        eulerAngles.z < 360 - rotLimitAbs.z)
        {
            eulerAngles.z = (eulerAngles.z < 180 ? 1 : -1) * rotLimitAbs.z;
        }
        transform.localEulerAngles = eulerAngles;
        //前を向くようにリセット
        if (SwitchInput.GetButtonDown(0, SwitchButton.Up))
        {
            SwitchGyro.SetBaseGyro(0);
        }
    }
}
