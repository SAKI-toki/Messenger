using UnityEngine;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// メッセージリスト
/// </summary>
static public class MessageList
{
    static List<MessageInfo> messageInfoList;

    /// <summary>
    /// メッセージを取得
    /// </summary>
    static MessageInfo GetMessage(int index)
    {
        LoadMessage();
        return messageInfoList[index];
    }

    /// <summary>
    /// メッセージの数を取得
    /// </summary>
    static int GetMessageNum()
    {
        return messageInfoList.Count;
    }

    /// <summary>
    /// メッセージの読み込み
    /// </summary>
    static void LoadMessage()
    {
        if (messageInfoList != null) return;
        //リストを生成
        messageInfoList = new List<MessageInfo>();
        //メッセージリストを読み込む
        TextAsset messageAsset = (TextAsset)Resources.Load("MessageAsset");
        StringReader stringReader = new StringReader(messageAsset.text);
        Resources.UnloadAsset(messageAsset);
        //全てのメッセージをリストに追加する
        while (stringReader.Peek() != -1)
        {
            MessageInfo messageInfo = new MessageInfo();
            messageInfoList.Add(messageInfo);
        }
    }
}

/// <summary>
/// メッセージの情報
/// </summary>
public class MessageInfo
{
    //選択できる単語の数(定数)
    public const int SelectWordCount = 3;

    //位置
    //TODO

    //選択したワード
    int[] selectWords = new int[SelectWordCount];

    /// <summary>
    /// ワードの取得
    /// </summary>
    public string GetWord(int index)
    {
        return WordList.GetWord(selectWords[index]);
    }
}
