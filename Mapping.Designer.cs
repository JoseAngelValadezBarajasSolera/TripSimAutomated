using System.Windows.Forms;

namespace Trip_Simulator
{
    partial class Map
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        // Overrides default close button and make it greyed out.
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams parms = base.CreateParams;
        //        parms.ClassStyle |= 0x200;  // CS_NOCLOSE
        //        return parms;
        //    }
        //}

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent(MainForm main)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Map));
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.EventMapUserControl = new System.Windows.Forms.Integration.ElementHost();
            this.mapUserControl1 = new Trip_Simulator.MapUserControl(main);
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(30, 31);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScrollBarsEnabled = false;
            this.webBrowser1.Size = new System.Drawing.Size(1552, 1029);
            this.webBrowser1.TabIndex = 0;
            // 
            // EventMapUserControl
            // 
            this.EventMapUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EventMapUserControl.Location = new System.Drawing.Point(0, 0);
            this.EventMapUserControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.EventMapUserControl.Name = "EventMapUserControl";
            this.EventMapUserControl.Size = new System.Drawing.Size(1552, 1029);
            this.EventMapUserControl.TabIndex = 4;
            this.EventMapUserControl.Text = "elementHost2";
            this.EventMapUserControl.Child = this.mapUserControl1;
            // 
            // Map
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1552, 1029);
            this.Controls.Add(this.EventMapUserControl);
            this.Controls.Add(this.webBrowser1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Map";
            this.Text = "Map";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Map_FormClosed);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Integration.ElementHost EventMapUserControl;
        private MapUserControl mapUserControl1;
    }
}