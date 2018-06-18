﻿using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class PrefabManager : MonoBehaviour
    {
        public static PrefabManager Instance { get; private set; }

        public GameObject tilePrefab;
        public GameObject[] materialPrefabs;
        public GameObject[] recipePrefabs;
        public GameObject monster;
        public GameObject line;

        public Sprite unselectedBubble;
        public Sprite selectedBubble;

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }
    }
}