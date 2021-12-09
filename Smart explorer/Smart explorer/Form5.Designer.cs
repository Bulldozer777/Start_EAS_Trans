
namespace Smart_explorer
{
    partial class Form5
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form5));
            this.button64 = new System.Windows.Forms.Button();
            this.button63 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button64
            // 
            this.button64.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button64.Location = new System.Drawing.Point(12, 39);
            this.button64.Name = "button64";
            this.button64.Size = new System.Drawing.Size(286, 28);
            this.button64.TabIndex = 111;
            this.button64.Text = "Телефоны работников УФПС";
            this.button64.UseVisualStyleBackColor = true;
            this.button64.Click += new System.EventHandler(this.button64_Click);
            // 
            // button63
            // 
            this.button63.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button63.Location = new System.Drawing.Point(12, 12);
            this.button63.Name = "button63";
            this.button63.Size = new System.Drawing.Size(286, 28);
            this.button63.TabIndex = 110;
            this.button63.Text = "Отдел контроля телефоны";
            this.button63.UseVisualStyleBackColor = true;
            this.button63.Click += new System.EventHandler(this.button63_Click);
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 100);
            this.Controls.Add(this.button64);
            this.Controls.Add(this.button63);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form5";
            this.Text = "Smart explorer телефоны";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button64;
        private System.Windows.Forms.Button button63;
    }
}