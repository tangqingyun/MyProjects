namespace Site._360doc.WinTool
{
    partial class Main
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
            this.TbUrl = new System.Windows.Forms.TextBox();
            this.BtnSubmit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TbPageNum = new System.Windows.Forms.TextBox();
            this.TbContent = new System.Windows.Forms.TextBox();
            this.tbArtID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TbUrl
            // 
            this.TbUrl.Location = new System.Drawing.Point(93, 25);
            this.TbUrl.Name = "TbUrl";
            this.TbUrl.Size = new System.Drawing.Size(443, 21);
            this.TbUrl.TabIndex = 0;
            // 
            // BtnSubmit
            // 
            this.BtnSubmit.Location = new System.Drawing.Point(603, 33);
            this.BtnSubmit.Name = "BtnSubmit";
            this.BtnSubmit.Size = new System.Drawing.Size(81, 46);
            this.BtnSubmit.TabIndex = 1;
            this.BtnSubmit.Text = "一键更新";
            this.BtnSubmit.UseVisualStyleBackColor = true;
            this.BtnSubmit.Click += new System.EventHandler(this.BtnSubmit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "链接地址：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "更新页数：";
            // 
            // TbPageNum
            // 
            this.TbPageNum.Location = new System.Drawing.Point(93, 58);
            this.TbPageNum.Name = "TbPageNum";
            this.TbPageNum.Size = new System.Drawing.Size(71, 21);
            this.TbPageNum.TabIndex = 0;
            this.TbPageNum.Text = "1";
            // 
            // TbContent
            // 
            this.TbContent.Location = new System.Drawing.Point(93, 114);
            this.TbContent.MaxLength = 327670000;
            this.TbContent.Multiline = true;
            this.TbContent.Name = "TbContent";
            this.TbContent.Size = new System.Drawing.Size(592, 284);
            this.TbContent.TabIndex = 3;
            // 
            // tbArtID
            // 
            this.tbArtID.Location = new System.Drawing.Point(237, 58);
            this.tbArtID.Name = "tbArtID";
            this.tbArtID.Size = new System.Drawing.Size(76, 21);
            this.tbArtID.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(183, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "文章ID：";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 423);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbArtID);
            this.Controls.Add(this.TbContent);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnSubmit);
            this.Controls.Add(this.TbPageNum);
            this.Controls.Add(this.TbUrl);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "360doc工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TbUrl;
        private System.Windows.Forms.Button BtnSubmit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TbPageNum;
        private System.Windows.Forms.TextBox TbContent;
        private System.Windows.Forms.TextBox tbArtID;
        private System.Windows.Forms.Label label3;
    }
}

