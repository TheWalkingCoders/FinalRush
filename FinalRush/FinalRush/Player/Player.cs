﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Timers;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace FinalRush
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    };

    class Player
    {
        #region Variables

        // FIELDS       

        public List<Bullets> bullets;
        public Rectangle Hitbox;
        public Direction Direction;
        SpriteEffects effect;
        Collisions collisions = new Collisions();
        GameMain main;
        MainMenu Main;
        public Texture2D Marco = Resources.Marco;
        public Texture2D MarcoJump = Resources.MarcoSaut;
        public Color marco_color = new Color(254, 255, 255, 255);

        bool up,spaceIsPressed = true, change = true;
        int compteursaut = 0, compteur = 1;
        public int speed;
        public int health = 100;
        public Texture2D healthbar;
        int fallspeed;
        public int framecolumn;
        int speedjump = 1;
        bool hasjumped;
        float rotation;
        Direction Old_Dir;
        float distanceX, distanceY;
        MouseState mouse;
        public bool dead;
        public bool shot;
        public string state = "standing"; // Définition de l'état de Marco, afin de choisir les sprites qui vont animer le perso

        SpriteFont Scoring;
        SoundEffectInstance vide_sound_instance = Resources.ammo_vide.CreateInstance();
        SoundEffectInstance fire_sound_instance = Resources.coup_de_feu.CreateInstance();

        #endregion

        // CONSTRUCTORS

        #region Constructeur
        public Player()
        {
            bullets = new List<Bullets>();
            speed = 4;
            fallspeed = 5;
            Hitbox.Width = 36;
            Hitbox.Height = 40;
            Hitbox = new Rectangle(50, 376, Hitbox.Width, Hitbox.Height);
            framecolumn = 1;
            effect = SpriteEffects.None;
            Direction = Direction.Right;
            main = Global.GameMain;
            Main = new MainMenu(Global.Handler, 0f);
            distanceX = mouse.X - Hitbox.X;
            distanceY = mouse.Y - Hitbox.Y;
            rotation = (float)Math.Atan2(distanceY, distanceX);
            Global.Player = this;
        }
        #endregion

        // METHODS

        #region LoadContent

        public void LoadContent(ContentManager content)
        {
            Scoring = content.Load<SpriteFont>("TimerText");
            healthbar = Resources.HealthBar;
        }

        #endregion

        #region Animation

        public void Animate(int begin, int end, int speed)
        {
            if (compteur % speed == 0)
            {
                if (framecolumn > end - 1) framecolumn = begin;
                else framecolumn++;
            }
            compteur++;
        }
        #endregion

        // UPDATE & DRAW

        public void Update(MouseState souris, KeyboardState clavier, List<Wall> walls, List<Bonus> bonus)
        {
            if (Global.Collisions.collision_speed)
                speed = 6;
            else if (Global.Collisions.LowSpeed)
                speed = 2;
            else
                speed = 4;
            GameTime gametime;
            gametime = new GameTime();
            Main.Update(gametime);

            #region Bullets
            if (Keyboard.GetState().IsKeyDown(Keys.Q) && bullets.Count() < 3 && !shot && Global.Handler.ammo_left > 0 && state == "standing")
            {
                shot = true;
                Bullets bullet = new Bullets(Resources.bullet);
                bullet.velocity = 5;
                if (Direction == Direction.Right)
                {
                    bullet.position = new Vector2(Hitbox.X + Hitbox.Width / 2, Hitbox.Y + Hitbox.Height / 3) + new Vector2(bullet.velocity * 5, 0);
                    Old_Dir = Direction.Right;
                }
                else
                {
                    bullet.position = new Vector2(Hitbox.X, Hitbox.Y + Hitbox.Height / 3) + new Vector2(bullet.velocity * 5, 0);
                    Old_Dir = Direction.Left;
                }
                bullet.isVisible = true;
                bullets.Add(bullet);
            }
            else
                shot = false;
            foreach (Bullets bullet in bullets)
            {
                if (Old_Dir == Direction.Right)
                    bullet.position.X += bullet.velocity; // va vers la droite
                else
                    bullet.position.X -= bullet.velocity; // va vers la gauche

                // la balle disparait si elle parcourt la distance ou rencontre un obstacle
                if (Vector2.Distance(bullet.position, new Vector2(Hitbox.X, Hitbox.Y)) >= 200)
                    bullet.isVisible = false;
                foreach (Wall wall in walls)
                {
                    if (wall.Hitbox.Intersects(new Rectangle((int)bullet.position.X, (int)bullet.position.Y, 5, 2)))
                        bullet.isVisible = false;
                }
            }

            //on retire la balle de la liste si elle disparait
            for (int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i].isVisible)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }

            #endregion

            #region Gravité

            bool gravity = collisions.CollisionDown(Hitbox, walls, this.fallspeed);

            if (!gravity)
            {
                if (collisions.CollisionUp(Hitbox, walls, fallspeed))
                    speedjump = 0;
                if (hasjumped && speedjump > 1)
                {
                    this.Hitbox.Y -= speedjump;
                    speedjump -= 1;
                }
                else
                {
                    this.Hitbox.Y += this.fallspeed;
                    framecolumn = 1;
                    state = "jump";
                }
            }
            else
            {
                if (!collisions.CollisionDown(Hitbox, walls, this.fallspeed - 4))
                    if (!collisions.CollisionDown(Hitbox, walls, this.fallspeed - 2))
                        this.Hitbox.Y += 2;
                    else
                        this.Hitbox.Y++;
                if (state == "jump")
                    state = "standing";
            }

            #endregion

            #region Couteau

            if (clavier.IsKeyDown(Keys.D) && state == "standing")
            {

                SoundEffectInstance couteau_sound_instance = Resources.couteau.CreateInstance();
                couteau_sound_instance.Play();
                framecolumn = 1;
                compteur = 1;
                state = "cut";
                Hitbox.Width = 50;
                Hitbox.Y -= 16;
                Hitbox.Height = 55;
            }

            #endregion

            #region Déplacements

            //Le sprite et la hitbox accroupi quand le personnage est baissé ou coincé sous un mur
            if (clavier.IsKeyDown(Keys.Down))
            {
                Hitbox.Width = 35;
                Hitbox.Height = 25;
                state = "squat";
            }
            //Celui debout quand on se relève et qu'il y a suffisament de place
            if (clavier.IsKeyUp(Keys.Down) && Hitbox.Height == 25 && !collisions.CollisionUp(new Rectangle(Hitbox.X, Hitbox.Y - 15, Hitbox.Width, Hitbox.Height), walls, speed))
            {
                state = "standing";
                Hitbox.Width = 36;
                Hitbox.Height = 40;

                if (collisions.CollisionDown(new Rectangle(Hitbox.X, Hitbox.Y, Hitbox.Width, Hitbox.Height), walls, speed))
                {
                    Hitbox.Y = Hitbox.Y - 15;
                }
            }

            // Tir Marco
            if (clavier.IsKeyDown(Keys.Q) && state == "standing") //On tire avec "Q" et debout uniquement 
            {

                if (state != "shoot")
                {
                    framecolumn = 1;
                    compteur = 1;
                }

                if (Global.Handler.ammo_left > 0)
                {
                    fire_sound_instance.Play();
                    state = "shoot";
                }
                else
                    vide_sound_instance.Play();
            }
            if (state == "cut")
            {
                if (compteur % 2 == 0)
                {
                    if (framecolumn < 8)
                    {
                        framecolumn++;
                        compteur++;
                    }
                    else
                    {
                        framecolumn = 1;
                        if (clavier.IsKeyUp(Keys.D))
                        {
                            state = "standing";
                            Hitbox.Width = 36;
                            Hitbox.Height = 40;
                            Hitbox.Y += 15;
                        }
                    }
                }
                else
                    compteur++;
            }
            else
                if (state == "shoot")
                {
                    if (compteur % 4 == 0)
                    {
                        if (framecolumn < 10)
                        {
                            Hitbox.Width = 52;
                            Hitbox.Height = 40;
                            framecolumn++;
                        }
                        else
                        {
                            state = "standing";
                            Hitbox.Width = 36;
                            Hitbox.Height = 40;
                            framecolumn = 1;
                            compteur = 1;
                        }
                    }
                    compteur++;
                }
                else
                    if (clavier.IsKeyDown(Keys.Right) && state != "squat")          // Permet de bouger à droite
                    {
                        // Droite si pas de collisions et si le perso est encore sur l'écran
                        if (!collisions.CollisionRight(Hitbox, walls, speed))
                        {
                            this.Hitbox.X += speed;
                            this.Direction = Direction.Right;
                            if (collisions.CollisionDown(Hitbox, walls, speed))
                                this.Animate(1, 15, 2);
                        }
                        else
                        {
                            framecolumn = 1;
                            compteur = 1;
                        }
                    }

                    else if (clavier.IsKeyDown(Keys.Left) && state != "squat")    // Permet de bouger à gauche
                    {
                        // Gauche si pas de collisions et si le perso est encore sur l'écran
                        if (!collisions.CollisionLeft(Hitbox, walls, this.speed) && Hitbox.X > 0)
                        {
                            this.Hitbox.X -= this.speed;
                            this.Direction = Direction.Left;
                            if (collisions.CollisionDown(Hitbox, walls, this.speed))
                                this.Animate(1, 15, 2);
                        }
                        else
                        {
                            framecolumn = 1;
                            compteur = 1;
                        }
                    }

            if (clavier.IsKeyDown(Keys.Right) && state == "squat")          // Permet de bouger à droite accroupi
            {
                // Droite si pas de collisions et si le perso est encore sur l'écran
                if (!collisions.CollisionRight(Hitbox, walls, this.speed))
                {
                    this.Hitbox.X += this.speed - 1;
                    this.Direction = Direction.Right;
                    if (collisions.CollisionDown(Hitbox, walls, this.speed))
                        this.Animate(2, 8, 2);
                }
                else
                {
                    framecolumn = 1;
                    compteur = 1;
                }
            }

            else if (clavier.IsKeyDown(Keys.Left) && state == "squat")    // Permet de bouger à gauche accroupi
            {
                // Gauche si pas de collisions et si le perso est encore sur l'écran
                if (!collisions.CollisionLeft(Hitbox, walls, this.speed) && Hitbox.X > 0)
                {
                    this.Hitbox.X -= this.speed - 1;
                    this.Direction = Direction.Left;
                    if (collisions.CollisionDown(Hitbox, walls, this.speed))
                        this.Animate(2, 8, 2);
                }
                else
                {
                    framecolumn = 1;
                    compteur = 1;
                }
            }
            #endregion

            #region Déplacements test
            else if (clavier.IsKeyDown(Keys.A))
                Hitbox = new Rectangle(50, 370, Hitbox.Width, Hitbox.Height);
            else if (clavier.IsKeyDown(Keys.N))
                Hitbox = new Rectangle(4500, 100, Hitbox.Width, Hitbox.Height);
            #endregion

            #region Saut

            if (clavier.IsKeyDown(Keys.Space))
            {
                if (compteursaut == 1) spaceIsPressed = false;
                else
                {
                    spaceIsPressed = true;
                    compteursaut++;
                }
            }
            else compteursaut = 0;

            if (clavier.IsKeyDown(Keys.Space) && spaceIsPressed && !hasjumped && gravity && state == "standing")
            {
                framecolumn = 1;
                if (!collisions.CollisionUp(Hitbox, walls, this.fallspeed))
                {
                    SoundEffectInstance saut_instance = Resources.saut.CreateInstance();
                    saut_instance.Play();
                    this.Hitbox.Y -= 12;
                    speedjump = 12;
                }
                hasjumped = true;
                state = "jump";
            }

            if (hasjumped == true && collisions.CollisionDown(Hitbox, walls, this.fallspeed) == true && state != "squat")
            {
                hasjumped = false;
                state = "standing";
            }

            #endregion

            #region Effets et animation

            if (clavier.IsKeyUp(Keys.Right) && clavier.IsKeyUp(Keys.Left) && state == "standing")              // Ici c'est une partie de l'animation
            {
                framecolumn = 1;
            }

            switch (Direction)
            {
                case Direction.Left:
                    effect = SpriteEffects.FlipHorizontally;
                    break;
                case Direction.Right:
                    effect = SpriteEffects.None;
                    break;
                default:
                    break;

            }
            #endregion

            #region Chute

            dead = Hitbox.Y > 480;

            if (dead)
            {
                Hitbox = new Rectangle(50, 350, Hitbox.Width, Hitbox.Height);
            }

            #endregion
            if (Global.Collisions.accu > 0) change = false;
            if (!change && Global.Collisions.accu > 0)
            {
                if (marco_color.G < 20) up = true;
                if (marco_color.G > 235) up = false;
                if (up)
                {
                    marco_color.G += 15;
                    marco_color.B += 15;
                }
                else
                {
                    marco_color.G -= 15;
                    marco_color.B -= 15;
                }
                Global.Collisions.accu--;
            }
            else marco_color = new Color(254, 255, 255, 255);
        }


        public void Draw(SpriteBatch spritebatch)
        {
            foreach (Bullets bullet in bullets)
                bullet.Draw(spritebatch);
            //Affichage du joueur 
            switch (state) // Affichage des bons sprites en fonction de l'état, ça va être utile pour le tir, la mort, la prise d'un coup, switch arme etc.. 
            {
                case "standing":
                    spritebatch.Draw(Marco, Hitbox, new Rectangle((framecolumn - 1) * 36, 0, Hitbox.Width, Hitbox.Height), marco_color, 0f, new Vector2(0, 0), effect, 0f);
                    break;
                case "squat":
                    spritebatch.Draw(Resources.MarcoSquat, Hitbox, new Rectangle((framecolumn - 1) * 35, 0, Hitbox.Width, Hitbox.Height), marco_color, 0f, new Vector2(0, 0), effect, 0f);
                    break;
                case "jump":
                    spritebatch.Draw(Resources.MarcoSaut, Hitbox, new Rectangle((framecolumn - 1) * 36, 0, Hitbox.Width, Hitbox.Height), marco_color, 0f, new Vector2(0, 0), effect, 0f);
                    break;
                case "shoot":
                    spritebatch.Draw(Resources.MarcoTir, Hitbox, new Rectangle((framecolumn - 1) * 52, 0, Hitbox.Width, Hitbox.Height), marco_color, 0f, new Vector2(0, 0), effect, 0f);
                    break;
                case "cut":
                    spritebatch.Draw(Resources.MarcoCut, Hitbox, new Rectangle((framecolumn - 1) * 50, 0, Hitbox.Width, Hitbox.Height), marco_color, 0f, new Vector2(0, 0), effect, 0f);
                    break;
            }
        }
    }
}