namespace CargaIndividual
{
    partial class LogDetalle
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
            this.DetalleMensajesGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.DetalleMensajesGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // DetalleMensajesGridView
            // 
            this.DetalleMensajesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DetalleMensajesGridView.Location = new System.Drawing.Point(12, 12);
            this.DetalleMensajesGridView.Name = "DetalleMensajesGridView";
            this.DetalleMensajesGridView.Size = new System.Drawing.Size(260, 237);
            this.DetalleMensajesGridView.TabIndex = 0;
            // 
            // LogDetalle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.DetalleMensajesGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LogDetalle";
            this.Text = "LogDetalle";
            this.Load += new System.EventHandler(this.LogDetalle_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DetalleMensajesGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DetalleMensajesGridView;
    }
}