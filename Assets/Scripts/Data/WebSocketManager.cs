﻿using AlchemyPlanet.TownScene;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

namespace AlchemyPlanet.Data
{
    public class WebSocketManager : MonoBehaviour
    {
        public static WebSocketManager Instance { get; private set; }
        public WebSocket ws;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        private void Start()
        {
            ws = new WebSocket("ws://54.180.47.195:3000/");

            ws.OnMessage += (sender, e) =>
            {
                Debug.Log(e.Data);
                var message = JsonConvert.DeserializeObject<Message>(e.Data);
                
                if (message.status == "StructureUpgradeInfo")
                {
                    Debug.Log(message.data);
                    string data_string = ConvertLappedJsonString(message.data);
                    var data = JsonConvert.DeserializeObject<StructureUpgradeInfo>(data_string);
                    Debug.Log("TEST_TEST_TEST_TEST_TEST");
                    OnMessageGetStructureUpgradeInfo(data);
                }
            };

            ws.Connect();
            SendGetStructureUpgradeInfo(0, 1);
        }

        private void OnDestroy()
        {
            Instance = null;
            ws.Close();
        }

        private string ConvertLappedJsonString(string Jsonstring)
        {
            string BackSlashDeleted = Jsonstring.Replace("\\\"", "\"");
            return BackSlashDeleted.Remove(0, 1).Remove(BackSlashDeleted.Length - 2);
        }

        private void SendGetStructureUpgradeInfo(int uid, int sid)
        {
            var data = new StructureUpgradeInfo(uid, sid, new DateTime(), 0);
            var message = new Message("GetStructureUpgradeInfo", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }

        private void OnMessageGetStructureUpgradeInfo(StructureUpgradeInfo data)
        {
            Debug.Log(data);
        }

        private void SendSetStructureUpgradeInfo(int uid, int sid, DateTime sDate, int rTime)
        {
            var data = new StructureUpgradeInfo(uid, sid, sDate, rTime);
            var message = new Message("SetStructureUpgradeInfo", data);
            var str = JsonConvert.SerializeObject(message);
            ws.Send(str);
        }
    }
}