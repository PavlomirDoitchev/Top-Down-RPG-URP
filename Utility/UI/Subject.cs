﻿
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utility.UI
{
    public abstract class Subject : MonoBehaviour
    {
        private List<IObserver> _observers = new List<IObserver>();

        public void AddObserver(IObserver observer) 
        {
            _observers.Add(observer);
        }
        public void RemoveObserver(IObserver observer) 
        {
            _observers.Remove(observer);
        }

        protected void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.OnNotify();
            }
        }
    }
}
