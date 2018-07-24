﻿using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class DataManager : MonoBehaviour
{

    public static DataManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
        //CreateSampleDialog();
    }

    #region PlayerData_Not_Using
    /*

    //현재 이용중인 플레이어 데이터
    //플레이 중에 데이터 수정이 이루어지고, 저장시 대입되는 데이터이다.
    public static PlayerData Current_Player { get; private set; }

    //    X     프로필을 불러오는 로직에서 임시적으로 사용중이다. 이후에는 필요없음. // 플레이어 정보를 포멧에 맞추어 string으로 반환한다.
    public string LoadCurrentPlayerInfo()
    {
        return string.Format("Player Rank: {0}\nPlayer Name: {1}\nSortie : {2}\nSuccess : {3}",
            Current_Player.rank, Current_Player.player_name, Current_Player.sortie, Current_Player.success);
    }


    // .data 형식을 가지고 있는 파일을 검색해서 파일명의 목록을 List<string> 로 반환한다.
    public List<string> GetPlayerSaves()
    {
        List<string> list = new List<string>();
        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath);
        foreach (var item in di.GetFiles())
        {
            if (item.Extension.ToLower().CompareTo(".data") == 0)
            {
                list.Add(item.Name.Replace(".data", ""));
            }
        }
        return list;
    }

    //{player_name}.data 파일을 생성하고 플레이어 데이터를 초기화해 저장한다.
    public void CreateData(string player_name)
    {
        FileStream stream = new FileStream(string.Format("{0}/{1}.data", Application.persistentDataPath, player_name), FileMode.Create);
        PlayerData data = new PlayerData(player_name);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(stream, data);

        Debug.Log(string.Format("{0}/{1}.data 저장", Application.persistentDataPath, player_name));
        stream.Close();
    }

    //Current_Player 를 {Current_Player . player_name}.data 파일에 저장한다.
    public void SaveData()
    {
        FileStream stream = new FileStream(string.Format("{0}/{1}.data", Application.persistentDataPath, Current_Player.player_name), FileMode.Create);
        PlayerData data = Current_Player;

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(stream, data);

        Debug.Log(string.Format("저장 : {0}/{1}.data", Application.persistentDataPath, Current_Player.player_name));
        stream.Close();
    }

    //{player_name}.data 를 불러와 Current_Player 에 대입한다.
    public void LoadData(string player_name)
    {
        FileStream stream = File.Open(string.Format("{0}/{1}.data", Application.persistentDataPath, player_name), FileMode.Open);

        BinaryFormatter bf = new BinaryFormatter();
        Current_Player = (PlayerData)bf.Deserialize(stream);

        stream.Close();
    }

    */


    /*
    public List<Dialog> LoadDialog(string dialog_index)
    {
        FileStream stream = File.Open(string.Format("{0}/{1}.txt", Application.dataPath + "/Resources/Dialogs/", dialog_index), FileMode.Open);
        StreamReader sr = new StreamReader(stream, System.Text.Encoding.Default);
        string[] dialogdata = sr.ReadToEnd().Split('/');
        stream.Close();

        List<Dialog> DialogScene = new List<Dialog>();
        for (int i = 0; i< dialogdata.Length; i++)
        {
            DialogScene.Add(new Dialog());
        }

        return DialogScene;
    }
    */

    #endregion /

    #region CreateSampleData
    public void CreateSampleDialog()
    {
        List<Dialog> script = new List<Dialog> {
            new Dialog("사람A", "첫번째 대사야.", new Illust[] { new Illust("A", IllistPos.Left, IllustMode.Front), new Illust("B", IllistPos.Right, IllustMode.Back) }),
            new Dialog("사람B", "두번째 대사는 이거.", new Illust[] { new Illust("A", IllistPos.Left, IllustMode.Back), new Illust("B", IllistPos.Right, IllustMode.Front) }),
            new Dialog("사람A", "이게 마지막 대사.", new Illust[] { new Illust("A", IllistPos.Left, IllustMode.Front), new Illust("B", IllistPos.Right, IllustMode.Back) })
        };

        using (StreamWriter file = File.CreateText(string.Format("{0}/{1}.json", "Assets/Resources/Datas/Dialogs/", "Test")))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(file, script);
        }
    }
    #endregion CreateSampleData

    public void SavePlayerData()
    {

    }

    public void LoadPlayerData(string player_name)
    {
        using (StreamReader file = File.OpenText(string.Format("{0}/PlayerData/{1}", Application.persistentDataPath, player_name)))
        {

        }
    }

    public static List<Dialog> LoadDialog(string dialog_name)
    {
        using (StreamReader file = new StreamReader(new MemoryStream(Resources.Load<TextAsset>(string.Format("Datas/Dialogs/{0}", dialog_name)).bytes), System.Text.Encoding.UTF8))
        {
            JsonSerializer serializer = new JsonSerializer();
            List<Dialog> script = (List<Dialog>)serializer.Deserialize(file, typeof(List<Dialog>));
            return script;
        }
    }
}

public class PlayerData
{
    public string player_id;
    public string player_name;

    //재화
    public int unicoin;
    public int cosmoston;

}

#region AlchemyData
public class Meterial
{
    public string item_id;
    public string item_name;
    public string discription;
    //더할 수 있는 재료의 코드를 저장하는 리스트
    public List<string> combinable;
}

public class Formula
{
    public List<string[]> formula;
}
#endregion AlchemyData

#region ShopData
public enum CurrencyType
{
    Unicoin = 0, Cosmoston
}

public struct ShopItem
{
    public string item_id;
    public string item_name;
    public string discription;
    public CurrencyType currencyType;
    public int price;
    public string image_address;
}
#endregion ShopData

#region DialogData
public class Dialog
{
    public string name;
    public string content;
    public Illust[] illusts = new Illust[2];

    public Dialog(string name, string content, Illust[] illusts)
    {
        this.name = name;
        this.content = content;
        this.illusts = illusts;
    }
}

public enum IllistPos { Left = 0, Center, Right }
public enum IllustMode { Front = 0, Back }
public struct Illust
{
    public string name;
    public IllistPos pos;
    public IllustMode mode;

    public Illust(string name, IllistPos pos, IllustMode mode)
    {
        this.name = name;
        this.pos = pos;
        this.mode = mode;
    }
}
#endregion DialogData