﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ParticleSystem
{
    
    public class Emitter
    {
        public int X; // координата X центра эмиттера, будем ее использовать вместо MousePositionX
        public int Y; // соответствующая координата Y 
        public int Direction = 0; // вектор направления в градусах куда сыпет эмиттер
        public int Spreading = 360; // разброс частиц относительно Direction
        public int SpeedMin = 1; // начальная минимальная скорость движения частицы
        public int SpeedMax = 10; // начальная максимальная скорость движения частицы
        public int RadiusMin = 2; // минимальный радиус частицы
        public int RadiusMax = 10; // максимальный радиус частицы
        public int LifeMin = 20; // минимальное время жизни частицы
        public int LifeMax = 100; // максимальное время жизни частицы
        public int ParticlesCount = 500;
        public bool a = true;
        public Color ColorFrom = Color.White; // начальный цвет частицы
        public Color ColorTo = Color.FromArgb(0, Color.Black); // конечный цвет частиц

        List<ParticleColorful> particles = new List<ParticleColorful>();

        public int MousePositionX;
        public int MousePositionY;
        
        public float GravitationX = 0;
        public float GravitationY = 1;
        
        public int ParticlesPerTick = 1;

        public List<IImpactPoint> impactPoints = new List<IImpactPoint>();
        public virtual ParticleColorful CreateParticle()
        {
            var particle = new ParticleColorful();
            particle.FromColor = ColorFrom;
            particle.ToColor = ColorTo;

            return particle;
        }
        public virtual void ResetParticle(ParticleColorful particle)
        {
                particle.FromColor = Color.White;
                //particle = new ParticleColorful();
                particle.Life = Particle.rand.Next(LifeMin, LifeMax);

                particle.X = X;
                particle.Y = Y;

                var direction = Direction
                    + (double)Particle.rand.Next(Spreading)
                    - Spreading / 2;

                var speed = Particle.rand.Next(SpeedMin, SpeedMax);

                particle.SpeedX = (float)(Math.Cos(direction / 180 * Math.PI) * speed);
                particle.SpeedY = -(float)(Math.Sin(direction / 180 * Math.PI) * speed);

                particle.Radius = Particle.rand.Next(RadiusMin, RadiusMax);
        }

        public void UpdateIm()
        {
            foreach (var particle in particles)
            {
                foreach (var point in impactPoints)
                {
                    point.ImpactParticle(particle);
                }
            }
        }
        public void UpdateState()
        {
                int particlesToCreate = ParticlesPerTick;

                foreach (var particle in particles)
                {
                    if (particle.Life <= 0)
                    {
                        if (particlesToCreate > 0)
                        {
                            particlesToCreate -= 1;
                            ResetParticle(particle);
                        }
                    }
                    else
                    {
                        particle.Life -= 1;
                        foreach (var point in impactPoints)
                        {
                            point.ImpactParticle(particle);
                        }

                        particle.SpeedX += GravitationX;
                        particle.SpeedY += GravitationY;

                        particle.X += particle.SpeedX;
                        particle.Y += particle.SpeedY;
                    }
                }

                while (particlesToCreate >= 1)
                {
                    particlesToCreate -= 1;
                    var particle = CreateParticle();
                    ResetParticle(particle);
                    particles.Add(particle);
                }
        }

        public void Render(Graphics g)
        {
            // утащили сюда отрисовку частиц
            foreach (var particle in particles)
            {
                particle.Draw(g);
            }

            foreach (var point in impactPoints)
            {
                point.Render(g);
            }
        }
    }

    public class TopEmitter : Emitter
    {
        public int Width; // длина экрана

        public override void ResetParticle(ParticleColorful particle)
        {
            //particle.Life -= 1;
            base.ResetParticle(particle); // вызываем базовый сброс частицы, там жизнь переопределяется и все такое

            // а теперь тут уже подкручиваем параметры движения
            particle.X = Particle.rand.Next(Width); // позиция X -- произвольная точка от 0 до Width
            particle.Y = 0;  // ноль -- это верх экрана 
            
            particle.SpeedY = 10; // падаем вниз по умолчанию
            particle.SpeedX = Particle.rand.Next(-2, 2); // разброс влево и вправо у частиц 

        }
    }
}
