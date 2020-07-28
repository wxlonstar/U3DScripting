using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MileCode.MileTest { 
    public partial class MyLevel : MonoBehaviour {
        #region SINGLETON
        private static MyLevel _instance;
        public static MyLevel Instance {
            get {
                if(_instance == null) {
                    _instance = GameObject.FindObjectOfType<MyLevel>();
                }
                return _instance;
            }
            
        }
        #endregion
    }
}
