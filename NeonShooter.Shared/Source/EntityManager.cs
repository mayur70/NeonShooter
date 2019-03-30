using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NeonShooter.Shared.Source
{
    static class EntityManager
    {
        static List<Entity> entities = new List<Entity>();

        static bool isUpdating;
        static List<Entity> addedEntities = new List<Entity>();
        static List<Enemy> enemies = new List<Enemy>();
        static List<Bullet> bullets = new List<Bullet>();


        public static int Count { get { return entities.Count; } }

        public static void Add(Entity entity)
        {
            if (!isUpdating)
                AddEntity(entity);
                //entities.Add(entity);
            else
                addedEntities.Add(entity);
        }

        public static void Update(GameTime gameTime)
        {
            isUpdating = true;

            foreach (Entity entity in entities)
                entity.Update(gameTime);

            isUpdating = false;

            foreach (Entity entity in addedEntities)
                AddEntity(entity);
            addedEntities.Clear();

            HandleCollisions();

            //remove expired entities
            entities = entities.Where(entity => !entity.IsExpired).ToList();
            bullets = bullets.Where(bullet => !bullet.IsExpired).ToList();
            enemies = enemies.Where(enemy => !enemy.IsExpired).ToList();
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity entity in entities)
                entity.Draw(spriteBatch);
        }

        private static void AddEntity(Entity entity)
        {
            entities.Add(entity);
            if (entity is Bullet)
                bullets.Add(entity as Bullet);
            else if (entity is Enemy)
                enemies.Add(entity as Enemy);
        }

        private static bool IsColliding(Entity a, Entity b)
        {
            float radius = a.Radius + b.Radius;
            return !a.IsExpired && !b.IsExpired && Vector2.DistanceSquared(a.Position, b.Position) < radius * radius;
        }

        static void HandleCollisions()
        {
            //between enemies
            for(int i =0; i< enemies.Count; i++)
            {
                for(int j = i + 1; j < enemies.Count; j++)
                {
                    if(IsColliding(enemies[i], enemies[j]))
                    {
                        enemies[i].HandleCollision(enemies[j]);
                        enemies[j].HandleCollision(enemies[i]);
                    }
                }
            }
            //Debug.WriteLine("enemies - " + enemies.Count + "bullets - " + bullets.Count);
            //between bullets & enemies
            for(int i=0;i< enemies.Count; i++)
            {
                for(int j =0; j < bullets.Count; j++)
                {
                    if (IsColliding(enemies[i], bullets[j]))
                    {
                        enemies[i].WasShot();
                        bullets[j].IsExpired = true;
                    }
                }
            }

            //between player & enemies
            for(int i =0; i < enemies.Count; i++)
            {
                if(enemies[i].IsActive && IsColliding(PlayerShip.Instance, enemies[i]))
                {
                    PlayerShip.Instance.Kill();
                    enemies.ForEach(enemy => enemy.WasShot());
                    break;
                }
            }
        }
    }
}
