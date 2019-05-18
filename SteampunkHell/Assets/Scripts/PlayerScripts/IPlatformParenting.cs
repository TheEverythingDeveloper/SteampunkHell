using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlatformParenting
{
    void OnTriggerEnter(Collider other);
    void OnTriggerStay(Collider other);
    void OnTriggerExit(Collider other);

    /*EXPLICACION DE COMO USAR:
    Para usar esta interface, tenemos que heredar obviamente de esta interfaz. Despues tenemos que hacer algo tipo:
     
        //Esto en start o donde queramos que tenga platform parenting
        _platformParent = gameObject.AddComponent<PlatformParenting>();
        _platformParent._myLayerMask = Layers.PLAYER;
        _platformParent.debug = true;
         
        
        #region IPlatformParenting
        PlatformParenting _platformParent;
        public void OnTriggerEnter(Collider other)
        {
            _platformParent.OnTriggerEnter(other);
        }
        public void OnTriggerStay(Collider other)
        {
            _platformParent.OnTriggerStay(other);
        }
        public void OnTriggerExit(Collider other)
        {
            _platformParent.OnTriggerExit(other);
        }
        #endregion
         */
}
