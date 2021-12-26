using System;
using System.Collections.Generic;
using System.Drawing;

namespace ParticleSystem
{
    public abstract class IImpactPoint
    {
        public float X;
        public float Y;
        public abstract void ImpactParticle(ParticleColorful particle);
        public virtual void Render(Graphics g)
        {
            g.FillEllipse(
                    new SolidBrush(Color.Red),
                    X - 5,
                    Y - 5,
                    10,
                    10
                );
        }
    }

    public class GravityPoint : IImpactPoint
    {
        public int Power = 100; // сила притяжения

        // а сюда по сути скопировали с минимальными правками то что было в UpdateState
        public override void ImpactParticle(ParticleColorful particle)
        {
            float gX = X - particle.X;
            float gY = Y - particle.Y;

            double r = Math.Sqrt(gX * gX + gY * gY); // считаем расстояние от центра точки до центра частицы
            if (r + particle.Radius < Power / 2) // если частица оказалось внутри окружности
            {
                // то притягиваем ее
                float r2 = (float)Math.Max(100, gX * gX + gY * gY);
                particle.SpeedX += gX * Power / r2;
                particle.SpeedY += gY * Power / r2;
            }
        }
        public override void Render(Graphics g)
        {
            // буду рисовать окружность с диаметром равным Power
            g.DrawEllipse(
                   new Pen(Color.Red),
                   X - Power / 2,
                   Y - Power / 2,
                   Power,
                   Power
               );

            var stringFormat = new StringFormat(); // создаем экземпляр класса
            stringFormat.Alignment = StringAlignment.Center; // выравнивание по горизонтали
            stringFormat.LineAlignment = StringAlignment.Center;

            var text = $"Я гравитон\nc силой {Power}";
            var font = new Font("Verdana", 10);
            var size = g.MeasureString(text, font);

            g.FillRectangle(
                new SolidBrush(Color.Red),
                X - size.Width / 2, // так как я выравнивал текст по центру то подложка должна быть центрирована относительно X,Y
                Y - size.Height / 2,
                size.Width,
                size.Height
            );
                g.DrawString(
                text,
                font,
                new SolidBrush(Color.White),
                X,
                Y,
                stringFormat
            );
        }
    }

    public class AntiGravityPoint : IImpactPoint
    {
        public int Power = 100; // сила отторжения

        // а сюда по сути скопировали с минимальными правками то что было в UpdateState
        public override void ImpactParticle(ParticleColorful particle)
        {
            float gX = X - particle.X;
            float gY = Y - particle.Y;
            float r2 = (float)Math.Max(100, gX * gX + gY * gY);

            particle.SpeedX -= gX * Power / r2; // тут минусики вместо плюсов
            particle.SpeedY -= gY * Power / r2; // и тут
        }
    }

    public class ColorPoint : IImpactPoint
    {
        public Color color;
        public override void Render(Graphics g)
        {
            // буду рисовать окружность с диаметром равным Power
            g.DrawEllipse(
                   new Pen(color),
                   X - 50,
                   Y - 50,
                   100,
                   100
               );
        }
        public override void ImpactParticle(ParticleColorful particle)
        {
            float gX = X - particle.X;
            float gY = Y - particle.Y;

            
            double r = Math.Sqrt(gX * gX + gY * gY); // считаем расстояние от центра точки до центра частицы
            //if (r + particle.Radius < 100) // если частица оказалось внутри окружности
            if (r<50)
            {
                // то притягиваем ее
                float r2 = (float)Math.Max(100, gX * gX + gY * gY);
               // particle.color = Color.Red;
                //particle.SpeedY = 0;
               // particle.SpeedX = 0;
                particle.FromColor = color;
                
            }
            else
            {
               // particle.FromColor = Color.White;
            }
        }
    }
}
