using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace MysteryIsland.World.Collision
{
    public class StaticActor : ICollisionActor
    {
        public IShapeF Bounds { get; }

        public StaticActor(IShapeF bounds)
        {
            this.Bounds = bounds;
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            // static actors do nothing
        }
    }
}
