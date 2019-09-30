using UnityEngine;

/// <summary>
/// 視点移動
/// </summary>
public class ViewpointMovement : MonoBehaviour
{
    [SerializeField, Header("カメラ")]
    Transform cameraTransform = null;
    [SerializeField, Header("プレイヤー")]
    Transform playerTransform = null;
    [SerializeField, Header("懐中電灯")]
    Transform flashLightTransform = null;

    ViewpointMovementConfig.VM vm = ViewpointMovementConfig.VM.GyroXStickY;
    void Update()
    {
        if (SwitchInput.GetButtonDown(0, SwitchButton.Up))
            ++vm;
        else if (SwitchInput.GetButtonDown(0, SwitchButton.Down))
            --vm;
        if (vm < 0) vm = 0;
        if (vm == ViewpointMovementConfig.VM.None) vm = ViewpointMovementConfig.VM.None - 1;
        ViewpointMovementConfig.GetInstance().SetViewpointMovement(vm);

        ViewpointMovementConfig.GetInstance().GetViewpointMovement().
            ViewpointMovementImpl(cameraTransform, playerTransform, flashLightTransform);
    }
}