﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace FinalRush
{
    class Boss
    {
        public Rectangle Hitbox;
        Direction Direction;
        int speed;
        public int pv;
        int fallspeed;
        int speedjump = 1;
        int random;
        int framecolumn;
        int origin;
        int compt = 0;
        bool left;
        public bool isDead2;
        int distance2player = 2000;
        List<Bullets> bullets;
        public List<Bullets> enemy_bullets;
        public bool a_portee;
        SoundEffectInstance shot_sound_instance;
        Color color;

        Random rand = new Random();
        SpriteEffects effect;
        Collisions collisions = new Collisions();

        public Boss(int x, int y, Texture2D newTexture)
        {
            framecolumn = 1;
            speed = 3;
            pv = 3;
            fallspeed = 5;
            random = rand.Next(7, 15);
            effect = SpriteEffects.None;
            Direction = Direction.Right;
            Hitbox.Width = 30;
            Hitbox.Height = 38;
            Hitbox = new Rectangle(x, y, Hitbox.Width, Hitbox.Height);
            left = true;
            isDead2 = false;
            bullets = Global.Player.bullets;
            enemy_bullets = new List<Bullets>();
            Global.Boss = this;
            a_portee = false;
            shot_sound_instance = Resources.tir_rafale.CreateInstance();
            origin = Hitbox.X;
        }

        public void Update(List<Wall> walls)
        {
            compt++; // Cette petite ligne correspond à l'IA ( WAAAW Gros QI )
            distance2player = Hitbox.X - Global.Player.Hitbox.X;

            #region Mort Ennemi
            for (int i = 0; i < bullets.Count(); i++)
            {
                if (Hitbox.Intersects(new Rectangle((int)bullets[i].position.X, (int)bullets[i].position.Y, 30, 30)))
                {
                    bullets[i].isVisible = false;
                    bullets.RemoveAt(i);
                    i--;
                    if (pv > 1)
                        pv--;
                    else
                        isDead2 = true;
                }
            }

            if (!isDead2 && Keyboard.GetState().IsKeyDown(Keys.D) && Global.Player.Hitbox.Intersects(Hitbox))
                isDead2 = true;

            #endregion

            #region Animation
            if (!isDead2)
            {
                if (distance2player != 0)
                {
                    if (framecolumn > 8)
                        framecolumn = 1;
                    else if (compt % 2 == 0)
                        framecolumn++;
                }
                else
                    framecolumn = 9;
            }
            #endregion

            #region Gravité
            if (!isDead2)
            {
                bool gravity = collisions.CollisionDown(Hitbox, walls, this.fallspeed);

                if (!gravity)
                {
                    if (collisions.CollisionUp(Hitbox, walls, fallspeed))
                        speedjump = 0;
                    if (speedjump > 1)
                    {
                        this.Hitbox.Y -= speedjump;
                        speedjump -= 1;
                    }
                    else
                        this.Hitbox.Y += this.fallspeed;
                }
                else
                    if (!collisions.CollisionDown(Hitbox, walls, this.fallspeed - 4))
                        if (!collisions.CollisionDown(Hitbox, walls, this.fallspeed - 2))
                            this.Hitbox.Y += 2;
                        else
                            this.Hitbox.Y++;
            }
            #endregion

            #region Déplacements
            if (!isDead2)
            {
                if (distance2player == 0)
                    speed = 0;
                else
                {
                    speed = 1;
                    if (distance2player < 0 && distance2player >= -200)
                    {
                        left = false;
                        compt = 1;
                    }
                    else
                        if (distance2player > 0 && distance2player <= 200)
                        {
                            left = true;
                            compt = 1;
                        }
                        else
                            if (distance2player > 200 || distance2player <= -200)
                            {
                                if (this.Hitbox.X <= origin - 200)
                                {
                                    left = false;
                                    compt = 1;
                                }
                                else if (this.Hitbox.X >= origin + 200)
                                {
                                    left = true;
                                    compt = 1;
                                }
                            }
                }
                if (left)
                {
                    if (!collisions.CollisionLeft(Hitbox, walls, speed) && Hitbox.X > 0 && collisions.CollisionDown(new Rectangle(Hitbox.X - Hitbox.Width, Hitbox.Y, Hitbox.Width, Hitbox.Height), walls, speed))
                    {
                        this.Hitbox.X -= speed;
                        this.Direction = Direction.Left;
                    }
                    else
                    {
                        framecolumn = 1;
                        compt += 10;
                    }
                }

                if (!left)
                {
                    if (!collisions.CollisionRight(Hitbox, walls, speed) && collisions.CollisionDown(new Rectangle(Hitbox.X + Hitbox.Width, Hitbox.Y, Hitbox.Width, Hitbox.Height), walls, speed))
                    {
                        this.Hitbox.X += speed;
                        this.Direction = Direction.Right;
                    }
                    else
                    {
                        framecolumn = 1;
                        compt += 10;
                    }
                }
            }


            #endregion

            switch (Direction)
            {
                case Direction.Right:
                    effect = SpriteEffects.FlipHorizontally;
                    break;
                case Direction.Left:
                    effect = SpriteEffects.None;
                    break;
            }
            switch (pv)
            {
                case 1:
                    color.A = 100;
                    break;
                case 2:
                    color.A = 200;
                    break;
                case 3:
                    color = Color.White;
                    break;
            }
        }


        public void Draw(SpriteBatch spritebatch)
        {
            if (!isDead2)
                spritebatch.Draw(Resources.Elite, Hitbox, new Rectangle((framecolumn - 1) * 30, 0, Hitbox.Width, Hitbox.Height), color, 0f, new Vector2(0, 0), effect, 0f);
        }
    }
}