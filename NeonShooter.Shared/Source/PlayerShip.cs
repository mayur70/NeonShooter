using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace NeonShooter.Shared.Source
{
    class PlayerShip : Entity
    {
        const float speed = 8f;
        const int cooldownFrames = 6;
        int cooldownRemaining = 0;
        int framesUntilRespawn = 0;

        static readonly Random rnd = new Random();
        private static PlayerShip instance;

        public bool IsDead { get { return framesUntilRespawn > 0; } }

        public static PlayerShip Instance
        {
            get
            {
                if (instance == null)
                    instance = new PlayerShip();
                return instance;
            }
        }
        private PlayerShip()
        {
            image = Art.Player;
            Position = GameRoot.ScreenSize / 2;
            Radius = 10f;
        }
        public override void Update(GameTime gameTime)
        {
            if (IsDead)
            {
                framesUntilRespawn--;
               
                return;
            }

            // if Previous game over then reset
            if (PlayerStatus.IsGameOver)
            {
                PlayerStatus.Reset();
            }

            Velocity = speed * Input.GetMovementDirection();
            Position += Velocity;
            Position = Vector2.Clamp(Position, Size / 2, GameRoot.ScreenSize - Size / 2);

            if(Velocity.LengthSquared() > 0)
            {
                Orientation = Velocity.ToAngle();
            }

            var aim = Input.GetAimDirection();
            if(aim.LengthSquared() > 0 && cooldownRemaining <= 0)
            {
                cooldownRemaining = cooldownFrames;
                float aimAngle = aim.ToAngle();
                Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);

                float randomSpread = rnd.NextFloat(-0.04f, 0.04f) + rnd.NextFloat(-0.04f, 0.04f);
                Vector2 vel = MathUtil.FromPolar(aimAngle + randomSpread, 11f);

                Vector2 offset = Vector2.Transform(new Vector2(25, -8), aimQuat);
                EntityManager.Add(new Bullet(Position + offset, vel));

                offset = Vector2.Transform(new Vector2(25, 8), aimQuat);
                EntityManager.Add(new Bullet(Position + offset, vel));
                if (!GameRoot.MuteSounds)
                {
                    Sound.Shot.Play(0.2f, rnd.NextFloat(-0.2f, 0.2f), 0);
                }
            }

            if (cooldownRemaining > 0)
                cooldownRemaining--;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(!IsDead)
                base.Draw(spriteBatch);
        }

        public void Kill()
        {
            framesUntilRespawn = 60;
            PlayerStatus.RemoveLife();

            framesUntilRespawn = PlayerStatus.IsGameOver ? 300 : 120;
            
        }
    }
}
