namespace PhoneShop
{
    partial class KulthidaStore
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KulthidaStore));
            this.subscribe = new System.Windows.Forms.Button();
            this.Login = new System.Windows.Forms.Button();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.Admin = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.SuspendLayout();
            // 
            // subscribe
            // 
            this.subscribe.BackColor = System.Drawing.Color.DarkRed;
            this.subscribe.Font = new System.Drawing.Font("RSU", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.subscribe.Location = new System.Drawing.Point(513, 502);
            this.subscribe.Name = "subscribe";
            this.subscribe.Size = new System.Drawing.Size(204, 57);
            this.subscribe.TabIndex = 0;
            this.subscribe.Text = "สมัครสมาชิก";
            this.subscribe.UseVisualStyleBackColor = false;
            this.subscribe.Click += new System.EventHandler(this.button1_Click);
            // 
            // Login
            // 
            this.Login.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.Login.Font = new System.Drawing.Font("RSU", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Login.Location = new System.Drawing.Point(743, 502);
            this.Login.Name = "Login";
            this.Login.Size = new System.Drawing.Size(174, 57);
            this.Login.TabIndex = 2;
            this.Login.Text = "เข้าสู่ระบบ";
            this.Login.UseVisualStyleBackColor = false;
            this.Login.Click += new System.EventHandler(this.Login_Click);
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.BoldPanel;
            this.kryptonLabel1.Location = new System.Drawing.Point(911, 618);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(6, 2);
            this.kryptonLabel1.TabIndex = 3;
            this.kryptonLabel1.Values.Text = "";
            // 
            // Admin
            // 
            this.Admin.Location = new System.Drawing.Point(680, 565);
            this.Admin.Name = "Admin";
            this.Admin.Size = new System.Drawing.Size(107, 34);
            this.Admin.TabIndex = 5;
            this.Admin.Values.ImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.Admin.Values.Text = "พนักงาน";
            this.Admin.Click += new System.EventHandler(this.Admin_Click);
            // 
            // KulthidaStore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1100, 707);
            this.Controls.Add(this.Admin);
            this.Controls.Add(this.kryptonLabel1);
            this.Controls.Add(this.Login);
            this.Controls.Add(this.subscribe);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.AliceBlue;
            this.Name = "KulthidaStore";
            this.Text = "Kulthida Store";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button subscribe;
        private System.Windows.Forms.Button Login;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton Admin;
    }
}

