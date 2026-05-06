using System;
using UnityEngine;

namespace Scenes.TestScene
{
    public class Bullet : MonoBehaviour
    {
        private float speed = 10;
        public Vector3 direction;

        private void Update()
        {
            transform.position += direction * (speed * Time.deltaTime);
        }
    }
}
