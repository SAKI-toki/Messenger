using UnityEngine;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// メッセージリスト
/// </summary>
static public class MessageInfoList
{
    //選択できる単語の数(定数)
    public const int SelectWordCount = 3;
    //メッセージの情報リスト
    static List<MessageInfo> messageInfoList;

    /// <summary>
    /// メッセージを取得
    /// </summary>
    static public MessageInfo GetMessage(int index)
    {
        LoadMessage();
        return messageInfoList[index];
    }

    /// <summary>
    /// メッセージの数を取得
    /// </summary>
    static public int GetMessageCount()
    {
        LoadMessage();
        return messageInfoList.Count;
    }

    /// <summary>
    /// メッセージの書き込み(追加)
    /// </summary>
    static public void AddMessage(MessageInfo messageInfo)
    {
        //追加してはいけないメッセージならreturn
        if (!ISAddMessage(messageInfo)) return;
        messageInfoList.Add(messageInfo);
        WriteMessage(messageInfo);

    }

    /// <summary>
    /// メッセージの読み込み
    /// </summary>
    static void LoadMessage()
    {
        if (messageInfoList != null) return;
        //メッセージの数を読み込む
        int messageCount = PlayerPrefs.GetInt("MessageCount", 0);
        //リストを生成
        messageInfoList = new List<MessageInfo>(messageCount);
        //全てのメッセージをリストに追加する
        for (int i = 0; i < messageCount; ++i)
        {
            int[] words = new int[SelectWordCount];
            for (int j = 0; j < SelectWordCount; ++j)
            {
                words[j] = PlayerPrefs.GetInt(GetWordKey(i, j), 0);
            }
            messageInfoList.Add(new MessageInfo(PlayerPrefs.GetString(PlayerNameKey(i), "NO NAME"), words));
        }
    }

    /// <summary>
    /// メッセージの書き込み
    /// </summary>
    static void WriteMessage(MessageInfo messageInfo)
    {
        int messageCount = PlayerPrefs.GetInt(MessageCountKey, 0);
        //各情報の書き込み
        {
            //単語の書き込み(インデックス)
            for (int i = 0; i < SelectWordCount; ++i)
            {
                PlayerPrefs.SetInt(GetWordKey(messageCount, i), messageInfo.GetWordIndex(i));
            }
            //プレイヤー名の書き込み
            PlayerPrefs.SetString(PlayerNameKey(messageCount), messageInfo.GetPlayerName());
        }
        //一つメッセージの数を増やす
        messageCount += 1;
        PlayerPrefs.SetInt(MessageCountKey, messageCount);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 追加していいメッセージかどうか判定する
    /// </summary>
    static bool ISAddMessage(MessageInfo messageInfo)
    {
        return true;
    }

    //メッセージの数のキー
    const string MessageCountKey = "MessageCount";

    /// <summary>
    /// 単語のキーの取得
    /// </summary>
    static string GetWordKey(int num, int index)
    {
        return num.ToString() + "Word" + index.ToString();
    }
    /// <summary>
    /// プレイヤー名のキーの取得
    /// </summary>
    static string PlayerNameKey(int num)
    {
        return "PlayerName" + num.ToString();
    }
}

/// <summary>
/// メッセージの情報
/// </summary>
public class MessageInfo
{
    public MessageInfo(string name, int[] words)
    {
        playerName = name;
        selectWords = words;
    }

    //プレイヤーの名前
    string playerName;

    //選択したワード
    int[] selectWords;

    /// <summary>
    /// プレイヤーの名前の取得
    /// </summary>
    public string GetPlayerName()
    {
        return playerName;
    }

    /// <summary>
    /// ワードの取得
    /// </summary>
    public string GetWord(int index)
    {
        return WordList.GetWord(GetWordIndex(index));
    }

    /// <summary>
    /// 単語リスト上でのインデックスを取得
    /// </summary>
    public int GetWordIndex(int index)
    {
        return selectWords[index];
    }

}
