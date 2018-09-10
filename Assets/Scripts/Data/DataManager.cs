﻿using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace AlchemyPlanet.Data
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance { get; private set; }

        public PlayerData CurrentPlayerData { get; set; }

        //아이템 데이터 프리로드
        public Dictionary<string, Material> materials;
        public Dictionary<string, Structure> structures;

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
            //CreateSampleStructure(); // 타운관리모드 테스트할때 이거 주석 풀기

            LoadMaterials();
            LoadStuructures();
            LoadPlayerData();
        }

        #region CreateSampleData
        public void CreateSampleDialog()
        {
            List<Dialog> script = new List<Dialog> {
            new Dialog("시드", "첫번째 대사야.", new Illust[] { new Illust("시드", IllistPos.Left, IllustMode.Front), new Illust("마칭냥", IllistPos.Right, IllustMode.Back) }),
            new Dialog("마칭냥", "두번째 대사는 이거.", new Illust[] { new Illust("시드", IllistPos.Left, IllustMode.Back), new Illust("마칭냥", IllistPos.Right, IllustMode.Front) }),
            new Dialog("시드", "이게 마지막 대사.", new Illust[] { new Illust("시드", IllistPos.Left, IllustMode.Front), new Illust("마칭냥", IllistPos.Right, IllustMode.Back) })
        };

            using (StreamWriter file = File.CreateText(string.Format("{0}/{1}.json", "Assets/Resources/Datas/Dialogs/", "Sample")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, script);
            }
        }

        public void CreateSampleMaterials()
        {
            Dictionary<string, Material> materials = new Dictionary<string, Material> {
            {"Red", new Material("Red", "빨강")},
            {"Blue", new Material("Blue", "파랑")},
            {"Yellow", new Material("Yellow", "노랑")},
            {"Perple", new Material("Perple", "보라")}
        };

            using (StreamWriter file = File.CreateText("Assets/Resources/Datas/Materials.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, materials);
            }
        }

        public void CreateSampleFomulas()
        {
            List<Formula> formulas = new List<Formula> {
            new Formula(new Dictionary<string, int>{ {"A002",3 }, {"A003", 3} }, "A004", 1),
            new Formula(new Dictionary<string, int>{ {"A001",2 }, {"A003", 2} }, "A002", 1)
        };

            using (StreamWriter file = File.CreateText("Assets/Resources/Datas/Formulas.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, formulas);
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
                Dictionary<string, Interior> Interiors = new Dictionary<string, Interior> { { "Tree", new Interior("Tree") } };
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
                Dictionary<string, Material> materials = (Dictionary<string, Material>)serializer.Deserialize(file, typeof(Dictionary<string, Material>));

                Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Items/UI");


                for (int i = 0; i < sprites.Length; i++)
                {
                    if (materials.ContainsKey(sprites[i].name))
                    {
                        if (sprites[i].name == materials[sprites[i].name].item_name)
                            materials[sprites[i].name].image = sprites[i];
                    }
                }

                this.materials = materials;
            }
        }

        private void LoadStuructures()
        {
            structures = new Dictionary<string, Structure>();
            using (StreamReader file = new StreamReader(new MemoryStream(Resources.Load<TextAsset>("Datas/Buildings").bytes), System.Text.Encoding.UTF8))
            {
                JsonSerializer serializer = new JsonSerializer();
                Dictionary<string, Building> Buildings = (Dictionary<string, Building>)serializer.Deserialize(file, typeof(Dictionary<string, Building>));

                foreach(string str in Buildings.Keys)
                {
                    this.structures.Add(str, new Building(Buildings[str].structureName, Buildings[str].buildingDiscription, Buildings[str].buildingLevel));
                }

                Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Town/town");

                for (int i = 0; i < sprites.Length; i++)
                {
                    if (structures.ContainsKey(sprites[i].name))
                    {
                        if (sprites[i].name == structures[sprites[i].name].structureName)
                            structures[sprites[i].name].image = sprites[i];
                    }
                }
                foreach (string str in Buildings.Keys)
                {
                    structures[str].StructureObject = Resources.Load<GameObject>("Prefabs/TownScene/" + str);
                }
            }
            using (StreamReader file = new StreamReader(new MemoryStream(Resources.Load<TextAsset>("Datas/Interiors").bytes), System.Text.Encoding.UTF8))
            {
                JsonSerializer serializer = new JsonSerializer();
                Dictionary<string, Interior> Interiors = (Dictionary<string, Interior>)serializer.Deserialize(file, typeof(Dictionary<string, Interior>));

                foreach (string str in Interiors.Keys)
                {
                    this.structures.Add(str, new Interior(Interiors[str].structureName));
                }

                Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Town/town");

                for (int i = 0; i < sprites.Length; i++)
                {
                    if (structures.ContainsKey(sprites[i].name))
                    {
                        if (sprites[i].name == structures[sprites[i].name].structureName)
                            structures[sprites[i].name].image = sprites[i];
                    }
                }
                foreach (string str in Interiors.Keys)
                {
                    structures[str].StructureObject = Resources.Load<GameObject>("Prefabs/TownScene/" + str);
                }
            }
        }

        #region PlayerData
        public static PlayerData PlayerStringToData(string dataString)
        {
            PlayerData data = JsonConvert.DeserializeObject<PlayerData>(dataString);
            return data;
        }

        public string PlayerDataToString(PlayerData data)
        {
            string dataString = JsonConvert.SerializeObject(data);
            return dataString;
        }

        public string PlayerDataToString()
        {
            string dataString = JsonConvert.SerializeObject(Instance.CurrentPlayerData);
            return dataString;
        }

        public void LoadPlayerData()
        {
            CurrentPlayerData = new PlayerData();


        }
        #endregion PlayerData

        #region Dialog
        public List<Dialog> LoadDialog(string dialog_name)
        {
            using (StreamReader file = new StreamReader(new MemoryStream(Resources.Load<TextAsset>(string.Format("Datas/Dialogs/{0}", dialog_name)).bytes), System.Text.Encoding.UTF8))
            {
                JsonSerializer serializer = new JsonSerializer();
                List<Dialog> script = (List<Dialog>)serializer.Deserialize(file, typeof(List<Dialog>));
                return script;
            }
        }

        public Dictionary<string, Sprite> LoadIllust(List<Dialog> dialogs)
        {
            Dictionary<string, Sprite> illusts = new Dictionary<string, Sprite>();

            foreach (Dialog d in dialogs)
            {
                if (!illusts.ContainsKey(d.illusts[0].name))
                    illusts.Add(d.illusts[0].name, Resources.Load<Sprite>(string.Format("Sprites/Illusts/{0}", d.illusts[0].name)));

                if (!illusts.ContainsKey(d.illusts[1].name))
                    illusts.Add(d.illusts[1].name, Resources.Load<Sprite>(string.Format("Sprites/Illusts/{0}", d.illusts[1].name)));
            }
            return illusts;
        }
        #endregion Dialog

        public List<Formula> LoadFormulas()
        {
            using (StreamReader file = new StreamReader(new MemoryStream(Resources.Load<TextAsset>("Datas/Formulas").bytes), System.Text.Encoding.UTF8))
            {
                JsonSerializer serializer = new JsonSerializer();
                List<Formula> formulas = (List<Formula>)serializer.Deserialize(file, typeof(List<Formula>));
                return formulas;
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
        public int oxygentank;
        public int Max_oxygentank;

        //재료
        public Dictionary<string, int> inventory;

        public List<Structure> structures;
        public List<GameObject> setupBuildilngs;

        public PlayerData()
        {
            this.player_id = Social.localUser.id;
            this.player_name = "Popin";
            this.unicoin = 0;
            this.cosmoston = 0;
            this.oxygentank = 10;
            this.Max_oxygentank = 10;

            this.inventory = new Dictionary<string, int>();
            this.structures = new List<Structure>();
            structures.Add(DataManager.Instance.structures["Tree"].Clone());
            structures[0].setup = true;
            structures.Add(DataManager.Instance.structures["House"].Clone());
            structures[1].setup = true;
            structures.Add(DataManager.Instance.structures["Tree"].Clone());
            structures[2].setup = false;

            inventory.Add("Red", 1);
            inventory.Add("Blue", 2);
        }

        public void Upgrade()
        {
            
        }
    }

    public class NPCDAta
    {
        public string npc_name;
        public List<Dialog> dialogs;
        public Dictionary<string, Sprite> illusts;

        public NPCDAta(string npc_name)
        {
            this.npc_name = npc_name;
            //dialogs = DataManager.Instance.LoadDialog("Sample");
            dialogs = DataManager.Instance.LoadDialog(this.npc_name);
            illusts = DataManager.Instance.LoadIllust(dialogs);
        }
    }

    public class Structure
    {
        public string structureName;
        public Sprite image;
        public GameObject StructureObject;
        public Vector2 position;
        public bool setup;

        public virtual Structure Clone()
        {
            Structure strc = new Structure();
            strc.structureName = structureName;
            strc.image = image;
            strc.StructureObject = StructureObject;
            strc.setup = setup;
            return strc;
        }
    }

    public class Building : Structure
    {
        public string buildingDiscription;
        public int buildingLevel;

        public Building(string buildingName, string buildingDiscription, int buildingLevel)
        {
            this.structureName = buildingName;
            this.buildingDiscription = buildingDiscription;
            this.buildingLevel = buildingLevel;
        }
        public override Structure Clone()
        {
            Structure strc = new Building(structureName,buildingDiscription, buildingLevel);
            strc.image = image;
            strc.StructureObject = StructureObject;
            strc.setup = setup;
            return strc;
        }
    }

    public class Interior : Structure
    {
        public Interior(string interiorName)
        {
            this.structureName = interiorName;
        }
        public override Structure Clone()
        {
            Structure strc = new Interior(structureName);
            strc.image = image;
            strc.StructureObject = StructureObject;
            strc.setup = setup;
            return strc;
        }
    }

    #region AlchemyData
    public class Material
    {
        public string item_name;
        public string discription;
        public Sprite image;

        public Material(string item_name, string discription)
        {
            this.item_name = item_name;
            this.discription = discription;
        }
    }

    public class Formula
    {
        public Dictionary<string, int> formula;
        public string result;
        public int resultcount;

        public Formula(Dictionary<string, int> formula, string result, int resultcount)
        {
            this.formula = formula;
            this.result = result;
            this.resultcount = resultcount;
        }
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
}