using Unity.Netcode.Components;
using UnityEngine;


    [DisallowMultipleComponent]
    public class NetworkClientTransform : NetworkTransform
    {
        
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
