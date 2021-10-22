
namespace Start_EAS_Trans
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.button34 = new System.Windows.Forms.Button();
            this.button35 = new System.Windows.Forms.Button();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.label29 = new System.Windows.Forms.Label();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.button36 = new System.Windows.Forms.Button();
            this.button29 = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.button37 = new System.Windows.Forms.Button();
            this.button31 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button34
            // 
            this.button34.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button34.Location = new System.Drawing.Point(253, 409);
            this.button34.Name = "button34";
            this.button34.Size = new System.Drawing.Size(198, 29);
            this.button34.TabIndex = 93;
            this.button34.Text = "Новая задача";
            this.button34.UseVisualStyleBackColor = true;
            this.button34.Click += new System.EventHandler(this.button34_Click);
            // 
            // button35
            // 
            this.button35.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button35.Location = new System.Drawing.Point(253, 438);
            this.button35.Name = "button35";
            this.button35.Size = new System.Drawing.Size(198, 29);
            this.button35.TabIndex = 94;
            this.button35.Text = "Автоматический режим";
            this.button35.UseVisualStyleBackColor = true;
            this.button35.Click += new System.EventHandler(this.button35_Click);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox3.Location = new System.Drawing.Point(254, 473);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(197, 21);
            this.checkBox3.TabIndex = 95;
            this.checkBox3.Text = "Считывание по умолчанию";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label29.Location = new System.Drawing.Point(254, 497);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(53, 19);
            this.label29.TabIndex = 96;
            this.label29.Text = "Папка:";
            // 
            // textBox9
            // 
            this.textBox9.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox9.Location = new System.Drawing.Point(306, 491);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(103, 25);
            this.textBox9.TabIndex = 97;
            // 
            // button36
            // 
            this.button36.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button36.Location = new System.Drawing.Point(413, 490);
            this.button36.Name = "button36";
            this.button36.Size = new System.Drawing.Size(39, 27);
            this.button36.TabIndex = 98;
            this.button36.Text = "...";
            this.button36.UseVisualStyleBackColor = true;
            this.button36.Click += new System.EventHandler(this.button36_Click);
            // 
            // button29
            // 
            this.button29.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button29.Location = new System.Drawing.Point(253, 353);
            this.button29.Name = "button29";
            this.button29.Size = new System.Drawing.Size(198, 29);
            this.button29.TabIndex = 99;
            this.button29.Text = "Add строку транс раб";
            this.button29.UseVisualStyleBackColor = true;
            this.button29.Click += new System.EventHandler(this.button29_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox2.Location = new System.Drawing.Point(254, 305);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(143, 23);
            this.checkBox2.TabIndex = 102;
            this.checkBox2.Text = "Запись в файл.txt";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged_1);
            // 
            // button37
            // 
            this.button37.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button37.Location = new System.Drawing.Point(253, 324);
            this.button37.Name = "button37";
            this.button37.Size = new System.Drawing.Size(119, 30);
            this.button37.TabIndex = 103;
            this.button37.Text = "Открыть файл";
            this.button37.UseVisualStyleBackColor = true;
            this.button37.Click += new System.EventHandler(this.button37_Click);
            // 
            // button31
            // 
            this.button31.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button31.Location = new System.Drawing.Point(373, 324);
            this.button31.Name = "button31";
            this.button31.Size = new System.Drawing.Size(78, 30);
            this.button31.TabIndex = 104;
            this.button31.Text = "Папку";
            this.button31.UseVisualStyleBackColor = true;
            this.button31.Click += new System.EventHandler(this.button31_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 536);
            this.Controls.Add(this.button31);
            this.Controls.Add(this.button37);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.button29);
            this.Controls.Add(this.button36);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.button35);
            this.Controls.Add(this.button34);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form3";
            this.Text = "Start_EAS_Trans v 1.3 automatic mode";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.Controls.SetChildIndex(this.button34, 0);
            this.Controls.SetChildIndex(this.button35, 0);
            this.Controls.SetChildIndex(this.checkBox3, 0);
            this.Controls.SetChildIndex(this.label29, 0);
            this.Controls.SetChildIndex(this.textBox9, 0);
            this.Controls.SetChildIndex(this.button36, 0);
            this.Controls.SetChildIndex(this.button29, 0);
            this.Controls.SetChildIndex(this.checkBox2, 0);
            this.Controls.SetChildIndex(this.button37, 0);
            this.Controls.SetChildIndex(this.button31, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button34;
        private System.Windows.Forms.Button button35;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Button button36;
        private System.Windows.Forms.Button button29;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Button button37;
        private System.Windows.Forms.Button button31;
    }
}