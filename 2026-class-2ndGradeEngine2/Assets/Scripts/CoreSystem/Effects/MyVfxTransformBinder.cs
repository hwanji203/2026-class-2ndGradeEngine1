using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

namespace CoreSystem.Effects
{
    [VFXBinder("Transform/Custom Transform Binder")]
    public class MyVfxTransformBinder : VFXBinderBase
    {
        [VFXPropertyBinding("UnityEngine.Transform")]
        public ExposedProperty Property;
        public Transform Target;

        public override bool IsValid(VisualEffect component)
        {
            return true;
        }

        public override void UpdateBinding(VisualEffect component)
        {
        }
    }
}