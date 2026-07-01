using UnityEngine;

namespace GGMLib.FeedbackSystem
{
    public abstract class AbstractFeedback : MonoBehaviour
    {
        public abstract void CreateFeedback();

        public virtual void StopFeedback()
        {
            
        }
    }
}