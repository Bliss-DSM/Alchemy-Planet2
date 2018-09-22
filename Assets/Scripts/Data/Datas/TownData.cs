﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.Data
{
    public class Structure
    {
        public int id;
        public string structureName;
        public Sprite image;
        public GameObject StructureObject;
        public float position;
        public bool setup = false;
        public bool flip = false;

        public virtual Structure Clone()
        {
            Structure strc = new Structure();
            strc.id = id;
            strc.structureName = structureName;
            strc.image = image;
            strc.StructureObject = StructureObject;
            strc.position = position;
            strc.setup = setup;
            return strc;
        }
        public void Build()
        {
            StructureObject.GetComponent<SpriteRenderer>().sprite = image;
            StructureObject.transform.position = new Vector2(position, image.bounds.size.y / 2 - 1);
            if (StructureObject.GetComponent<PolygonCollider2D>())
                GameObject.DestroyImmediate(StructureObject.GetComponent<PolygonCollider2D>(), true);
            StructureObject.AddComponent<PolygonCollider2D>();
            StructureObject.GetComponent<PolygonCollider2D>().isTrigger = true;
            StructureObject.GetComponent<SpriteRenderer>().sortingOrder = -90;
            StructureObject.name = id.ToString();
            StructureObject.GetComponent<SpriteRenderer>().flipX = flip;
        }
    }

    public class Building : Structure
    {
        public string buildingDiscription;
        public int buildingLevel;
        public bool upgrading = false;
        public string material1Name;
        public string material2Name;
        public int material1Count;
        public int material2Count;
        public long UpgradeEndTime;

        public Building(string buildingName, string buildingDiscription, int buildingLevel)
        {
            this.structureName = buildingName;
            this.buildingDiscription = buildingDiscription;
            this.buildingLevel = buildingLevel;
        }
        public override Structure Clone()
        {
            Building strc = new Building(structureName, buildingDiscription, buildingLevel);
            strc.image = image;
            strc.StructureObject = StructureObject;
            strc.position = position;
            strc.setup = setup;
            strc.material1Name = material1Name;
            strc.material2Name = material2Name;
            strc.material1Count = material1Count;
            strc.material2Count = material2Count;
            return strc;
        }
        public void UpgradeStart()
        {
            upgrading = true;
            image = Resources.Load<Sprite>("Sprites/Town/Tent");
            DataManager.Instance.CurrentPlayerData.SetBuilding(this);
            AlchemyPlanet.TownScene.BuildingManagement.Instance.SendMessage("SetImage");
            int UpgradeTime = buildingLevel * 10;
            UpgradeEndTime = DateTime.Now.AddSeconds(UpgradeTime).ToBinary();
            WebSocketManager.Instance.SendUpdateBuilding("", DataManager.Instance.CurrentPlayerData.player_id, id.ToString(), structureName, buildingLevel, position, setup, flip, upgrading, UpgradeEndTime);
        }

        public void UpgradeEnd()
        {
            buildingLevel++;
            image = Resources.Load<Sprite>("Sprites/Town/" + structureName + buildingLevel.ToString());
            upgrading = false;
            DataManager.Instance.CurrentPlayerData.SetBuilding(this);

            Debug.Log(AlchemyPlanet.TownScene.BuildingManagement.Instance);

            if(AlchemyPlanet.TownScene.BuildingManagement.Instance != null)
                AlchemyPlanet.TownScene.BuildingManagement.Instance.SendMessage("SetImage");

            WebSocketManager.Instance.SendUpdateBuilding("", DataManager.Instance.CurrentPlayerData.player_id, id.ToString(), structureName, buildingLevel, position, setup, flip, upgrading, UpgradeEndTime);
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
            strc.position = position;
            strc.setup = setup;
            return strc;
        }
    }
}