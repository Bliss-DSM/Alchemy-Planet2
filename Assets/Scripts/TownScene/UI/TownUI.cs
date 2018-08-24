﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlchemyPlanet.Data;
using DG.Tweening;

namespace AlchemyPlanet.TownScene {
    public class TownUI : Common.UI<TownUI>
    {
        public GameObject player;
        public Camera mainCamera;
        public bool turnOnBuildBar;

        [SerializeField] private Button buildingbutton;
        [SerializeField] private GameObject buildBar;
        [SerializeField] private Button UIOffButton;
        [SerializeField] private Button TownManageButton;
        [SerializeField] private Button TownUpgradeButton;
        [SerializeField] private Button InventoryButton;
        
        protected override void Awake()
        {
            base.Awake();
            StartCoroutine(LateAwake());
        }

        private void Start()
        {
            /*
            GameObject house = Instantiate(Resources.Load<GameObject>("Prefabs/TownScene/House"));
            GameObject tree = Instantiate(Resources.Load<GameObject>("Prefabs/TownScene/Tree"));
            
            DataManager.Instance.CurrentPlayerData.setupBuildings.Add(house, "House");
            DataManager.Instance.CurrentPlayerData.setupBuildings.Add(tree, "Tree");
            DataManager.Instance.CurrentPlayerData.ownBuildings.Add("Tree", 1);
            */
            GetComponent<CanvasScaler>().uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
            foreach (string str in DataManager.Instance.CurrentPlayerData.setupBuildings.Values) {
                Instantiate(DataManager.Instance.structures[str].StructureObject);
            } // 저장된 타운 불러오기
    }

        IEnumerator LateAwake()
        {
            while (UIManager.Instance == null) {
                yield return null;
            }
            UIManager.Instance.Clear();
            UIManager.Instance.menuStack.Push(Instance);
            this.transform.SetParent(UIManager.Instance.transform);

            turnOnBuildBar = false;
            buildingbutton.onClick.AddListener(() => {
                StartCoroutine("MoveBar");
            });
            UIOffButton.onClick.AddListener(() =>
            {
                UIManager.Instance.menuStack.Peek().gameObject.SetActive(false);
                UIManager.Instance.OpenMenu<EmptyUI>();
            });
            TownManageButton.onClick.AddListener(() =>
            {
                UIManager.Instance.menuStack.Peek().gameObject.SetActive(false);
                UIManager.Instance.OpenMenu<TownManager>();
            });
            TownUpgradeButton.onClick.AddListener(() =>
            {
                UIManager.Instance.menuStack.Peek().gameObject.SetActive(false);

            });
            InventoryButton.onClick.AddListener(() => {
                UIManager.Instance.OpenMenu<InventoryCell>();

                InventoryCell.Instance.SetItem();
            });

        }

        IEnumerator MoveBar()
        {
            if (!turnOnBuildBar) {
                //while (buildBar.transform.position.x > 535)
                //{
                //    buildBar.transform.Translate(Vector2.left * 500 * Time.deltaTime);
                //    yield return new WaitForFixedUpdate();
                //}

                buildBar.transform.DOMoveX(535, 1).SetEase(Ease.OutQuint);
                turnOnBuildBar = true;
            }
            else
            {
                //while (buildBar.transform.position.x < 900)
                //{
                //    buildBar.transform.Translate(Vector2.right * 500 * Time.deltaTime);
                //    yield return new WaitForFixedUpdate();
                //}

                buildBar.transform.DOMoveX(900, 1).SetEase(Ease.OutQuint);
                turnOnBuildBar = false;
            }

            yield return null;
        }
    }
}