using UnityEngine;

/// <summary>
/// プレイヤーの制御クラス
/// </summary>
[RequireComponent(typeof(ExamineController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("移動速度")]
    float moveSpeed = 1.0f;
    [SerializeField, Header("回転速度")]
    float rotateSpeed = 1.0f;
    void Start()
    {
        //地面に落としておく
        Physics.autoSimulation = false;
        Physics.Simulate(10.0f);
        Physics.autoSimulation = true;
    }

    void Update()
    {
        Move();
        Rotation();
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    void Move()
    {
        float moveStick = SwitchInput.GetVertical(0);
        transform.Translate(Vector3.forward * moveStick * moveSpeed * Time.deltaTime);
    }
    /// <summary>
    /// 回転処理
    /// </summary>
    void Rotation()
    {
        float rotateStick = SwitchInput.GetHorizontal(0);
        transform.Rotate(0, rotateStick * rotateSpeed * Time.deltaTime, 0);
    }

}
