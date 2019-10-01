using UnityEngine;
#if UNITY_SWITCH  && !(UNITY_EDITOR)
using nn.swkbd;
#endif

/// <summary>
/// プレイヤー名の入力
/// </summary>
public class InputPlayerName : MonoBehaviour
{
    //最小の名前の長さ
    const int MinLength = 2;
    //最大の名前の長さ
    const int MaxLength = 8;
    [SerializeField]
    UnityEngine.UI.Text inputText = null;
    [SerializeField]
    UnityEngine.UI.Image blackImage = null;
    //キーボードを表示するかどうか
    //背景を暗くするために1フレーム後にキーボードを表示
    bool showKeyboardFlag = false;

#if UNITY_SWITCH && !(UNITY_EDITOR)
    ShowKeyboardArg showKeyboardArg;

    void Start()
    {
        //ソフトウェアキーボードの初期化
        Swkbd.Initialize(ref showKeyboardArg, false);
        inputText.text = "";
        blackImage.color = new Color(0, 0, 0, 0);
    }

    void Update()
    {
        //背景を暗くするために1フレーム後にキーボードを表示
        if (showKeyboardFlag)
        {
            ShowKeyboard();
            showKeyboardFlag = false;
            blackImage.color = new Color(0, 0, 0, 0);
        }
        else if (SwitchInput.GetButtonDown(0, SwitchButton.Down))
        {
            showKeyboardFlag = true;
            blackImage.color = new Color(0, 0, 0, 0.8f);
        }
        else if(SwitchInput.GetButtonDown(0, SwitchButton.Up))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }
    }

    /// <summary>
    /// キーボードの表示
    /// </summary>
    void ShowKeyboard()
    {
        //キーボードの設定の初期化
        Swkbd.InitializeKeyboardConfig(ref showKeyboardArg.keyboardConfig);
        //テキストの初期化(ここでは空で初期化する)
        Swkbd.SetInitialText(ref showKeyboardArg, "");

        //テキストのサイズを設定
        showKeyboardArg.keyboardConfig.textMinLength = MinLength;
        showKeyboardArg.keyboardConfig.textMaxLength = MaxLength;

        System.Text.StringBuilder resultString = new System.Text.StringBuilder(Swkbd.TextMaxLength);
        //キーボードを表示し、その間MainThreadは停止する
        var result = Swkbd.ShowKeyboard(resultString, showKeyboardArg, true);
        if (result.IsSuccess())
        {
            //結果をUIに表示
            inputText.text = resultString.ToString();
        }
    }

    void OnDestroy()
    {
        Swkbd.Destroy(ref showKeyboardArg);
    }
#endif
}
