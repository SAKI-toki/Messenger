using UnityEngine;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// 単語リスト
/// </summary>
static public class WordList
{
    static List<string> wordList;

    /// <summary>
    /// 単語を取得
    /// </summary>
    static public string GetWord(int index)
    {
        LoadWord();
        return wordList[index];
    }

    /// <summary>
    /// 単語の数を取得
    /// </summary>
    static int GetWordNum()
    {
        return wordList.Count;
    }

    /// <summary>
    /// 単語の読み込み
    /// </summary>
    static void LoadWord()
    {
        if (wordList != null) return;
        //リストを生成
        wordList = new List<string>();
        //単語リストを読み込む
        TextAsset wordAsset = (TextAsset)Resources.Load("WordList");
        StringReader stringReader = new StringReader(wordAsset.text);
        Resources.UnloadAsset(wordAsset);
        //全ての単語をリストに追加する
        while (stringReader.Peek() != -1)
        {
            wordList.Add(stringReader.ReadLine());
        }
    }
}