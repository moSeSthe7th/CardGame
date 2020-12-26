using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardFindingGame
{
    public class Gambler : MonoBehaviour
    {
        [SerializeField] private Transform speechBubblesParent;

        private Collider2D _collider;
        private Collider2D gamblerCollider => _collider ?? (_collider = GetComponent<Collider2D>());

        public void CloseGamblerCollider()
        {
            gamblerCollider.enabled = false;
        }

    }
}

