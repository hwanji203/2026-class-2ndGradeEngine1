using System;
using System.Collections;
using UnityEngine;

namespace GGMLib.FeedbackSystem
{
    public class BlickFeedback : AbstractFeedback
    {
        [SerializeField] private SkinnedMeshRenderer targetRenderer;
        [SerializeField] private float blinkDuration = 0.1f;
        [SerializeField] private float blinkIntensity = 0.2f;

        private Material _targetMaterial;
        private WaitForSeconds _waitTime;
        private readonly int _blinkValueHash = Shader.PropertyToID("_blinkValue");

        private MaterialPropertyBlock _propertyBlock;

        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
            targetRenderer.GetPropertyBlock(_propertyBlock); //프로퍼티 블록 리딩
            _waitTime = new WaitForSeconds(blinkDuration);
        }

        public override void CreateFeedback()
        {
            StopFeedback();
            StartCoroutine(BlickCoroutine());
        }

        private IEnumerator BlickCoroutine()
        {
            _propertyBlock.SetFloat(_blinkValueHash, blinkIntensity);
            targetRenderer.SetPropertyBlock(_propertyBlock);
            yield return _waitTime;
            _propertyBlock.SetFloat(_blinkValueHash, 0);
            targetRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}