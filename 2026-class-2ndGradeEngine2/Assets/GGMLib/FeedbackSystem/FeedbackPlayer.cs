using UnityEngine;

namespace GGMLib.FeedbackSystem
{
    public class FeedbackPlayer : MonoBehaviour
    {
        private AbstractFeedback[] _feedbacks;

        private void Awake()
        {
            _feedbacks = GetComponentsInChildren<AbstractFeedback>();
        }
        
        public void PlayerAllFeedback()
        {
            foreach (AbstractFeedback feedback in _feedbacks)
            {
                feedback.CreateFeedback();
            }
        }

        public void StopAllFeedbacks()
        {
            foreach (AbstractFeedback feedback in _feedbacks)
            {
                feedback.StopFeedback();
            }
        }
    }
}