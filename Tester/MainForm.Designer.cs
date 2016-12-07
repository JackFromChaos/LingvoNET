namespace Tester
{
    partial class MainForm
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
            this.btFindAll = new System.Windows.Forms.Button();
            this.tbWord = new System.Windows.Forms.TextBox();
            this.btFindSimilar = new System.Windows.Forms.Button();
            this.cbGender = new System.Windows.Forms.ComboBox();
            this.cbAnimacy = new System.Windows.Forms.ComboBox();
            this.lbGender = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbComparability = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbAspect = new System.Windows.Forms.ComboBox();
            this.tbRes = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btFindAll
            // 
            this.btFindAll.Location = new System.Drawing.Point(683, 4);
            this.btFindAll.Name = "btFindAll";
            this.btFindAll.Size = new System.Drawing.Size(75, 23);
            this.btFindAll.TabIndex = 2;
            this.btFindAll.Text = "Find";
            this.btFindAll.UseVisualStyleBackColor = true;
            this.btFindAll.Click += new System.EventHandler(this.btFindAll_Click);
            // 
            // tbWord
            // 
            this.tbWord.Location = new System.Drawing.Point(14, 37);
            this.tbWord.Name = "tbWord";
            this.tbWord.Size = new System.Drawing.Size(136, 20);
            this.tbWord.TabIndex = 0;
            this.tbWord.Text = "рисовать";
            this.tbWord.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbWord_KeyDown);
            // 
            // btFindSimilar
            // 
            this.btFindSimilar.Location = new System.Drawing.Point(683, 33);
            this.btFindSimilar.Name = "btFindSimilar";
            this.btFindSimilar.Size = new System.Drawing.Size(75, 23);
            this.btFindSimilar.TabIndex = 6;
            this.btFindSimilar.Text = "FindSimilar";
            this.btFindSimilar.UseVisualStyleBackColor = true;
            this.btFindSimilar.Click += new System.EventHandler(this.btFindSimilar_Click);
            // 
            // cbGender
            // 
            this.cbGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGender.FormattingEnabled = true;
            this.cbGender.Location = new System.Drawing.Point(157, 35);
            this.cbGender.Name = "cbGender";
            this.cbGender.Size = new System.Drawing.Size(121, 21);
            this.cbGender.TabIndex = 7;
            // 
            // cbAnimacy
            // 
            this.cbAnimacy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAnimacy.FormattingEnabled = true;
            this.cbAnimacy.Location = new System.Drawing.Point(284, 36);
            this.cbAnimacy.Name = "cbAnimacy";
            this.cbAnimacy.Size = new System.Drawing.Size(121, 21);
            this.cbAnimacy.TabIndex = 8;
            // 
            // lbGender
            // 
            this.lbGender.AutoSize = true;
            this.lbGender.Location = new System.Drawing.Point(154, 19);
            this.lbGender.Name = "lbGender";
            this.lbGender.Size = new System.Drawing.Size(42, 13);
            this.lbGender.TabIndex = 9;
            this.lbGender.Text = "Gender";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(281, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Animacy";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Word";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(408, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Adj Comparability";
            // 
            // cbComparability
            // 
            this.cbComparability.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbComparability.FormattingEnabled = true;
            this.cbComparability.Location = new System.Drawing.Point(411, 35);
            this.cbComparability.Name = "cbComparability";
            this.cbComparability.Size = new System.Drawing.Size(121, 21);
            this.cbComparability.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(535, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Verb Aspect";
            // 
            // cbAspect
            // 
            this.cbAspect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAspect.FormattingEnabled = true;
            this.cbAspect.Location = new System.Drawing.Point(538, 35);
            this.cbAspect.Name = "cbAspect";
            this.cbAspect.Size = new System.Drawing.Size(121, 21);
            this.cbAspect.TabIndex = 14;
            // 
            // tbRes
            // 
            this.tbRes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbRes.BackColor = System.Drawing.Color.White;
            this.tbRes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbRes.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbRes.Location = new System.Drawing.Point(12, 63);
            this.tbRes.Name = "tbRes";
            this.tbRes.ReadOnly = true;
            this.tbRes.Size = new System.Drawing.Size(765, 415);
            this.tbRes.TabIndex = 16;
            this.tbRes.Text = "";
            this.tbRes.WordWrap = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 490);
            this.Controls.Add(this.tbRes);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbAspect);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbComparability);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbGender);
            this.Controls.Add(this.cbAnimacy);
            this.Controls.Add(this.cbGender);
            this.Controls.Add(this.btFindSimilar);
            this.Controls.Add(this.tbWord);
            this.Controls.Add(this.btFindAll);
            this.Name = "MainForm";
            this.Text = "Tester - LingvoNET";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btFindAll;
        private System.Windows.Forms.TextBox tbWord;
        private System.Windows.Forms.Button btFindSimilar;
        private System.Windows.Forms.ComboBox cbGender;
        private System.Windows.Forms.ComboBox cbAnimacy;
        private System.Windows.Forms.Label lbGender;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbComparability;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbAspect;
        private System.Windows.Forms.RichTextBox tbRes;
    }
}

