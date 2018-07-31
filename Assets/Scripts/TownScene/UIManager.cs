﻿using System.Collections.Generic;
using UnityEngine;
using AlchemyPlanet.TownScene;

namespace AlchemyPlanet.TownScene
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [SerializeField] private TownUI TownUIPrefab;
        [SerializeField] private DialogUI DialogUIPrefab;
        [SerializeField] private DialogLogMenu DialogLogMenuPrefab;
        [SerializeField] private EmptyUI EmptyUIPrefab;
        [SerializeField] private InventoryCell InventoryPrefab;
        [SerializeField] private TownManager TownManagerUIPrefab;

        public Stack<Common.UI> menuStack = new Stack<Common.UI>();

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                GameObject.Destroy(this.gameObject);
            }
        }

        public void Clear()
        {
            while (menuStack.Count > 0)
            {
                CloseMenu();
            }
        }

        public void OpenMenu<T>() where T : Common.UI
        {
            var prefab = GetPrefab<T>();
            var instance = Instantiate<Common.UI>(prefab, transform);

            menuStack.Push(instance);
        }

        //public void OpenMenu<T>(bool disablePrev) where T : Common.UI
        //{
        //    var prefab = GetPrefab<T>();
        //    var instance = Instantiate<Common.UI>(prefab, transform);

        //    menuStack.Push(instance);
        //}

        public void CloseMenu()
        {
            var instance = menuStack.Pop();
            GameObject.Destroy(instance.gameObject);
            if (menuStack.Count > 1)
            {
                menuStack.Peek().gameObject.SetActive(true);
            }
        }

        public T GetPrefab<T>() where T : Common.UI
        {
            if (typeof(T) == typeof(TownUI))
                return TownUIPrefab as T;
            if (typeof(T) == typeof(DialogUI))
                return DialogUIPrefab as T;
            if (typeof(T) == typeof(DialogLogMenu))
                return DialogLogMenuPrefab as T;
            if (typeof(T) == typeof(EmptyUI))
                return EmptyUIPrefab as T;
            if (typeof(T) == typeof(InventoryCell))
                return InventoryPrefab as T;
            if (typeof(T) == typeof(TownManager))
                return TownManagerUIPrefab as T;

            throw new MissingReferenceException();
        }
    }
}