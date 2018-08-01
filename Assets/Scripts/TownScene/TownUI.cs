﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyPlanet.TownScene {
    public class TownUI : Common.UI<TownUI>
    {
        [SerializeField] private Button buildingbutton;
        [SerializeField] private GameObject buildBar;
        [SerializeField] private Button UIOffButton;
        [SerializeField] private Button TownManageButton;
        [SerializeField] private Button TownUpgradeButton;
        [SerializeField] private Button InventoryButton;

        private bool turnOnBuildBar;
        private GameObject TownManager;

        protected override void Awake()
        {
            base.Awake();
            StartCoroutine(LateAwake());
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
                while (buildBar.transform.position.x > 535)
                {
                    buildBar.transform.Translate(Vector2.left * 500 * Time.deltaTime);
                    yield return new WaitForFixedUpdate();
                }
                turnOnBuildBar = true;
            }
            else
            {
                while (buildBar.transform.position.x < 900)
                {
                    buildBar.transform.Translate(Vector2.right * 500 * Time.deltaTime);
                    yield return new WaitForFixedUpdate();
                }
                turnOnBuildBar = false;
            }
        }

        void OnTownManager()
        {
            TownManager.SetActive(true);
        }

        void OffTownManager()
        {
            TownManager.SetActive(true);
        }
    }
}