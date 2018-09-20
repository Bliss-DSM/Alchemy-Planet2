﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AlchemyPlanet.PrologueScene
{
    public enum ObjectKind { NPC, Object, Door }

    public class PrologueObject : MonoBehaviour
    {
        [SerializeField] private string objcect_name;
        [SerializeField] private ObjectKind kind;

        //Object일 떄 필요한 말풍선
        [SerializeField] private GameObject bubble;

        //Nexxt Stage
        [SerializeField] private GameObject currentStages;
        [SerializeField] private GameObject nextStage;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Debug.Log("On");
                switch (kind)
                {
                    case ObjectKind.NPC:
                        {
                            OpenDialog();
                            break;
                        }
                    case ObjectKind.Object:
                        {
                            ShowBubble();
                            break;
                        }
                    case ObjectKind.Door:
                        {
                            MoveNext();
                            break;
                        }
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && kind == ObjectKind.Object)
            {
                Debug.Log("Off");
                CloseBubble();
            }
        }

        public void OpenDialog()
        {
            Data.DataManager.Instance.selected_dialog = new Data.NPCDAta(objcect_name);
            TownScene.UIManager.Instance.OpenMenu<TownScene.DialogUI>();
            TownScene.DialogUI.Instance.SetDialog();
        }

        public void ShowBubble()
        {
            bubble.SetActive(true);
            bubble.transform.DOScale(1, 0.5f);
        }
        public void CloseBubble()
        {
            bubble.transform.DOScale(0, 0.5f).OnComplete(() => bubble.SetActive(false));
        }
        public void MoveNext()
        {
            currentStages.SetActive(false);
            nextStage.SetActive(true);
        }
    }
}
