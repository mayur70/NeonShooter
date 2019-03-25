using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeonShooter.Shared.Source
{
    abstract class Entity
    {
        protected Texture2D image;
        protected Color color = Color.White; // Tint color of image, allows us to change transperancy

        public Vector2 Position;
        public Vector2 Velocity;
        public float Orientation;
        public float Radius = 20f; // for collision detection
        public bool IsExpired; //true if entity was destroyed & should be deleted
        public Vector2 Size
        {
            get
            {
                return image == null ? Vector2.Zero : new Vector2(image.Width, image.Height);
            }
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Position, null, color, Orientation, Size / 2, 1f, SpriteEffects.None, 0);
        }
    }
}
