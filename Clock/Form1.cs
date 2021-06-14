using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clock
{
    public partial class Form1 : Form
    {
        bool widthImages = false;
        int size, sizeExtra;
        DateTime needDate;
        Image imgClock = Image.FromFile("clock.jpg");
        Image imgBack = Image.FromFile("back.jpg");
        DateTime dateNewYear = new DateTime(2017, 1, 1, 00, 00, 00);

        public Form1()
        {
            InitializeComponent();

            dateTimePicker1.Value = dateNewYear;

            Timer timerRefresh = new Timer();
            timerRefresh.Interval = 1000;
            timerRefresh.Tick += Timer_Tick;
            timerRefresh.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            size = Math.Min(ClientRectangle.Width, ClientRectangle.Height) / 2 - 70;

            if (widthImages)
            {
                Bitmap bmp = new Bitmap(imgBack, Size);
                BackgroundImage = bmp;
            }
            else
            {
                BackgroundImage = null;
            }

            PaintClock(e.Graphics);
            ShowTime(e.Graphics);

            panel1.Invalidate();

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            sizeExtra = Math.Min(panel1.Width, panel1.Height) / 2 - 10;
            if (widthImages)
            {
                Bitmap bmp = new Bitmap(imgBack, panel1.Size);
                panel1.BackgroundImage = bmp;
            }
            else
            {
                panel1.BackgroundImage = null;
            }
            PaintExtraClock(e.Graphics);
            ShowExtraTime(e.Graphics);
        }

        private void ShowExtraTime(Graphics g)
        {

            Pen arrowHour = new Pen(Color.DarkTurquoise, 3);
            Pen arrowMinute = new Pen(Color.Turquoise, 2);

            arrowHour.StartCap = LineCap.Round;
            arrowMinute.StartCap = LineCap.Round;
            arrowHour.EndCap = LineCap.ArrowAnchor;
            arrowMinute.EndCap = LineCap.ArrowAnchor;
            TimeSpan difference = needDate - DateTime.Now;
            float angle = (difference.Hours > 12 ? difference.Hours - 12 : difference.Hours) * 30;

            g.RotateTransform(angle);
            g.DrawLine(arrowHour, 0, 0, sizeExtra / 2, 0);
            g.RotateTransform(-angle);

            angle = difference.Minutes * 6;
            g.RotateTransform(angle);
            g.DrawLine(arrowMinute, 0, 0, (int)(sizeExtra / 1.5), 0);
            g.RotateTransform(-angle);

            label1.Text = difference.Days > 0 ? $"Осталось дней: {difference.Days}" : "";
        }

        private void PaintExtraClock(Graphics g)
        {
            Matrix m = g.Transform;
            Rectangle rect = new Rectangle(-sizeExtra, -sizeExtra, sizeExtra * 2, sizeExtra * 2);
            m.Translate(panel1.Width / 2f, panel1.Height / 2f);
            m.Rotate(-90);
            g.Transform = m;

            if (widthImages)
            {
                Bitmap bmp = new Bitmap(imgClock, rect.Size);
                TextureBrush brush = new TextureBrush(bmp);
                brush.TranslateTransform(-sizeExtra, -sizeExtra);
                brush.RotateTransform(90);
                g.FillEllipse(brush, rect);
            }
            else
            {
                g.FillEllipse(Brushes.Azure, rect);
            }

            Pen pentStrokes = new Pen(Color.MediumTurquoise);
            pentStrokes.EndCap = LineCap.Round;
            pentStrokes.StartCap = LineCap.Round;

            for (int i = 0; i < 360; i += 30)
            {
                if (i % 30 == 0)
                {
                    int x = sizeExtra;
                    pentStrokes.Width = 3;
                    pentStrokes.Color = Color.Turquoise;
                    g.DrawLine(pentStrokes, x - 1, 0, x + 1, 0);

                }
                g.RotateTransform(30);
            }
        }

        private void ShowTime(Graphics g)
        {

            Pen arrowHour = new Pen(Color.PaleTurquoise, 5);
            Pen arrowMinute = new Pen(Color.PaleTurquoise, 3);
            Pen arrowSecond = new Pen(Color.Turquoise, 2);

            arrowHour.StartCap = LineCap.Round;
            arrowMinute.StartCap = LineCap.Round;
            arrowSecond.StartCap = LineCap.Round;
            arrowHour.EndCap = LineCap.ArrowAnchor;
            arrowMinute.EndCap = LineCap.ArrowAnchor;
            arrowSecond.EndCap = LineCap.ArrowAnchor;

            float angle = (DateTime.Now.Hour > 12 ? DateTime.Now.Hour - 12 : DateTime.Now.Hour) * 30;

            g.RotateTransform(angle);
            g.DrawLine(arrowHour, 0, 0, size / 2, 0);
            g.RotateTransform(-angle);

            angle = DateTime.Now.Minute * 6;
            g.RotateTransform(angle);
            g.DrawLine(arrowMinute, 0, 0, (int)(size / 1.5), 0);
            g.RotateTransform(-angle);

            angle = DateTime.Now.Second * 6;
            g.RotateTransform(angle);
            g.DrawLine(arrowSecond, -20, 0, size, 0);
            g.RotateTransform(-angle);
        }

        private void PaintClock(Graphics g)
        {
            Matrix m = g.Transform;
            Rectangle rect = new Rectangle(-size, -size, size * 2, size * 2);
            m.Translate(ClientRectangle.Width / 2f, ClientRectangle.Height / 2f);
            m.Rotate(-90);
            g.Transform = m;
            if (widthImages)
            {
                Bitmap bmp = new Bitmap(imgClock, rect.Size);
                TextureBrush brush = new TextureBrush(bmp);
                brush.TranslateTransform(-size, -size);
                brush.RotateTransform(90);
                g.FillEllipse(brush, rect);
            }
            else
            {
                g.FillEllipse(Brushes.Azure, rect);
            }
            //g.DrawEllipse(Pens.Black, rect);
            Pen pentStrokes = new Pen(Color.MediumTurquoise);
            pentStrokes.EndCap = LineCap.Round;
            pentStrokes.StartCap = LineCap.Round;

            for (int i = 0; i < 360; i++)
            {
                if (i % 30 == 0)
                {
                    int x = size;
                    pentStrokes.Width = 3;
                    pentStrokes.Color = Color.PaleTurquoise;
                    g.DrawLine(pentStrokes, x - 10, 0, x + 10, 0);
                    g.DrawString((i == 0 ? 360 : i) / 30 + "", new Font("Courier", size / 12), Brushes.MediumTurquoise, x, -15, new StringFormat(StringFormatFlags.DirectionVertical));
                }
                else if (i % 6 == 0)
                {
                    pentStrokes.Width = 1;
                    pentStrokes.Color = Color.Cyan;
                    g.DrawLine(pentStrokes, size - 5, 0, size + 5, 0);
                }
                else
                {
                    pentStrokes.Width = 1;
                    pentStrokes.Color = Color.LightGreen;
                    g.DrawLine(pentStrokes, size - 2, 0, size + 2, 0);
                }
                g.RotateTransform(1);
            }

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

            needDate = dateTimePicker1.Value;
            Invalidate();
        }

        private void widthImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            widthImagesToolStripMenuItem.Checked = !widthImagesToolStripMenuItem.Checked;
            widthImages = widthImagesToolStripMenuItem.Checked;
            Invalidate();
        }

        private void visibleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            visibleToolStripMenuItem.Checked = !visibleToolStripMenuItem.Checked;
            panel1.Visible = visibleToolStripMenuItem.Checked;
        }

        private void selectImageBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = ("Картинки|*.bmp;*.jpg;*.jpeg;*.png");
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                imgBack = Image.FromFile(dlg.FileName);
                Invalidate();
            }
        }

        private void selectImageClockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = ("Картинки|*.bmp;*.jpg;*.jpeg;*.png");
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                imgClock = Image.FromFile(dlg.FileName);
                Invalidate();
            }
        }

        private void ownDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ownDateToolStripMenuItem.Checked = !ownDateToolStripMenuItem.Checked;
            dateTimePicker1.Visible = ownDateToolStripMenuItem.Checked;
            if (!dateTimePicker1.Visible)
            {
                dateTimePicker1.Value = dateNewYear;
            }
        }
    }
}
