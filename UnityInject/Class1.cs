using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityInjectX;

namespace UnityInject
{
    public class Loader
    {
        private static GameObject _Load;
        public static void Init()
        {

            _Load = new GameObject();
            _Load.AddComponent<Main>();
            GameObject.DontDestroyOnLoad(_Load);
        }
        public static void Unload()
        {
            _Unload();
        }
        private static void _Unload()
        {
            GameObject.Destroy(_Load);
        }
    }
}
