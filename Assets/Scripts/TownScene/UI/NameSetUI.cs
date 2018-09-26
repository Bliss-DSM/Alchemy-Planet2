﻿using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.TownScene
{
    public class NameSetUI : Common.UI<NameSetUI>
    {
        [SerializeField] private InputField PlayerNameInput;
        [SerializeField] private Button CommitButton;

        protected override void Awake()
        {
            base.Awake();
            CommitButton.onClick.AddListener(() => {
                Data.DataManager.Instance.CurrentPlayerData.player_name = PlayerNameInput.text;
                Data.WebSocketManager.Instance.SendInsertName("0", Data.DataManager.Instance.CurrentPlayerData.player_id,
                    Data.DataManager.Instance.CurrentPlayerData.player_name);
                UIManager.Instance.CloseMenu();
            });
        }

    }

}