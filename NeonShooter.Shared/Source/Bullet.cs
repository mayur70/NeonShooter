using Microsoft.Xna.Framework;

namespace NeonShooter.Shared.Source
{
    class Bullet : Entity
    {
        public Bullet(Vector2 position, Vector2 velocity)
        {
            image = Art.Bullet;
            Position = position;
            Velocity = velocity;
            Orientation = Velocity.ToAngle();
            Radius = 8f;
        }
        public override void Update(GameTime gameTime)
        {
            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();

            Position += Velocity;

            if (!GameRoot.Viewport.Bounds.Contains(Position.ToPoint()))
                IsExpired = true;
        }
    }
}
