using Agents;

namespace Enemies
{
    public abstract class AbstractEnemy : Agent
    {
        public INavMovement NavMovement { get; private set; }

        protected override void InitializeComponents()
        {
            base.InitializeComponents();
            NavMovement = GetModule<INavMovement>();
        }
    }
}