using System.Diagnostics;
using System.Net.NetworkInformation;

namespace CheckVPN
{
	public partial class Form1 : Form
	{
		private bool isConnectedToVPN;
		private System.Windows.Forms.Timer timer;

		private bool IsConnectedToVPN()
		{
			foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
			{
				if (ni.OperationalStatus == OperationalStatus.Up)
				{
					string description = ni.Description.ToLower();
					if (description.Contains("vpn") || description.Contains("tap"))
					{
						return true;
						
					}
				}
			}
			return false;
		}

		private void DrawBackground(object sender, PaintEventArgs e)
		{
			using (Brush brush = new SolidBrush(isConnectedToVPN ? Color.Green : Color.Red))
			{
				e.Graphics.FillRectangle(brush, this.ClientRectangle);
			}
			label1.Text = isConnectedToVPN ? "VPN ENABLED" : "VPN DISABLED";
			
		}

		private void OnTimerTick(object sender, EventArgs e)
		{
			bool currentVPNStatus = IsConnectedToVPN();
			if (currentVPNStatus != isConnectedToVPN)
			{
				isConnectedToVPN = currentVPNStatus;
				this.Invalidate(); // Trigger a repaint only if the VPN status changes
			}
		}

		public Form1()
		{
			InitializeComponent();

			isConnectedToVPN = IsConnectedToVPN();

			this.Paint += new PaintEventHandler(DrawBackground);
			this.Size = new Size(300, 200);
			this.StartPosition = FormStartPosition.CenterScreen;
			this.BackColor = Color.White;
			this.TopMost = true; // Make the form always on top

			label1.Font = new Font("Arial", 26);

			timer = new System.Windows.Forms.Timer();
			timer.Interval = 1000; // Check every second
			timer.Tick += new EventHandler(OnTimerTick);
			timer.Start();
		}
	}
}
