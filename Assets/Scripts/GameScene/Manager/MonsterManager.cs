﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class MonsterManager : MonoBehaviour
    {
        public static MonsterManager Instance { get; private set; } 
        public Dictionary<int, Monster> Monsters { get; private set; }
        public Vector3 SpawnPoint { get; private set; }
        public int Key { get; private set; }
        public float SpawnCooltime;

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = this;
            Monsters = new Dictionary<int, Monster>();
            SpawnPoint = new Vector3(6, 2, 0);
            Key = 0;
            SpawnCooltime = 20;
        }

        private void Start()
        {
            StartCoroutine("SpawnMonsterCoroutine");
        }

        public void SpawnMonster()
        {
            Monster monster = Instantiate(PrefabManager.Instance.monster, SpawnPoint, Quaternion.identity).GetComponent<Monster>();
            Monsters.Add(Key, monster);
            monster.Index = Monsters.Count - 1;
            monster.ChangeSortingLayer(monster.Index * -2, monster.Index * -2 + 1);
            Key++;
        }

        IEnumerator SpawnMonsterCoroutine()
        {
            while (true)
            {
                SpawnMonster();
                yield return new WaitForSeconds(SpawnCooltime);
            }
        }

        public void KillMonster(int key)
        {
            DestroyMonster(key);
            Monsters.Remove(key);
            ReallocateMonsterIndex();
        }

        private void DestroyMonster(int key)
        {
            Monster monster;
            Monsters.TryGetValue(key, out monster);
            Destroy(monster.gameObject);
        }

        private void ReallocateMonsterIndex()
        {
            int index = 0;
            foreach(var item in Monsters)
            {
                item.Value.Index = index;
                index++;
            }
        }

        public int GetKeyByValue(Monster monster)
        {
            foreach (var item in Monsters)
                if (item.Value.Equals(monster))
                    return item.Key;

            return -1;
        }
    }
}
