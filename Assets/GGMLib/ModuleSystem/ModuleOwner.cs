using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GGMLib
{
    public abstract class ModuleOwner : MonoBehaviour
    {
        protected Dictionary<Type, IModule> _moduleDict;

        protected virtual void Awake()
        {
            _moduleDict = GetComponentsInChildren<IModule>().ToDictionary(module => module.GetType());

            InitializeComponents();
            AfterInitComponents();
        }

        protected virtual void InitializeComponents()
        {
            foreach (var module in _moduleDict.Values)
            {
                module.Initialize(this);
            }
        }

        protected virtual void AfterInitComponents()
        {
            foreach (var module in _moduleDict.Values.OfType<IAfterInitModule>())
            {
                module.AfterInit();
            }
        }

        public T GetModule<T>()
        {
            if (_moduleDict.TryGetValue(typeof(T), out IModule module))
                return (T)module;

            IModule findModule = _moduleDict.Values.FirstOrDefault(module => module is T);

            if (findModule is T castedModlue)
                return castedModlue;

            return default;
        }
    }
}

