﻿using AlchemyPlanet.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.CharacterScene
{
    public class OrganizeUI : MonoBehaviour
    {
        public static OrganizeUI Instance { get; private set; }

        public Button leftButton;
        public Button rightButton;
        public PageLights partyBoxPageLights;
        public PageLights characterListPageLights;
        public Button okayButton;
        public Button backButton;
        public GameObject characterListCells;
        public GameObject characterListCellImage;
        public GameObject partyBoxCharacter1;
        public GameObject partyBoxCharacter2;
        public GameObject partyBoxCharacter3;
        public List<CharacterEnum> characterSpritesKey;
        public List<Sprite> characterSpritesValue;

        private List<GameObject> partyBoxCharacterObjects = new List<GameObject>();

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
            leftButton.onClick.AddListener(OnClickLeftButton);
            rightButton.onClick.AddListener(OnClickRightButton);
            okayButton.onClick.AddListener(OnClickOkayButton);
            backButton.onClick.AddListener(OnClickBackButton);

            LoadCharacterSprite(0);
            LoadParty();
        }

        private void OnClickLeftButton()
        {
            partyBoxPageLights.ChangeLightToLeft();

            if (GameManager.Instance.PartyIndex == 1) GameManager.Instance.PartyIndex = 9;
            else GameManager.Instance.PartyIndex--;

            foreach (var item in partyBoxCharacterObjects)
                Destroy(item);

            partyBoxCharacter1.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "";
            partyBoxCharacter2.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "";
            partyBoxCharacter3.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "";

            LoadParty();
        }

        private void OnClickRightButton()
        {
            partyBoxPageLights.ChangeLightToRight();

            if (GameManager.Instance.PartyIndex == 9) GameManager.Instance.PartyIndex = 1;
            else GameManager.Instance.PartyIndex++;

            foreach (var item in partyBoxCharacterObjects)
                Destroy(item);

            partyBoxCharacter1.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "";
            partyBoxCharacter2.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "";
            partyBoxCharacter3.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "";

            LoadParty();
        }

        private void OnClickOkayButton()
        {
            GameManager.Instance.CurrentCharacters = GameManager.Instance.InstantiateCharacters();

            UIManager.Instance.DestroyUI();
            UIManager.Instance.CreateMainUI();
        }

        private void OnClickBackButton()
        {
            GameManager.Instance.CurrentCharacters = GameManager.Instance.InstantiateCharacters();

            UIManager.Instance.DestroyUI();
            UIManager.Instance.CreateMainUI();
        }

        private void LoadCharacterSprite(int page)
        {
            int startIndex = page * 15;
            int endIndex = startIndex + 14;
            endIndex = Mathf.Min(endIndex, DataManager.Instance.CurrentPlayerData.characters.Count);

            for(int i = startIndex; i < endIndex; i++)
            {
                GameObject go = Instantiate(characterListCellImage, characterListCells.transform.GetChild(i));
                go.GetComponent<Image>().sprite = GetCharacterSprite(DataManager.Instance.CurrentPlayerData.characters[i].name);
            }
        }

        private Sprite GetCharacterSprite(CharacterEnum c)
        {
            int index = 0;

            for(int i = 0; i < characterSpritesKey.Count; i++)
            {
                if (characterSpritesKey[i] == c)
                {
                    index = i;
                    break;
                }
            }

            return characterSpritesValue[index];
        }

        private void LoadParty()
        {
            if (DataManager.Instance.CurrentPlayerData.party[GameManager.Instance.PartyIndex - 1, 0] != 0)
            {
                partyBoxCharacterObjects.Add(Instantiate(characterListCellImage, partyBoxCharacter1.transform.GetChild(0)));
                partyBoxCharacterObjects[partyBoxCharacterObjects.Count - 1].GetComponent<Image>().sprite = GetCharacterSprite(DataManager.Instance.CurrentPlayerData.party[GameManager.Instance.PartyIndex - 1, 0]);
                foreach (var item in DataManager.Instance.CurrentPlayerData.characters)
                    if (item.name == DataManager.Instance.CurrentPlayerData.party[GameManager.Instance.PartyIndex - 1, 0])
                    {
                        partyBoxCharacter1.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = item.level.ToString();
                    }
            }
            if (DataManager.Instance.CurrentPlayerData.party[GameManager.Instance.PartyIndex - 1, 1] != 0)
            {
                partyBoxCharacterObjects.Add(Instantiate(characterListCellImage, partyBoxCharacter2.transform.GetChild(0)));
                partyBoxCharacterObjects[partyBoxCharacterObjects.Count - 1].GetComponent<Image>().sprite = GetCharacterSprite(DataManager.Instance.CurrentPlayerData.party[GameManager.Instance.PartyIndex - 1, 1]);
                foreach (var item in DataManager.Instance.CurrentPlayerData.characters)
                    if (item.name == DataManager.Instance.CurrentPlayerData.party[GameManager.Instance.PartyIndex - 1, 1])
                    {
                        partyBoxCharacter2.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = item.level.ToString();
                    }
            }
            if (DataManager.Instance.CurrentPlayerData.party[GameManager.Instance.PartyIndex - 1, 2] != 0)
            {
                partyBoxCharacterObjects.Add(Instantiate(characterListCellImage, partyBoxCharacter3.transform.GetChild(0)));
                partyBoxCharacterObjects[partyBoxCharacterObjects.Count - 1].GetComponent<Image>().sprite = GetCharacterSprite(DataManager.Instance.CurrentPlayerData.party[GameManager.Instance.PartyIndex - 1, 2]);
                foreach (var item in DataManager.Instance.CurrentPlayerData.characters)
                    if (item.name == DataManager.Instance.CurrentPlayerData.party[GameManager.Instance.PartyIndex - 1, 2])
                    {
                        partyBoxCharacter3.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = item.level.ToString();
                    }
            }
        }
    }
}