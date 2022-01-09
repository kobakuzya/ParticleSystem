using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParticleSystem
{
    public partial class Form1 : Form
    {
        List<Emitter> emitters = new List<Emitter>();
        Emitter emitter;
        public Form1()
        {
            InitializeComponent();
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);

            emitter = new TopEmitter
            {
                Direction = 0,
                Spreading = 10,
                SpeedMin = 10,
                SpeedMax = 10,
                ColorFrom = Color.White,
                ColorTo = Color.FromArgb(0, Color.Black),
                ParticlesPerTick = 10,
                Width = picDisplay.Width,
                GravitationY = 0f
            };
            // ниже добавляю цветные круги
            emitter.impactPoints.Add(new ColorPoint
            {
                X = (float)(picDisplay.Width / 2 - 200),
                Y = picDisplay.Height / 2 - 25,
                color = Color.Cyan
            });

            emitter.impactPoints.Add(new ColorPoint
            {
                X = (float)(picDisplay.Width / 2 - 150),
                Y = picDisplay.Height / 2 - 20,
                color = Color.HotPink
            });

            emitter.impactPoints.Add(new ColorPoint
            {
                X = (float)(picDisplay.Width / 2 - 100),
                Y = picDisplay.Height / 2 - 15,
                color = Color.Green
            });

            emitter.impactPoints.Add(new ColorPoint
            {
                X = (float)(picDisplay.Width / 2 - 50),
                Y = picDisplay.Height / 2 - 10,
                color = Color.OrangeRed
            });

            emitter.impactPoints.Add(new ColorPoint
            {
                X = (float)(picDisplay.Width / 2),
                Y = picDisplay.Height / 2 - 5,
                color = Color.Yellow
            });

            emitter.impactPoints.Add(new ColorPoint
            {
                X = (float)(picDisplay.Width / 2 + 50),
                Y = picDisplay.Height / 2 - 10,
                color = Color.DarkBlue
            });

            emitter.impactPoints.Add(new ColorPoint
            {
                X = (float)(picDisplay.Width / 2 + 100),
                Y = picDisplay.Height / 2 - 15,
                color = Color.Red
            });

            emitter.impactPoints.Add(new ColorPoint
            {
                X = (float)(picDisplay.Width / 2 + 150),
                Y = picDisplay.Height / 2 - 20,
                color = Color.SaddleBrown
            });

            emitter.impactPoints.Add(new ColorPoint
            {
                X = (float)(picDisplay.Width / 2 + 200),
                Y = picDisplay.Height / 2 - 25,
                color = Color.Purple
            });
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            emitter.UpdateState(); // обновляем эмиттер

            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.Black);
                emitter.Render(g); // рендерим через эмиттер
            }

            picDisplay.Invalidate();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {         
            foreach (var p in emitter.impactPoints)
            {
                // если эмиттер цветной круг
                if (p is ColorPoint)
                {
                    p.Y += trackBar2.Value - p.Y; // меняем положение
                }
            }
        }
    }
}
