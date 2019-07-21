﻿using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using LitJson;
using System.Collections;
using System;

namespace AlchemyPlanet.Data
{
    public enum CharacterEnum { Popin = 1 }

    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance { get; private set; }

        public PlayerData CurrentPlayerData { get; set; }

        //데이터 프리로드
        public Dictionary<string, ItemData> itemInfo;
        public Dictionary<string, Building> buildingInfo;
        public Dictionary<string, Interior> interiorInfo;
        //public Dictionary<string, Request> RequestInfo;

        public int selected_stage = 0;
        public NPCDAta selected_dialog;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(this.gameObject);
            }

            //CreateSampleDialog();
            //CreateSampleMaterials();
            //CreateSampleFomulas();
            //CreateSampleRequest();
            //CreateSampleStructure();
            
            LoadMaterials();
            LoadStuructures();
        }

        private void Start()
        {

        }

        #region CreateSampleData
        public void CreateSampleDialog()
        {
            List<DialogData> script = new List<DialogData> {
            new DialogData("시드", "첫번째 대사야.", new Illust[] { new Illust("시드", IllistPos.Left, IllustMode.Front), new Illust("마칭냥", IllistPos.Right, IllustMode.Back) }),
            new DialogData("마칭냥", "두번째 대사는 이거.", new Illust[] { new Illust("시드", IllistPos.Left, IllustMode.Back), new Illust("마칭냥", IllistPos.Right, IllustMode.Front) }),
            new DialogData("시드", "이게 마지막 대사.", new Illust[] { new Illust("시드", IllistPos.Left, IllustMode.Front), new Illust("마칭냥", IllistPos.Right, IllustMode.Back) })
        };

            using (StreamWriter file = File.CreateText(string.Format("{0}/{1}.json", "Assets/Resources/Datas/Dialogs/", "Sample")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, script);
            }
        }

        public void CreateSampleMaterials()
        {
            Dictionary<string, ItemData> materials = new Dictionary<string, ItemData> {
            {"파릇한 새싹", new ItemData("파릇한 새싹",ItemKind.Material , "파릇파릇한 연두색 새싹.\n언젠가 큰 나무가 될 수 있을거라 믿으며\n자라나고있다.")},
            {"붉은 꽃잎", new ItemData("붉은 꽃잎",ItemKind.Material, "붉은색 꽃에서 떨어져나온 꽃잎.\n향기가 좋아 방향제로 자주 쓰인다.")},
            {"작은 오렌지", new ItemData("작은 오렌지",ItemKind.Material, "새콤한 맛이 나는 작은 오렌지.\n과즙이 팡팡.")},
            {"블루베리", new ItemData("블루베리",ItemKind.Material, "새콤달콤한 블루베리.\n톡톡터지는 식감이 좋다. ")},
            {"라벤더", new ItemData("라벤더",ItemKind.Material, "향기로운 라벤더.\n방향제는 물론 장식용으로도 쓰인다.")},
            {"정화약", new ItemData("정화약",ItemKind.PurifyPotion, "뭐라고 적지...")}
        };

            using (StreamWriter file = File.CreateText("Assets/Resources/Datas/Materials.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, materials);
            }
        }

        public void CreateSampleFomulas()
        {
            List<FormulaData> formulas = new List<FormulaData> {
                new FormulaData(new Dictionary<string, int>{ { "작은 오렌지", 3 }, { "블루베리", 3} }, "파릇한 새싹", 1),
                new FormulaData(new Dictionary<string, int>{ { "붉은 꽃잎", 2 }, { "블루베리", 2} }, "라벤더", 1),
                new FormulaData(new Dictionary<string, int>{ { "파릇한 새싹", 2 }, { "라벤더", 2} }, "정화약", 1)
            };

            using (StreamWriter file = File.CreateText("Assets/Resources/Datas/Formulas.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, formulas);
            }
        }

        public void CreateSampleRequest()
        {
            List<Request> requests = new List<Request> {
                new Request(new Dictionary<string, int>{ { "작은 오렌지", 3 }, { "블루베리", 2} }, "변비약이 필요해요", 12000, 6, 300),
                new Request(new Dictionary<string, int>{ { "붉은 꽃잎", 3 }, { "파릇한 새싹", 3} }, "감기약이 필요해요", 11000, 6, 300),
                new Request(new Dictionary<string, int>{ { "라벤더", 3 }, { "정화약", 1} }, "위장약이 필요해요", 12000, 4, 300),
                new Request(new Dictionary<string, int>{ { "파릇한 새싹", 3 }, { "정화약", 1} }, "아무튼약이 필요해요", 12000, 6, 200)
            };

            using (StreamWriter file = File.CreateText("Assets/Resources/Datas/Requests.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, requests);
            }
        }

        public void CreateSampleStructure()
        {

            using (StreamWriter file = File.CreateText("Assets/Resources/Datas/Buildings.json"))
            {
                Dictionary<string, Building> Buildings = new Dictionary<string, Building> { { "House", new Building("House", "집", 1) } };
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Buildings);
            }
            using (StreamWriter file = File.CreateText("Assets/Resources/Datas/Interiors.json"))
            {
                Dictionary<string, Interior> Interiors = new Dictionary<string, Interior> { { "Tree", new Interior("Tree","나무") } };
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Interiors);
            }
        }

        #endregion CreateSampleData

        private void LoadMaterials()
        {
            using (StreamReader file = new StreamReader(new MemoryStream(Resources.Load<TextAsset>("Datas/Materials").bytes), System.Text.Encoding.UTF8))
            {
                JsonSerializer serializer = new JsonSerializer();
                Dictionary<string, ItemData> materials = (Dictionary<string, ItemData>)serializer.Deserialize(file, typeof(Dictionary<string, ItemData>));

                Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Items/Items/");


                for (int i = 0; i < sprites.Length; i++)
                {
                    if (materials.ContainsKey(sprites[i].name))
                    {
                        materials[sprites[i].name].image = sprites[i];
                    }
                }

                this.itemInfo = materials;
            }
        }

        private void LoadStuructures()
        {
            buildingInfo = new Dictionary<string, Building>();
            interiorInfo = new Dictionary<string, Interior>();
            using (StreamReader file = new StreamReader(new MemoryStream(Resources.Load<TextAsset>("Datas/Buildings").bytes), System.Text.Encoding.UTF8))
            {
                JsonSerializer serializer = new JsonSerializer();
                Dictionary<string, Building> Buildings = (Dictionary<string, Building>)serializer.Deserialize(file, typeof(Dictionary<string, Building>));

                foreach (string str in Buildings.Keys)
                {
                    this.buildingInfo.Add(str, Buildings[str].Clone());
                    buildingInfo[str].buildingObject = Resources.Load<GameObject>("Prefabs/TownScene/Structure");
                }

                Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/TownScene/타운리소스");

                for (int i = 0; i < sprites.Length; i++)
                {
                    if (buildingInfo.ContainsKey(sprites[i].name))
                    {
                        buildingInfo[sprites[i].name].image = sprites[i];
                    }
                }
            }
            using (StreamReader file = new StreamReader(new MemoryStream(Resources.Load<TextAsset>("Datas/Interiors").bytes), System.Text.Encoding.UTF8))
            {
                JsonSerializer serializer = new JsonSerializer();
                Dictionary<string, Interior> Interiors = (Dictionary<string, Interior>)serializer.Deserialize(file, typeof(Dictionary<string, Interior>));

                foreach (string str in Interiors.Keys)
                {
                    this.interiorInfo.Add(str, Interiors[str].Clone());
                    interiorInfo[str].interiorObject = Resources.Load<GameObject>("Prefabs/TownScene/Structure");
                }

                Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/TownScene/타운리소스");

                for (int i = 0; i < sprites.Length; i++)
                {
                    if (interiorInfo.ContainsKey(sprites[i].name))
                    {
                        interiorInfo[sprites[i].name].image = sprites[i];
                    }
                }
            }
        }

        #region PlayerData

        public string CreatePlayerId()
        {
            char[] array = new char[8];

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Convert.ToChar(UnityEngine.Random.Range(0, 10) + '0');
            }

            return array.ToString();
        }

        public void LoadPlayerData()
        {
            CurrentPlayerData = new PlayerData();

            BackendManager.Instance.UpdatePlayerUniCoin(BackendManager.Instance.GetInDate("player"), 10000);

            CurrentPlayerData.unicoin = int.Parse(BackendManager.Instance.GetContent("player","uniCoin","N"));
            CurrentPlayerData.cosmostone = int.Parse(BackendManager.Instance.GetContent("player","cosmoStone","N"));
            CurrentPlayerData.exp = int.Parse(BackendManager.Instance.GetContent("player","exp","N"));
            CurrentPlayerData.level = int.Parse(BackendManager.Instance.GetContent("player","level","N"));
            CurrentPlayerData.oxygentank = int.Parse(BackendManager.Instance.GetContent("player","oxygenTank","N"));
            CurrentPlayerData.player_name = BackendManager.Instance.GetContent("player","nickname","S");
            CurrentPlayerData.player_id = BackendManager.Instance.GetContent("player","playerid","S");

            JsonData itemList = BackEnd.Backend.GameInfo.GetContentsByIndate("item", BackendManager.Instance.GetInDate("item")).GetReturnValuetoJSON()["row"][0]["items"]["L"];
            for (int i = 0; i < itemList.Count; i++)
            {
                string name =itemList[i]["M"]["itemName"]["S"].ToString();
                int num = int.Parse(itemList[i]["M"]["number"]["N"].ToString());
                CurrentPlayerData.inventory.Add(name, num);
            }
        }
        
        public void CommitName(CollectionName data)
        {
            CurrentPlayerData.player_name = data.playerName;
        }

        public void CommitLevel(CollectionLevel data)
        {
            CurrentPlayerData.level = data.level;
            CurrentPlayerData.exp = data.exp;

            if (Common.StateBar.Instance != null)
                Common.StateBar.Instance.UpdateState();
        }

        public void CommitGoods(CollectionGoods data)
        {
            CurrentPlayerData.unicoin = data.uniCoin;
            CurrentPlayerData.cosmostone = data.cosmoStone;
            CurrentPlayerData.oxygentank = data.oxygenTank;

            if(Common.StateBar.Instance != null)
                Common.StateBar.Instance.UpdateState();
        }

        public void CommitItem(CollectionItem[] data)
        {
            foreach (var item in data)
            {
                if(CurrentPlayerData.inventory.ContainsKey(item.itemId))
                    CurrentPlayerData.inventory[item.itemId] = item.number;
                else
                    CurrentPlayerData.inventory.Add(item.itemId, item.number);
            }
        }

        public void CommitBuilding(CollectionBuilding[] data)
        {
            foreach (var item in data)
            {
                Building building = new Building(item.buildingId, "", item.level);
                building.buildingDiscription = buildingInfo[building.buildingName].buildingDiscription;
                building.effect = buildingInfo[building.buildingName].effect;
                building.id = int.Parse(item.playerBuildingId);
                building.image = buildingInfo[building.buildingName].image;
                building.position = item.position;
                building.flip = item.isFlipped;
                building.setup = item.isConstructed;
                building.upgrading = item.isUpgrading;
                building.endDate = item.endDate;
                building.material = buildingInfo[building.buildingName].material;
                building.money = buildingInfo[building.buildingName].money;
                CurrentPlayerData.buildings.Add(building);
            }
        }

        public void CommitInterior(CollectionInterior[] data)
        {
            foreach (var item in data)
            {
                Interior interior = new Interior(item.interiorId, "");
                interior.interiorDiscription = interiorInfo[interior.interiorName].interiorDiscription;
                interior.id = int.Parse(item.playerInteriorId);
                interior.image = interiorInfo[interior.interiorName].image;
                interior.position = item.position;
                interior.flip = item.isFlipped;
                interior.setup = item.isConstructed;
                interior.material = interiorInfo[interior.interiorName].material;
                interior.money = interiorInfo[interior.interiorName].money;
                CurrentPlayerData.interiors.Add(interior);
            }

        }

        public void CommitCharacter(CollectionCharacter[] data)
        {
            foreach (var item in data)
                CurrentPlayerData.characters.Add(new Character((CharacterEnum)Int32.Parse(item.characterId), item.level, item.health, item.speed, item.attackPower, ""));
        }

        public void CommitParty(CollectionParty[] data)
        {
            foreach (var item in data)
                CurrentPlayerData.party[item.partyIndex - 1, item.slotIndex - 1] = (CharacterEnum)int.Parse(item.characterId);
        }

        public void CommitRequest(CollectionRequest[] data)
        {
            //
        }

        public void CommitStoryStar(CollectionStroyStar[] data)
        {
            foreach (var item in data)
                CurrentPlayerData.stroystar.Add(item.stageNumber, item.number);
        }
        
        #endregion PlayerData

        #region Dialog
        public List<DialogData> LoadDialog(string dialog_name)
        {
            using (StreamReader file = new StreamReader(new MemoryStream(Resources.Load<TextAsset>(string.Format("Datas/Dialogs/{0}", dialog_name)).bytes), System.Text.Encoding.UTF8))
            {
                JsonSerializer serializer = new JsonSerializer();
                List<DialogData> script = (List<DialogData>)serializer.Deserialize(file, typeof(List<DialogData>));
                return script;
            }
        }

        public Dictionary<string, Sprite> LoadIllust(List<DialogData> dialogs)
        {
            Dictionary<string, Sprite> illusts = new Dictionary<string, Sprite>();

            foreach (DialogData d in dialogs)
            {
                if (!illusts.ContainsKey(d.illusts[0].name))
                    illusts.Add(d.illusts[0].name, Resources.Load<Sprite>(string.Format("Sprites/Illusts/{0}", d.illusts[0].name)));

                if (!illusts.ContainsKey(d.illusts[1].name))
                    illusts.Add(d.illusts[1].name, Resources.Load<Sprite>(string.Format("Sprites/Illusts/{0}", d.illusts[1].name)));
            }
            return illusts;
        }
        #endregion Dialog

        public List<FormulaData> LoadFormulas()
        {
            using (StreamReader file = new StreamReader(new MemoryStream(Resources.Load<TextAsset>("Datas/Formulas").bytes), System.Text.Encoding.UTF8))
            {
                JsonSerializer serializer = new JsonSerializer();
                List<FormulaData> formulas = (List<FormulaData>)serializer.Deserialize(file, typeof(List<FormulaData>));
                return formulas;
            }
        }

        public List<Request> LoadRequests()
        {
            using (StreamReader file = new StreamReader(new MemoryStream(Resources.Load<TextAsset>("Datas/Requests").bytes), System.Text.Encoding.UTF8))
            {
                JsonSerializer serializer = new JsonSerializer();
                List<Request> requests = (List<Request>)serializer.Deserialize(file, typeof(List<Request>));
                return requests;
            }
        }

        public List<StoryChallengeData> LoadStoryChallenges()
        {
            using (StreamReader file = new StreamReader(new MemoryStream(Resources.Load<TextAsset>("Datas/StoryChallenges").bytes), System.Text.Encoding.UTF8))
            {
                JsonSerializer serializer = new JsonSerializer();
                List<StoryChallengeData> storyChallenges = (List<StoryChallengeData>)serializer.Deserialize(file, typeof(List<StoryChallengeData>));
                return storyChallenges;
            }
        }
    }
    
    public class NPCDAta
    {
        public string npc_name;
        public List<DialogData> dialogs;
        public Dictionary<string, Sprite> illusts;

        public NPCDAta(string npc_name)
        {
            this.npc_name = npc_name;
            //dialogs = DataManager.Instance.LoadDialog("Sample");
            dialogs = DataManager.Instance.LoadDialog(this.npc_name);
            illusts = DataManager.Instance.LoadIllust(dialogs);
        }
    }

    public class Character
    {
        public CharacterEnum name;
        public int level;
        public int addtional_health;
        public int speed;
        public int atk;
        public string leader_skill_info;

        public Character(CharacterEnum name, int level, int addtional_health, int speed, int atk, string leader_skill_info)
        {
            this.name = name;
            this.level = level;
            this.addtional_health = addtional_health;
            this.speed = speed;
            this.atk = atk;
            this.leader_skill_info = leader_skill_info;
        }
    }

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

    #region StoryData

    public class StoryChallengeData
    {
        public string stage;
        public string[] challenges;

        public StoryChallengeData(string stage, string challenge1, string challenge2, string challenge3)
        {
            this.stage = stage;
            challenges = new string[3];
            challenges[0] = challenge1;
            challenges[1] = challenge2;
            challenges[2] = challenge3;
        }
    }

    #endregion StoryData
}