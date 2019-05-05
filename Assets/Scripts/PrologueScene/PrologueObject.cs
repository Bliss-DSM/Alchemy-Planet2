﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AlchemyPlanet.PrologueScene
{
    public enum ObjectKind { NPC, Object, Door, Zoom, Switch}
    public enum ObjectSwitch { NPCTutorial, End }

    public class PrologueObject : MonoBehaviour
    {
        [SerializeField] private string objcect_name;
        [SerializeField] private ObjectKind kind;

        //Object일 떄 필요한 말풍선
        [SerializeField] private GameObject bubble;

        [SerializeField] private MainCamera mainCamera;

        //Nexxt Stage
        [SerializeField] private GameObject currentStages;
        [SerializeField] private GameObject nextStage;

        //스위치
        [SerializeField] private ObjectSwitch switchKind;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && kind != ObjectKind.NPC)
            {
                ActiveObject(collision.gameObject);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                DeactiveObject();
            }
            //if(kind == ObjectKind.NPC)
            //{
            //    ActiveObject();
            //}
        }

        public void ActiveObject(GameObject target)
        {
            switch (kind)
            {
                case ObjectKind.NPC:
                    {
                        if (transform.position.x >= target.transform.position.x)
                        {
                            target.transform.rotation = Quaternion.Euler(0, 0, 0);
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                        }
                        else
                        {
                            target.transform.rotation = Quaternion.Euler(0, 180, 0);
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
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
                        StartCoroutine(MoveNext());
                        break;
                    }
                case ObjectKind.Zoom:
                    {
                        mainCamera.ZoomIn(7);
                        break;
                    }
                case ObjectKind.Switch:
                    {
                        switch (switchKind)
                        {
                            case ObjectSwitch.NPCTutorial:
                                PrologueScript.Instance.isOnNPCPos = true; break;
                            case ObjectSwitch.End:
                                PrologueScript.Instance.isEnd = true; break;
                        }
                        break;
                    }
            }
        }

        public void DeactiveObject()
        {
            if(kind == ObjectKind.Object)
            {
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
            bubble.transform.DOScaleY(0.4f, 0.3f);
        }
        public void CloseBubble()
        {
            bubble.transform.DOScaleY(0, 0.5f).OnComplete(() => bubble.SetActive(false));
        }

        public IEnumerator MoveNext()
        {
            mainCamera.FadeOut();
            yield return new WaitForSeconds(2);

            currentStages.SetActive(false);
            nextStage.SetActive(true);
            mainCamera.FadeIn();
        }
    }
}
