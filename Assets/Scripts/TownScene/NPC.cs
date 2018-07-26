﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.TownScene
{
    public class NPC : MonoBehaviour
    {
        public NPCDAta data;
        public float speed;
        public float moveDistance;

        private int moveChoice;         // 움직임
        private bool moving;            // 움직이는 중
        private bool talking = false;   // 말하는 중
        private Animator animator;      // 애니메이터

        // Use this for initialization
        void Start()
        {
            data = new NPCDAta(this.gameObject.name);

            moving = false;
            moveChoice = Random.Range(0, 3);
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            Move();
        }

        private void Move()
        {
            if (!moving)
            {
                moving = true;
                switch (moveChoice)
                {
                    case 0:
                        StartCoroutine("RightMove");
                        break;
                    case 1:
                        StartCoroutine("LeftMove");
                        break;
                    case 2:
                        StartCoroutine("StopMove");
                        break;
                }
            }
        }

        void stop()
        {
            animator.SetBool("Run", false);
            StopAllCoroutines();
            moving = true;
        }

        void talk()
        {
            if (!talking)
            {
                talking = true;
                UIManager.Instance.OpenMenu<DialogUI>();
                talking = false;
            }
            moveChoice = 2;
            StartCoroutine("StopMove");
        }

        private IEnumerator RightMove()
        {
            float firstPosX = transform.position.x;
            animator.SetBool("Run", true);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            while (firstPosX + moveDistance > transform.position.x)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                yield return null;
            }
            animator.SetBool("Run", false);
            moving = false;
            moveChoice = 2;
            yield return null;
        }

        private IEnumerator LeftMove()
        {
            float firstPosX = transform.position.x;
            animator.SetBool("Run", true);
            transform.rotation = Quaternion.Euler(0, 180, 0);
            while (firstPosX - moveDistance < transform.position.x)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                yield return null;
            }
            animator.SetBool("Run", false);
            moving = false;
            moveChoice = 2;
            yield return null;
        }

        private IEnumerator StopMove()
        {
            animator.SetBool("Run", false);
            yield return new WaitForSeconds(2f);
            moving = false;
            moveChoice = Random.Range(0, 2);
            yield return null;
        }
    }
}