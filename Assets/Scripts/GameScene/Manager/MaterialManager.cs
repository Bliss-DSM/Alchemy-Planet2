﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AlchemyPlanet.GameScene
{
    public class MaterialManager : MonoBehaviour
    {
        public static MaterialManager Instance { get; private set; }
        public Dictionary<Vector3, GameObject> Objects { get; private set; }
        public Dictionary<string, int> MaterialNumbers { get; private set; }
        public List<Material> MaterialChain { get; private set; }
        public List<Line> Lines { get; private set; }

        public int MaxChainNumber { get; private set; }
        public int Count { get; private set; }
        public int MinMaterialNumber { get; private set; }
        public float MinDistance { get; private set; }
        public bool IsClickedRightMaterial { get; set; }

        private const float x_min = 62.0f;
        private const float x_max = 660.0f;
        private const float y_min = 62.0f;
        private const float y_max = 574.0f;

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);

            Objects = new Dictionary<Vector3, GameObject>();
            MaterialNumbers = new Dictionary<string, int>();
            MaterialChain = new List<Material>();
            Lines = new List<Line>();

            MaxChainNumber = 5;
            Count = 17;
            MinMaterialNumber = 2;
            MinDistance = 100;
            IsClickedRightMaterial = false;
        }

        private void Start()
        {

            SetPositions(Count);

            // materialNumbers 초기화
            foreach (var item in PrefabManager.Instance.materialPrefabs)
                MaterialNumbers.Add(item.GetComponent<Material>().materialName, 0);

            foreach (var item in Objects.Keys.ToList())
                InstantiateMaterial(item);
        }

        public void SetPositions(int count)
        {
            Vector3 temp = Vector3.zero;
            bool isNotTooClose = true;

            for(int i = 0; i < count; i++)
            {
                isNotTooClose = true;
                temp.x = Random.Range(x_min, x_max);
                temp.y = Random.Range(y_min, y_max);

                foreach(var item in Objects)
                    if ((item.Key - temp).sqrMagnitude < (MinDistance * MinDistance)) isNotTooClose = false;

                if (isNotTooClose == true) Objects.Add(temp, null);
                else i--;
            }
        }

        public void DecreaseMaterialNumber(string name)
        {
            MaterialNumbers[name]--;
        }

        private void InstantiateMaterial(Vector3 key)
        {
            int materialIndex;

            if (FindFewMaterial(out materialIndex) == false)
                materialIndex = Random.Range(0, PrefabManager.Instance.materialPrefabs.Length);

            Material material;

            Objects[key] = Instantiate(PrefabManager.Instance.materialPrefabs[materialIndex], transform);
            Objects[key].transform.position = new Vector3(key.x, key.y);

            material = Objects[key].GetComponent<Material>();
            MaterialNumbers[material.materialName]++;
        }

        public void ReinstantiateMaterial(Material material)
        {
            DecreaseMaterialNumber(material.materialName);
            StartCoroutine("ReinstantiateMaterialCoroutine", material.transform.position);
            Destroy(material.gameObject);
        }

        private IEnumerator ReinstantiateMaterialCoroutine(Vector3 key)
        {
            yield return new WaitForSeconds(1.0f);
            InstantiateMaterial(key);
        }

        private bool FindFewMaterial(out int index)
        {
            index = 0;

            foreach (var item in MaterialNumbers)
            {
                if (item.Value < MinMaterialNumber) return true;
                index++;
            }

            return false;
        }
    }
}