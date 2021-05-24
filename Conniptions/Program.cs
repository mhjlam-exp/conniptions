using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace Conniptions
{
    public class Program
    {
        public class Conniptions : Form
        {
            [STAThread]
            public static void Main(string[] args)
            {
                Application.Run(new Conniptions());
            }

            private int remaining;

            private Timer soundTimer;
            private Timer updateTimer;

            private Random random;
            private NotifyIcon icon;
            private SoundPlayer clip;


            public Conniptions()
            {
                clip = new SoundPlayer(Properties.Resources.Conniptions);
                random = new Random(DateTime.Now.Millisecond);

                soundTimer = new Timer();
                soundTimer.Tick += new EventHandler(OnSound);
                soundTimer.Interval = random.Next(10000, 100000);

                remaining = soundTimer.Interval;

                updateTimer = new Timer();
                updateTimer.Tick += new EventHandler(OnUpdate);
                updateTimer.Interval = 1000;

                ContextMenu menu;
                menu = new ContextMenu();
                menu.MenuItems.Add("Conniptions", OnSound);
                menu.MenuItems.Add("Exit", OnExit);

                icon = new NotifyIcon();
                icon.Icon = new Icon(Properties.Resources.Collapse, 40, 40);

                icon.ContextMenu = menu;
                icon.Visible = true;
            }

            protected override void OnLoad(EventArgs e)
            {
                Visible = false;
                ShowInTaskbar = false;

                base.OnLoad(e);

                soundTimer.Start();
                updateTimer.Start();
            }
            
            private void OnUpdate(object sender, EventArgs e)
            {
                remaining -= updateTimer.Interval;

                int min = remaining / 60000;
                int sec = (remaining - min * 60000) / 1000;

                icon.Text = $"Next conniption in {min:D2}:{sec:D2}...";
            }

            private void OnSound(object sender, EventArgs e)
            {
                soundTimer.Stop();
                clip.Play();
                soundTimer.Interval = random.Next(10000, 100000);
                remaining = soundTimer.Interval;
                soundTimer.Start();
            }

            private void OnExit(object sender, EventArgs e)
            {
                Application.Exit();
            }

            protected override void Dispose(bool isDisposing)
            {
                icon.Dispose();
                base.Dispose(true);
            }
        }
    }
}
