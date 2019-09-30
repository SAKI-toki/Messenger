using UnityEngine;

/// <summary>
/// プレイヤーの制御クラス
/// </summary>
[RequireComponent(typeof(ExamineController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("移動速度")]
    float moveSpeed = 1.0f;
    new Rigidbody rigidbody = null;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        //地面に落としておく
        Physics.autoSimulation = false;
        Physics.Simulate(10.0f);
        Physics.autoSimulation = true;
    }

    void Update()
    {
        Move();
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    void Move()
    {
        float vertical = SwitchInput.GetVertical(0);
        float horizontal = SwitchInput.GetHorizontal(0);
        rigidbody.MovePosition(transform.position +
        (transform.forward * vertical + transform.right * horizontal) * moveSpeed * Time.deltaTime);
    }

}
