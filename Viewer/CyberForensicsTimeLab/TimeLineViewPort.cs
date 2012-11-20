using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace CyberForensicsTimeLabTest
{
    public partial class TimeLineViewPort : UserControl
    {
        private string[] list = {};
        private int selectedItem;
        private XmlDatabase db;
        private int pointerX = 200;
        private int pointerX2;

        private const int listStartY = 50;

        private decimal selectionPos;
        private decimal selectionPos2;

        DateTime selectedEventTimeStamp;

        private MarkAlignment markAlignment = MarkAlignment.none;

        public class ViewTimeSpan : System.EventArgs
        {
            private DateTime first;
            private DateTime last;

            public ViewTimeSpan(DateTime first, DateTime last)
            {
                this.first = first;
                this.last = last;
            }

            public DateTime FirstTimeStamp()
            {
                return first;
            }

            public DateTime LastTimeStamp()
            {
                return last;
            }
        }

        public delegate void TimeSpanChangedHandler(object sender, ViewTimeSpan timeSpan);
        public event TimeSpanChangedHandler TimeSpanChanged;

        public delegate void SelectionChangedHandler(object sender, ViewTimeSpan timeSpan);
        public event SelectionChangedHandler SelectionChanged;

        public TimeLineViewPort()
        {
            InitializeComponent();
        }

        public void SetDatabase(XmlDatabase db)
        {
            this.db = db;
            list = db.ActiveHandlers();
            AutoScrollMinSize = new Size(950, list.Length * 50 + 50);
            TimeSpanChanged(this, new ViewTimeSpan(FirstTimeStamp(), LastTimeStamp()));
            Invalidate();
        }

        public void SetSelectedEventTimestamp(DateTime selectedEventTimeStamp)
        {
            this.selectedEventTimeStamp = selectedEventTimeStamp;
            Invalidate();
        }

        private int positionFromTime(DateTime dateTime)
        {
            int width = AutoScrollMinSize.Width > this.Width ? AutoScrollMinSize.Width : this.Width;
            width -= 30;
            DateTime first = db.FirstTimeStamp();
            DateTime last = db.LastTimeStamp();
            try
            {
                if(dateTime >= first && dateTime <= last)
                    return (int)(((decimal)dateTime.Ticks - (decimal)first.Ticks) / ((decimal)last.Ticks - (decimal)first.Ticks) * (decimal)(width - 200) + (decimal)200);
                else
                    return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private void TimeLineViewPort_Paint(object sender, PaintEventArgs e)
        {
            int width = AutoScrollMinSize.Width > this.Width ? AutoScrollMinSize.Width : this.Width;
            int height = AutoScrollMinSize.Height > this.Height ? AutoScrollMinSize.Height : this.Height;
            width -= 30;
            Bitmap buffer = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(buffer);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);    

            Brush highlightBrush = new SolidBrush(Color.FromKnownColor(KnownColor.Highlight));
            for (int i = 0; i < list.Length; i++)
            {
                g.DrawLine(Pens.AliceBlue, 1, listStartY + i * 50, width - 2, listStartY + i * 50);
                if(i == selectedItem)
                    g.FillRectangle(highlightBrush, new Rectangle(0, listStartY + i * 50, width - 1, 50));
                

            }

            if (db == null)
            {
                e.Graphics.Clear(Color.AliceBlue);
                e.Graphics.DrawString("No case loaded", new Font("Arial", 8), Brushes.Black, new Point(50, listStartY + 50));
                return;
            }


            for (int c = 0; c < list.Length; c++)
            {
                Bitmap distributionImage = new Bitmap(750*2, 30);
                Graphics dg = Graphics.FromImage(distributionImage);

                dg.Clear(Color.FromArgb(192, 222, 222, 255));

                long[] distribution = db.GetDistribution(300, list[c]);
                
                long max = 0;
                for (int i = 0; i < 300; i++)
                {
                    if (distribution[i] != 0)
                    {
                        //distribution[i] = 1 + (long)(Math.Log10(distribution[i]));
                        if (distribution[i] > max)
                            max = distribution[i];
                    }
                }

                g.DrawString(list[c], new Font("Arial", 8), c == selectedItem ? Brushes.White : Brushes.Black, new Point(10, listStartY + 10 + 50 * c));
                
                if (max == 0)
                    continue;

                for (int i = 0; i < 300; i++)
                {
                    int value = (int)((decimal)25 * (decimal)distribution[i] / (decimal)max);
                    if (distribution[i] != 0)
                        value += 5;

                    dg.FillRectangle(Brushes.Red, new Rectangle(i * 5, 30 - value , 4, value));
                }

                g.DrawImage(distributionImage, 200, listStartY + 10 + 50 * c, width - 200, 30);

            }


            if (markAlignment == MarkAlignment.right)
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(64, 255, 128, 128)), new Rectangle(200, listStartY + 0, pointerX - 200, AutoScrollMinSize.Height));
                g.DrawLine(new Pen(Color.FromArgb(64, 255, 0, 0), 3), new Point(200, listStartY + 0), new Point(pointerX, listStartY + AutoScrollMinSize.Height));
                g.DrawLine(new Pen(Color.FromArgb(64, 255, 0, 0), 3), new Point(pointerX, listStartY + 0), new Point(200, listStartY + AutoScrollMinSize.Height));
            }
            if (markAlignment == MarkAlignment.left)
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(64, 255, 128, 128)), new Rectangle(pointerX, listStartY + 0, width - pointerX, AutoScrollMinSize.Height));
                g.DrawLine(new Pen(Color.FromArgb(64, 255, 0, 0), 3), new Point(pointerX, listStartY + 0), new Point(width, listStartY + AutoScrollMinSize.Height));
                g.DrawLine(new Pen(Color.FromArgb(64, 255, 0, 0), 3), new Point(width, listStartY + 0), new Point(pointerX, listStartY + AutoScrollMinSize.Height));
            }


            DateTime first = db.FirstTimeStamp();
            DateTime last = db.LastTimeStamp();
            TimeSpan span = last.Subtract(first);


            g.DrawLine(new Pen(Color.FromArgb(128, 0, 255, 0), 3), new Point(pointerX, listStartY + 0), new Point(pointerX, listStartY + AutoScrollMinSize.Height));
            long ticks = (long)((decimal)first.Ticks + (decimal)selectionPos * ((decimal)last.Ticks - (decimal)first.Ticks));
            g.DrawString(new DateTime(ticks).ToString(), new Font("Small Fonts", 6), Brushes.Black, new Point((int)pointerX + 4, listStartY + 12));

            if (pointerX != pointerX2)
            {
                g.DrawLine(new Pen(Color.FromArgb(128, 0, 255, 0), 3), new Point(pointerX2, listStartY + 0), new Point(pointerX2, listStartY + AutoScrollMinSize.Height));
                long ticks2 = (long)((decimal)first.Ticks + (decimal)selectionPos2 * ((decimal)last.Ticks - (decimal)first.Ticks));
                g.DrawString(new DateTime(ticks2).ToString(), new Font("Small Fonts", 6), Brushes.Black, new Point((int)pointerX2 + 4, listStartY + 22));

                g.FillRectangle(new SolidBrush(Color.FromArgb(64, 128, 128, 255)), new Rectangle(pointerX, listStartY + 0, pointerX2 - pointerX, AutoScrollMinSize.Height));

            }



            Color fromColor = Color.FromArgb(192, SystemColors.ButtonHighlight);
            Color toColor = Color.FromArgb(128, SystemColors.ButtonShadow);

            Brush linearGradientBrush = new LinearGradientBrush(new Rectangle(0, 0, width, listStartY), fromColor, toColor, 90f);
            g.FillRectangle(linearGradientBrush, new Rectangle(0, 0, width, listStartY));
            linearGradientBrush.Dispose();




            if ((last.Year - first.Year) < 60)
            {
                DateTime cursor = new DateTime(first.Year, 1, 1);
                g.DrawString("" + first.Year, new Font("Small Fonts", 6), Brushes.Black, new Point(4, 2));
                for (int i = 0; i <= (last.Year - first.Year); i++)
                {
                    int pos = positionFromTime(cursor);
                    if (pos != 0)
                    {
                        g.DrawLine(new Pen(Color.FromArgb(128, 150, 150, 150), 2), new Point(pos, 0), new Point(pos, listStartY + AutoScrollMinSize.Height));
                        g.DrawString("" + cursor.Year, new Font("Small Fonts", 6), Brushes.Black, new Point(pos + 4, 2));
                    }
                    cursor = cursor.AddYears(1);
                }
            }

            if (span.TotalDays / 30 < 60)
            {
                DateTime cursor = new DateTime(first.Year, first.Month, 1);
                g.DrawString("" + first.ToString("MMM"), new Font("Small Fonts", 6), Brushes.Black, new Point(4, 2 + 9));
                for (int i = 0; i <= span.TotalDays / 30 + 1; i++)
                {
                    int pos = positionFromTime(cursor);
                    if (pos != 0)
                    {
                        g.DrawLine(new Pen(Color.FromArgb(128, 150, 150, 150), 2), new Point(pos, 0), new Point(pos, listStartY + AutoScrollMinSize.Height));
                        g.DrawString(cursor.ToString("MMM"), new Font("Small Fonts", 6), Brushes.Black, new Point(pos + 4, 2 + 9));
                    }
                    cursor = cursor.AddMonths(1);
                }
            }

            if (span.TotalDays < 60)
            {
                DateTime cursor = new DateTime(first.Year, first.Month, first.Day);
                g.DrawString("" + first.Day, new Font("Small Fonts", 6), Brushes.Black, new Point(4, 2 + 9 + 9));
                for (int i = 0; i <= span.TotalDays + 1; i++)
                {
                    int pos = positionFromTime(cursor);
                    if (pos != 0)
                    {
                        g.DrawLine(new Pen(Color.FromArgb(128, 150, 150, 150), 2), new Point(pos, 0), new Point(pos, listStartY + AutoScrollMinSize.Height));
                        g.DrawString(cursor.ToString("dd"), new Font("Small Fonts", 6), Brushes.Black, new Point(pos + 4, 2 + 9 + 9));
                    }
                    cursor = cursor.AddDays(1);
                }
            }

            if (span.TotalHours < 60)
            {
                DateTime cursor = new DateTime(first.Year, first.Month, first.Day, first.Hour, 0, 0);
                g.DrawString("" + first.Hour, new Font("Small Fonts", 6), Brushes.Black, new Point(4, 2 + 9 + 9 + 9));
                for (int i = 0; i <= span.TotalHours + 1; i++)
                {
                    int pos = positionFromTime(cursor);
                    if (pos != 0)
                    {
                        g.DrawLine(new Pen(Color.FromArgb(128, 150, 150, 150), 2), new Point(pos, 0), new Point(pos, listStartY + AutoScrollMinSize.Height));
                        g.DrawString(cursor.ToString("HH"), new Font("Small Fonts", 6), Brushes.Black, new Point(pos + 4, 2 + 9 + 9 + 9));
                    }
                    cursor = cursor.AddHours(1);
                }
            }

            if (span.TotalMinutes < 60)
            {
                DateTime cursor = new DateTime(first.Year, first.Month, first.Day, first.Hour, first.Minute, 0);
                g.DrawString("" + first.Minute, new Font("Small Fonts", 6), Brushes.Black, new Point(4, 2 + 9 + 9 + 9 + 9));
                for (int i = 0; i <= span.TotalMinutes + 1; i++)
                {
                    int pos = positionFromTime(cursor);
                    if (pos != 0)
                    {
                        g.DrawLine(new Pen(Color.FromArgb(128, 150, 150, 150), 2), new Point(pos, 0), new Point(pos, listStartY + AutoScrollMinSize.Height));
                        g.DrawString(cursor.ToString("mm"), new Font("Small Fonts", 6), Brushes.Black, new Point(pos + 4, 2 + 9 + 9 + 9 + 9));
                    }
                    cursor = cursor.AddMinutes(1);
                }
            }

            if (selectedEventTimeStamp >= first && selectedEventTimeStamp <= last)
            {
                decimal selectedEventX = ((decimal)selectedEventTimeStamp.Ticks - (decimal)first.Ticks) / ((decimal)last.Ticks - (decimal)first.Ticks) * (decimal)(width - 200) + (decimal)200;
                g.DrawLine(new Pen(Color.FromArgb(128, 0, 0, 255), 2), new Point((int)selectedEventX, 0), new Point((int)selectedEventX, listStartY + AutoScrollMinSize.Height));
            }

            


            e.Graphics.DrawImage(buffer, AutoScrollPosition);
        }

        public DateTime FirstTimeStamp()
        {
            return db.FirstTimeStamp();
        }

        public DateTime LastTimeStamp()
        {
            return db.LastTimeStamp();
        }

        public void ZoomToSelection()
        {
            DateTime startDateTime = SelectionTimeStamp();
            DateTime endDateTime = SelectionTimeStampEnd();
            db.SetFirstTimeStamp(startDateTime);
            db.SetLastTimeStamp(endDateTime);
            TimeSpanChanged(this, new ViewTimeSpan(FirstTimeStamp(), LastTimeStamp()));
            Invalidate();
        }

        public void CropLeft()
        {
            db.SetFirstTimeStamp(SelectionTimeStamp());
            TimeSpanChanged(this, new ViewTimeSpan(FirstTimeStamp(), LastTimeStamp()));
            Invalidate();
        }

        public void CropRight()
        {
            db.SetLastTimeStamp(SelectionTimeStamp());
            TimeSpanChanged(this, new ViewTimeSpan(FirstTimeStamp(), LastTimeStamp()));
            Invalidate();
        }

        public void ResetCrop()
        {
            db.SetFirstTimeStamp(new DateTime(0));
            db.SetLastTimeStamp(new DateTime(0));
            TimeSpanChanged(this, new ViewTimeSpan(FirstTimeStamp(), LastTimeStamp()));
            Invalidate();
        }

        public enum MarkAlignment
        {
            none = 0,
            left = 1,
            right = 2
        };

        public void MarkCrop(MarkAlignment alignment)
        {
            markAlignment = alignment;
            Invalidate();
        }

        private void TimeLineViewPort_MouseHandlers(object sender, MouseEventArgs e)
        {

            if (db == null)
                return;

            if (e.Button == MouseButtons.None)
                return;

            int item = (e.Y - listStartY) / 50;

            if (e.Button == MouseButtons.Left && item == selectedItem && pointerX == e.X)
                return;

            if (e.Button == MouseButtons.Left)
            {
                selectedItem = item;
                pointerX = e.X;
                pointerX = Math.Max(200, pointerX);
                pointerX = Math.Min(this.Width - 30, pointerX);
                selectionPos = (decimal)(pointerX - 200) / (decimal)(this.Width - 200 - 30);
                SelectionChanged(this, new ViewTimeSpan(SelectionTimeStamp(), SelectionTimeStampEnd()));
            }
            if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Left)
            {
                pointerX2 = e.X;
                pointerX2 = Math.Max(200, pointerX2);
                pointerX2 = Math.Min(this.Width - 30, pointerX2);
                pointerX2 = Math.Max(pointerX, pointerX2);
                selectionPos2 = (decimal)(pointerX2 - 200) / (decimal)(this.Width - 200 - 30);
                SelectionChanged(this, new ViewTimeSpan(SelectionTimeStamp(), SelectionTimeStampEnd()));
            }

            Invalidate();
        }

        private DateTime SelectionTimeStamp()
        {
            DateTime firstTimeStamp = db.FirstTimeStamp();
            DateTime lastTimeStamp = db.LastTimeStamp();
            long ticks = (long)((decimal)firstTimeStamp.Ticks + (decimal)selectionPos * ((decimal)lastTimeStamp.Ticks - (decimal)firstTimeStamp.Ticks));

            return new DateTime(ticks);
        }

        private DateTime SelectionTimeStampEnd()
        {
            DateTime firstTimeStamp = db.FirstTimeStamp();
            DateTime lastTimeStamp = db.LastTimeStamp();
            long ticks = (long)((decimal)firstTimeStamp.Ticks + (decimal)selectionPos2 * ((decimal)lastTimeStamp.Ticks - (decimal)firstTimeStamp.Ticks));

            return new DateTime(ticks);
        }

        protected override void OnPaintBackground(PaintEventArgs e) { }

        private void TimeLineViewPort_Resize(object sender, EventArgs e)
        {
            this.AutoScrollMinSize = new Size(950, list.Length * 50 + 50);
        }

        private void TimeLineViewPort_Load(object sender, EventArgs e)
        {

        }

        private void TimeLineViewPort_SizeChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

    }
}
