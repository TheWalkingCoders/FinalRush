﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace FinalRush
{
    class GameMain
    {
        // FIELDS

        public Player LocalPlayer;
        public List<Wall> Walls;
        public List<Bonus> bonus;
        public List<Enemy> enemies;
        public List<Enemy2> enemies2;
        Random random = new Random();
        MainMenu menu;
        Texture2D background = Resources.Environnment;


        // CONSTRUCTOR

        public GameMain()
        {
            menu = new MainMenu(Global.Handler, 0f);
            LocalPlayer = new Player();
            Walls = new List<Wall>();
            bonus = new List<Bonus>();
            enemies = new List<Enemy>();
            enemies2 = new List<Enemy2>();
            Global.GameMain = this;

            #region Ennemis
            //Ennemis

            enemies.Add(new Enemy(1100, 350, Resources.Zombie));
            enemies.Add(new Enemy(1150, 350, Resources.Zombie));
            enemies.Add(new Enemy(1180, 350, Resources.Zombie));
            enemies.Add(new Enemy(2550, 250, Resources.Zombie));
            enemies.Add(new Enemy(2650, 250, Resources.Zombie));

            enemies2.Add(new Enemy2(2400, 376, Resources.Zombie));

            #endregion

            #region Plateformes
            //Plateformes

            Walls.Add(new Wall(425, 245, Resources.Platform, 50, 16, Color.IndianRed));
            Walls.Add(new Wall(130, 360, Resources.Platform, 92, 16, Color.IndianRed));
            Walls.Add(new Wall(280, 300, Resources.Platform, 92, 16, Color.IndianRed));
            Walls.Add(new Wall(680, 365, Resources.Platform, 92, 16, Color.IndianRed));
            Walls.Add(new Wall(520, 300, Resources.Platform, 92, 16, Color.IndianRed));
            Walls.Add(new Wall(1160, 200, Resources.Platform, 92, 16, Color.IndianRed));
            Walls.Add(new Wall(1266, 415, Resources.Platform, 1, 1, Color.IndianRed));
            Walls.Add(new Wall(1664, 350, Resources.Platform, 92, 16, Color.IndianRed));
            Walls.Add(new Wall(1828, 280, Resources.Platform, 92, 16, Color.IndianRed));
            Walls.Add(new Wall(1828, 140, Resources.Platform, 92, 16, Color.IndianRed));
            Walls.Add(new Wall(1664, 210, Resources.Platform, 92, 16, Color.IndianRed));
            Walls.Add(new Wall(1984, 140, Resources.Platform, 92, 16, Color.IndianRed));
            Walls.Add(new Wall(2100, 240, Resources.Platform, 92, 16, Color.IndianRed));
            Walls.Add(new Wall(2200, 340, Resources.Platform, 92, 16, Color.IndianRed));
            Walls.Add(new Wall(1984, 340, Resources.Platform, 50, 16, Color.IndianRed));
            Walls.Add(new Wall(2500, 340, Resources.Platform, 200, 16, Color.IndianRed));
            Walls.Add(new Wall(2500, 339, Resources.Platform, 1, 1, Color.IndianRed));
            Walls.Add(new Wall(2700, 339, Resources.Platform, 1, 1, Color.IndianRed));
            Walls.Add(new Wall(3520, 350, Resources.Platform, 92, 16, Color.IndianRed));
            Walls.Add(new Wall(3620, 300, Resources.Platform, 40, 16, Color.IndianRed));
            Walls.Add(new Wall(3680, 250, Resources.Platform, 20, 16, Color.IndianRed));
            Walls.Add(new Wall(3720, 200, Resources.Platform, 20, 16, Color.IndianRed));
            Walls.Add(new Wall(3860, 400, Resources.Platform, 92, 16, Color.IndianRed));
            Walls.Add(new Wall(4000, 340, Resources.Platform, 92, 16, Color.IndianRed));

            #endregion

            #region Terrain
            //Colonnes et sol

            Walls.Add(new Wall(896, 416, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(896, 352, Resources.Herbe, 64, 64, Color.White));
            Walls.Add(new Wall(960, 416, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(960, 352, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(960, 288, Resources.Herbe, 64, 64, Color.White));
            Walls.Add(new Wall(1024, 416, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(1024, 352, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(1024, 288, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(1024, 224, Resources.Herbe, 64, 64, Color.White));
            Walls.Add(new Wall(1600, 0, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(1600, 64, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(1600, 128, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(1600, 192, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(1600, 256, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(1600, 320, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(1920, 416, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(1920, 352, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(1920, 288, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(1920, 224, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(1920, 160, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(1920, 96, Resources.Herbe, 64, 64, Color.White));
            Walls.Add(new Wall(2816, 416, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(2816, 352, Resources.Herbe, 64, 64, Color.White));
            Walls.Add(new Wall(2944, 416, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(2944, 352, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(2944, 288, Resources.Herbe, 64, 64, Color.White));
            Walls.Add(new Wall(3072, 416, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(3072, 352, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(3072, 288, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(3072, 224, Resources.Herbe, 64, 64, Color.White));
            Walls.Add(new Wall(3200, 416, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(3200, 352, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(3200, 288, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(3200, 224, Resources.Ground, 64, 64, Color.White));
            Walls.Add(new Wall(3200, 160, Resources.Herbe, 64, 64, Color.White));



            //Sol

            for (int i = 0; i < 80; i++)
                if (i != 2 & i != 3 & i != 4 & i != 5 & i != 6 & i != 7 & i != 8 & i != 9
                  & i != 14 & i != 15 & i != 16 & i != 20 & i != 21 & i != 30
                  & i != 44 & i != 45 & i != 46 & i != 47 & i != 48 & i != 49 & i != 50 & i != 51
                  & i != 55 & i != 56 & i != 57 & i != 58 & i != 59 & i != 60 & i != 61 & i != 62)
                    Walls.Add(new Wall(64 * i, 416, Resources.Herbe, 64, 64, Color.White));
            #endregion

            #region Bonus
            //Bonus

            bonus.Add(new Bonus(700, 345, Resources.Coin, 20, 20, Color.White));
            bonus.Add(new Bonus(1200, 100, Resources.Coin, 20, 20, Color.White));
            bonus.Add(new Bonus(1940, 76, Resources.Coin, 20, 20, Color.White));
            bonus.Add(new Bonus(1984, 320, Resources.Coin, 20, 20, Color.White));
            bonus.Add(new Bonus(1984, 396, Resources.Coin, 20, 20, Color.White));
            bonus.Add(new Bonus(2520, 320, Resources.Coin, 20, 20, Color.White));
            bonus.Add(new Bonus(2650, 320, Resources.Coin, 20, 20, Color.White));
            bonus.Add(new Bonus(2816, 332, Resources.Coin, 20, 20, Color.White));
            bonus.Add(new Bonus(3200, 140, Resources.Coin, 20, 20, Color.White));
            bonus.Add(new Bonus(4020, 320, Resources.Coin, 20, 20, Color.White));
            #endregion
        }

        // UPDATE & DRAW

        public void Update(MouseState souris, KeyboardState clavier)
        {
            GameTime gametime = new GameTime();
            LocalPlayer.Update(souris, clavier, Walls, bonus);
            menu.Update(gametime);
            for (int i = 0; i < enemies2.Count; i++)
            {
                for (int j = 0; j < Global.Player.bullets.Count; j++)
                    if (Global.Enemy2.Hitbox.Intersects(new Rectangle((int)Global.Player.bullets[j].position.X,(int)Global.Player.bullets[j].position.Y,10,10)))
                    {
                        enemies2.RemoveAt(i);
                        i--;
                    }
            }
            foreach (Enemy enemy in enemies)
                enemy.Update(Walls, random.Next(10, 1000));
            foreach (Enemy2 enemy2 in enemies2)
                enemy2.Update(Walls);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            if (LocalPlayer.Hitbox.X <= 400)
                spritebatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);
            else if (LocalPlayer.Hitbox.X >= 4200)
                spritebatch.Draw(background, new Rectangle(3800, 0, 800, 480), Color.White);
            else
                spritebatch.Draw(background, new Rectangle(LocalPlayer.Hitbox.X + LocalPlayer.Hitbox.Width / 2 - 400, 0, 800, 480), Color.White);
            LocalPlayer.Draw(spritebatch);

            foreach (Enemy enemy in enemies)
                enemy.Draw(spritebatch);

            foreach (Enemy2 enemy2 in enemies2)
                enemy2.Draw(spritebatch);

            foreach (Wall wall in Walls)
                wall.Draw(spritebatch);

            foreach (Bonus b in bonus)
                b.Draw(spritebatch);
        }
    }
}