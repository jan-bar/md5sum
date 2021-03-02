
namespace md5sum
{
    partial class Md5Form
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Md5Form));
            this.TbShow = new System.Windows.Forms.TextBox();
            this.GbCalc = new System.Windows.Forms.GroupBox();
            this.CbUpLe = new System.Windows.Forms.CheckBox();
            this.CbSha256 = new System.Windows.Forms.CheckBox();
            this.CbCrc32 = new System.Windows.Forms.CheckBox();
            this.CbSha1 = new System.Windows.Forms.CheckBox();
            this.CbMd5 = new System.Windows.Forms.CheckBox();
            this.CbTime = new System.Windows.Forms.CheckBox();
            this.CbVersion = new System.Windows.Forms.CheckBox();
            this.LbRate = new System.Windows.Forms.Label();
            this.LbComplete = new System.Windows.Forms.Label();
            this.PbNow = new System.Windows.Forms.ProgressBar();
            this.PbAll = new System.Windows.Forms.ProgressBar();
            this.LbCmpare = new System.Windows.Forms.Label();
            this.TbInput = new System.Windows.Forms.TextBox();
            this.btnCmpare = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.saveAllFile = new System.Windows.Forms.SaveFileDialog();
            this.openAllFile = new System.Windows.Forms.OpenFileDialog();
            this.GbCalc.SuspendLayout();
            this.SuspendLayout();
            // 
            // TbShow
            // 
            this.TbShow.AllowDrop = true;
            this.TbShow.BackColor = System.Drawing.SystemColors.Window;
            this.TbShow.Location = new System.Drawing.Point(14, 14);
            this.TbShow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TbShow.Multiline = true;
            this.TbShow.Name = "TbShow";
            this.TbShow.ReadOnly = true;
            this.TbShow.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TbShow.Size = new System.Drawing.Size(843, 332);
            this.TbShow.TabIndex = 14;
            this.TbShow.WordWrap = false;
            this.TbShow.DragDrop += new System.Windows.Forms.DragEventHandler(this.TbShow_DragDrop);
            this.TbShow.DragEnter += new System.Windows.Forms.DragEventHandler(this.TbShow_DragEnter);
            this.TbShow.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TbShow_KeyDown);
            // 
            // GbCalc
            // 
            this.GbCalc.Controls.Add(this.CbUpLe);
            this.GbCalc.Controls.Add(this.CbSha256);
            this.GbCalc.Controls.Add(this.CbCrc32);
            this.GbCalc.Controls.Add(this.CbSha1);
            this.GbCalc.Controls.Add(this.CbMd5);
            this.GbCalc.Controls.Add(this.CbTime);
            this.GbCalc.Controls.Add(this.CbVersion);
            this.GbCalc.Location = new System.Drawing.Point(14, 352);
            this.GbCalc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GbCalc.Name = "GbCalc";
            this.GbCalc.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GbCalc.Size = new System.Drawing.Size(844, 67);
            this.GbCalc.TabIndex = 1;
            this.GbCalc.TabStop = false;
            this.GbCalc.Text = "常 用 配 置";
            // 
            // CbUpLe
            // 
            this.CbUpLe.AutoSize = true;
            this.CbUpLe.Location = new System.Drawing.Point(722, 29);
            this.CbUpLe.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CbUpLe.Name = "CbUpLe";
            this.CbUpLe.Size = new System.Drawing.Size(97, 22);
            this.CbUpLe.TabIndex = 7;
            this.CbUpLe.Text = "大写(&U)";
            this.CbUpLe.UseVisualStyleBackColor = true;
            // 
            // CbSha256
            // 
            this.CbSha256.AutoSize = true;
            this.CbSha256.Location = new System.Drawing.Point(464, 29);
            this.CbSha256.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CbSha256.Name = "CbSha256";
            this.CbSha256.Size = new System.Drawing.Size(115, 22);
            this.CbSha256.TabIndex = 5;
            this.CbSha256.Text = "SHA256(&F)";
            this.CbSha256.UseVisualStyleBackColor = true;
            // 
            // CbCrc32
            // 
            this.CbCrc32.AutoSize = true;
            this.CbCrc32.Location = new System.Drawing.Point(597, 29);
            this.CbCrc32.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CbCrc32.Name = "CbCrc32";
            this.CbCrc32.Size = new System.Drawing.Size(106, 22);
            this.CbCrc32.TabIndex = 6;
            this.CbCrc32.Text = "CRC32(&R)";
            this.CbCrc32.UseVisualStyleBackColor = true;
            // 
            // CbSha1
            // 
            this.CbSha1.AutoSize = true;
            this.CbSha1.Location = new System.Drawing.Point(348, 29);
            this.CbSha1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CbSha1.Name = "CbSha1";
            this.CbSha1.Size = new System.Drawing.Size(97, 22);
            this.CbSha1.TabIndex = 4;
            this.CbSha1.Text = "SHA1(&A)";
            this.CbSha1.UseVisualStyleBackColor = true;
            // 
            // CbMd5
            // 
            this.CbMd5.AutoSize = true;
            this.CbMd5.Location = new System.Drawing.Point(241, 29);
            this.CbMd5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CbMd5.Name = "CbMd5";
            this.CbMd5.Size = new System.Drawing.Size(88, 22);
            this.CbMd5.TabIndex = 3;
            this.CbMd5.Text = "MD5(&M)";
            this.CbMd5.UseVisualStyleBackColor = true;
            // 
            // CbTime
            // 
            this.CbTime.AutoSize = true;
            this.CbTime.Location = new System.Drawing.Point(127, 29);
            this.CbTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CbTime.Name = "CbTime";
            this.CbTime.Size = new System.Drawing.Size(97, 22);
            this.CbTime.TabIndex = 2;
            this.CbTime.Text = "时间(&T)";
            this.CbTime.UseVisualStyleBackColor = true;
            // 
            // CbVersion
            // 
            this.CbVersion.AutoSize = true;
            this.CbVersion.Location = new System.Drawing.Point(14, 29);
            this.CbVersion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CbVersion.Name = "CbVersion";
            this.CbVersion.Size = new System.Drawing.Size(97, 22);
            this.CbVersion.TabIndex = 1;
            this.CbVersion.Text = "版本(&V)";
            this.CbVersion.UseVisualStyleBackColor = true;
            // 
            // LbRate
            // 
            this.LbRate.AutoSize = true;
            this.LbRate.Location = new System.Drawing.Point(10, 426);
            this.LbRate.Name = "LbRate";
            this.LbRate.Size = new System.Drawing.Size(98, 18);
            this.LbRate.TabIndex = 2;
            this.LbRate.Text = "当前文件：";
            // 
            // LbComplete
            // 
            this.LbComplete.AutoSize = true;
            this.LbComplete.Location = new System.Drawing.Point(10, 462);
            this.LbComplete.Name = "LbComplete";
            this.LbComplete.Size = new System.Drawing.Size(98, 18);
            this.LbComplete.TabIndex = 3;
            this.LbComplete.Text = "全部文件：";
            // 
            // PbNow
            // 
            this.PbNow.Location = new System.Drawing.Point(109, 426);
            this.PbNow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PbNow.Name = "PbNow";
            this.PbNow.Size = new System.Drawing.Size(748, 18);
            this.PbNow.TabIndex = 6;
            // 
            // PbAll
            // 
            this.PbAll.Location = new System.Drawing.Point(109, 462);
            this.PbAll.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PbAll.Name = "PbAll";
            this.PbAll.Size = new System.Drawing.Size(748, 18);
            this.PbAll.TabIndex = 7;
            // 
            // LbCmpare
            // 
            this.LbCmpare.AutoSize = true;
            this.LbCmpare.Location = new System.Drawing.Point(10, 500);
            this.LbCmpare.Name = "LbCmpare";
            this.LbCmpare.Size = new System.Drawing.Size(170, 18);
            this.LbCmpare.TabIndex = 6;
            this.LbCmpare.Text = "输入待比对校验码：";
            // 
            // TbInput
            // 
            this.TbInput.Location = new System.Drawing.Point(177, 494);
            this.TbInput.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TbInput.Name = "TbInput";
            this.TbInput.Size = new System.Drawing.Size(555, 28);
            this.TbInput.TabIndex = 8;
            this.TbInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TbInput_KeyUp);
            // 
            // btnCmpare
            // 
            this.btnCmpare.Enabled = false;
            this.btnCmpare.Location = new System.Drawing.Point(739, 493);
            this.btnCmpare.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCmpare.Name = "btnCmpare";
            this.btnCmpare.Size = new System.Drawing.Size(118, 36);
            this.btnCmpare.TabIndex = 9;
            this.btnCmpare.Text = "比对(&O)";
            this.btnCmpare.UseVisualStyleBackColor = true;
            this.btnCmpare.Click += new System.EventHandler(this.BtnCmpare_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(14, 545);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(118, 36);
            this.btnBrowse.TabIndex = 0;
            this.btnBrowse.Text = "浏览(&B)";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Enabled = false;
            this.btnCopy.Location = new System.Drawing.Point(195, 545);
            this.btnCopy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(118, 36);
            this.btnCopy.TabIndex = 10;
            this.btnCopy.Text = "复制(&C)";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.BtnCopy_Click);
            // 
            // btnClear
            // 
            this.btnClear.Enabled = false;
            this.btnClear.Location = new System.Drawing.Point(376, 545);
            this.btnClear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(118, 36);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "清除(&E)";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(557, 545);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(118, 36);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "保存(&S)";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(738, 545);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(118, 36);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "关闭(&X)";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // saveAllFile
            // 
            this.saveAllFile.FileName = "md5sum.txt";
            this.saveAllFile.Filter = "txt file|*.txt";
            this.saveAllFile.RestoreDirectory = true;
            // 
            // openAllFile
            // 
            this.openAllFile.Filter = "All file|*.*";
            this.openAllFile.Multiselect = true;
            this.openAllFile.RestoreDirectory = true;
            // 
            // Md5Form
            // 
            this.AcceptButton = this.btnBrowse;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 590);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.btnCmpare);
            this.Controls.Add(this.TbInput);
            this.Controls.Add(this.LbCmpare);
            this.Controls.Add(this.PbAll);
            this.Controls.Add(this.PbNow);
            this.Controls.Add(this.LbComplete);
            this.Controls.Add(this.LbRate);
            this.Controls.Add(this.GbCalc);
            this.Controls.Add(this.TbShow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "Md5Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = Md5Form.Md5FormName;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Md5Form_FormClosed);
            this.Load += new System.EventHandler(this.Md5Form_Load);
            this.GbCalc.ResumeLayout(false);
            this.GbCalc.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TbShow;
        private System.Windows.Forms.GroupBox GbCalc;
        private System.Windows.Forms.CheckBox CbCrc32;
        private System.Windows.Forms.CheckBox CbSha1;
        private System.Windows.Forms.CheckBox CbMd5;
        private System.Windows.Forms.CheckBox CbTime;
        private System.Windows.Forms.CheckBox CbVersion;
        private System.Windows.Forms.Label LbRate;
        private System.Windows.Forms.Label LbComplete;
        private System.Windows.Forms.ProgressBar PbNow;
        private System.Windows.Forms.ProgressBar PbAll;
        private System.Windows.Forms.Label LbCmpare;
        private System.Windows.Forms.TextBox TbInput;
        private System.Windows.Forms.Button btnCmpare;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox CbUpLe;
        private System.Windows.Forms.CheckBox CbSha256;
        private System.Windows.Forms.SaveFileDialog saveAllFile;
        private System.Windows.Forms.OpenFileDialog openAllFile;
    }
}

