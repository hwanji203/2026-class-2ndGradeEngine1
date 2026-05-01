using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GGMLib.ModuleSystem
{
    public abstract class ModuleOwner : MonoBehaviour
    {
        private Dictionary<Type, IModule> _compos = new();

        protected virtual void Awake()
        {
            _compos = GetComponentsInChildren<IModule>().ToDictionary(compo => compo.GetType());
            // Type 런타임 중 객체의 정보 저장
            InitializeComponents();
            AfterInitComponents();
        }

        protected virtual void InitializeComponents()
        {
            foreach (IModule module in _compos.Values)
            {
                module.Initialize(this);
            }
        }
        protected virtual void AfterInitComponents()
        {
            foreach (IAfterInitModule module in _compos.Values.OfType<IAfterInitModule>())
            {
                module.AfterInit();
            }
        }

        public T GetModule<T>()
        {
            if (_compos.TryGetValue(typeof(T), out var module))
                return (T)module;

            IModule findedModule = _compos.Values.FirstOrDefault(moduleType => moduleType is T);
            if (findedModule is T castedModule)
                return castedModule;
            return default;
        }
    }
}