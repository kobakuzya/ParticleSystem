﻿using System;
using System.Drawing;

namespace ParticleSystem
{
    public class Particle
    {
        public int Radius; // радиус частицы
        public float X; // X координата положения частицы в пространстве
        public float Y; // Y координата положения частицы в пространстве

        public float SpeedX; // скорость перемещения по оси X
        public float SpeedY; // скорость перемещения по оси Y
        public float Life;
        
        // добавили генератор случайных чисел
        public static Random rand = new Random();

        // конструктор по умолчанию будет создавать рандомную частицу
        public Particle()
        {
            // генерируем произвольное направление и скорость
            var direction = (double)rand.Next(360);
            var speed = 1 + rand.Next(10);

            // рассчитываем вектор скорости
            SpeedX = (float)(Math.Cos(direction / 180 * Math.PI) * speed);
            SpeedY = -(float)(Math.Sin(direction / 180 * Math.PI) * speed);
            Radius = 2 + rand.Next(10);
            Life = 20 + rand.Next(100);
        }

        public void ShowInfo(Graphics g)
        {
            var stringFormat = new StringFormat(); // создаем экземпляр класса
            stringFormat.Alignment = StringAlignment.Center; // выравнивание по горизонтали
            stringFormat.LineAlignment = StringAlignment.Center;

            var text = $"X: {X}\nY: {Y}";

            var font = new Font("Verdana", 10);
            var size = g.MeasureString(text, font);

            g.FillRectangle(
                new SolidBrush(Color.Red),
                X - size.Width / 2,
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

        public virtual void Draw(Graphics g)
        {
            float k = Math.Min(1f, Life / 100); //коэффициент прозрачности по шкале от 0 до 1.0
            int alpha = (int)(k * 255);
            // создали кисть для рисования
            var color = Color.FromArgb(alpha, Color.Black);
            
            var b = new SolidBrush(color);
            // нарисовали залитый кружок радиусом Radius с центром в X, Y
            
            g.FillEllipse(b, X - Radius, Y - Radius, Radius * 2, Radius * 2);
            g.DrawLine(new Pen(Color.Black, 2), 0, 0, 5, 0);
            b.Dispose();
        }
    }

    public class ParticleColorful : Particle
    {
        // два новых поля под цвет начальный и конечный
        public Color FromColor;
        public Color ToColor;

        // для смеси цветов
        public static Color MixColor(Color color1, Color color2, float k)
        {
            return Color.FromArgb(
                (int)(color2.A * k + color1.A * (1 - k)),
                (int)(color2.R * k + color1.R * (1 - k)),
                (int)(color2.G * k + color1.G * (1 - k)),
                (int)(color2.B * k + color1.B * (1 - k))
            );
        }
        public override void Draw(Graphics g)
        {
            float k = Math.Min(1f, Life / 100);

            // так как k уменьшается от 1 до 0, то порядок цветов обратный
            var color = MixColor(ToColor, FromColor, k);
            var b = new SolidBrush(color);

            g.FillEllipse(b, X - Radius, Y - Radius, Radius * 2, Radius * 2);
            g.DrawLine(new Pen(Color.Green, 2), 0, 0, 5, 0);
            b.Dispose();
        }
    }
}
