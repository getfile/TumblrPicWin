	using System.Windows.Forms;

partial class Form1
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.urlBox = new System.Windows.Forms.ComboBox();
			this.logBox = new System.Windows.Forms.TextBox();
			this.logBn = new System.Windows.Forms.Button();
			this.imgPanel = new System.Windows.Forms.Panel();
			this.imgBar = new System.Windows.Forms.VScrollBar();
			this.htmlBn = new System.Windows.Forms.Button();
			this.tt = new System.Windows.Forms.ToolTip(this.components);
			this.archiveBn = new System.Windows.Forms.Button();
			this.autoScrollBn = new System.Windows.Forms.Button();
			this.imgPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// urlBox
			// 
			this.urlBox.FormattingEnabled = true;
			this.urlBox.Location = new System.Drawing.Point(5, 5);
			this.urlBox.Name = "urlBox";
			this.urlBox.Size = new System.Drawing.Size(412, 20);
			this.urlBox.TabIndex = 0;
			this.urlBox.SelectedIndexChanged += new System.EventHandler(this.urlBox_SelectedIndexChanged);
			this.urlBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.urlBox_KeyUp);
			// 
			// logBox
			// 
			this.logBox.Location = new System.Drawing.Point(5, 278);
			this.logBox.Multiline = true;
			this.logBox.Name = "logBox";
			this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.logBox.Size = new System.Drawing.Size(542, 94);
			this.logBox.TabIndex = 3;
			// 
			// logBn
			// 
			this.logBn.Location = new System.Drawing.Point(525, 5);
			this.logBn.Name = "logBn";
			this.logBn.Size = new System.Drawing.Size(21, 21);
			this.logBn.TabIndex = 4;
			this.logBn.Text = "L";
			this.tt.SetToolTip(this.logBn, "加载log");
			this.logBn.UseVisualStyleBackColor = true;
			this.logBn.Click += new System.EventHandler(this.logBn_Click);
			// 
			// imgPanel
			// 
			this.imgPanel.Controls.Add(this.imgBar);
			this.imgPanel.Location = new System.Drawing.Point(5, 57);
			this.imgPanel.Name = "imgPanel";
			this.imgPanel.Size = new System.Drawing.Size(542, 204);
			this.imgPanel.TabIndex = 5;
			this.imgPanel.MouseEnter += new System.EventHandler(this.imgPanel_MouseEnter);
			this.imgPanel.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.imgPanel_MouseWheel);
			this.imgPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.imgPanel_MouseMiddelClick);
			// 
			// imgBar
			// 
			this.imgBar.Location = new System.Drawing.Point(525, 0);
			this.imgBar.Name = "imgBar";
			this.imgBar.Size = new System.Drawing.Size(16, 340);
			this.imgBar.TabIndex = 0;
			this.imgBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.imgBar_Scroll);
			this.imgBar.ValueChanged += new System.EventHandler(this.imgBar_ValueChanged);
			// 
			// htmlBn
			// 
			this.htmlBn.Location = new System.Drawing.Point(498, 5);
			this.htmlBn.Name = "htmlBn";
			this.htmlBn.Size = new System.Drawing.Size(21, 21);
			this.htmlBn.TabIndex = 6;
			this.htmlBn.Text = "H";
			this.tt.SetToolTip(this.htmlBn, "输出html");
			this.htmlBn.UseVisualStyleBackColor = true;
			this.htmlBn.Click += new System.EventHandler(this.htmlBn_Click);
			// 
			// archiveBn
			// 
			this.archiveBn.Location = new System.Drawing.Point(471, 5);
			this.archiveBn.Name = "archiveBn";
			this.archiveBn.Size = new System.Drawing.Size(21, 21);
			this.archiveBn.TabIndex = 7;
			this.archiveBn.Text = "A";
			this.tt.SetToolTip(this.archiveBn, "打开archive网页");
			this.archiveBn.UseVisualStyleBackColor = true;
			this.archiveBn.Click += new System.EventHandler(this.archiveBn_Click);
			// 
			// autoScrollBn
			// 
			this.autoScrollBn.Location = new System.Drawing.Point(423, 5);
			this.autoScrollBn.Name = "autoScrollBn";
			this.autoScrollBn.Size = new System.Drawing.Size(42, 21);
			this.autoScrollBn.TabIndex = 8;
			this.autoScrollBn.Text = "关闭";
			this.tt.SetToolTip(this.autoScrollBn, "自动翻页");
			this.autoScrollBn.UseVisualStyleBackColor = true;
			this.autoScrollBn.Click += new System.EventHandler(this.autoScrollBn_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(555, 384);
			this.Controls.Add(this.autoScrollBn);
			this.Controls.Add(this.archiveBn);
			this.Controls.Add(this.urlBox);
			this.Controls.Add(this.htmlBn);
			this.Controls.Add(this.imgPanel);
			this.Controls.Add(this.logBn);
			this.Controls.Add(this.logBox);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Closed += new System.EventHandler(this.Form1_Closed);
			this.Resize += new System.EventHandler(this.Form1_Resize);
			this.imgPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox logBox;
		private System.Windows.Forms.Button logBn;
		private System.Windows.Forms.Panel imgPanel;
		private System.Windows.Forms.VScrollBar imgBar;
		private System.Windows.Forms.Button htmlBn;
		private ToolTip tt;
		private ComboBox urlBox;
		private Button archiveBn;
		private Button autoScrollBn;
	}

