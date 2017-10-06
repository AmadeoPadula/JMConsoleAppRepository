namespace CargaIndividual
{
    partial class CargaIndividualForm
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.SeleccionarArchivoOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.TipoArchivoPanel = new System.Windows.Forms.Panel();
            this.ArchivoLabel = new System.Windows.Forms.Label();
            this.ExaminarButton = new System.Windows.Forms.Button();
            this.NombreArchivoTextBox = new System.Windows.Forms.TextBox();
            this.HistoricoPagosCheckBox = new System.Windows.Forms.CheckBox();
            this.NumeroCreditoLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.MovimientosCheckBox = new System.Windows.Forms.CheckBox();
            this.TablaAmortizacionCheckBox = new System.Windows.Forms.CheckBox();
            this.NumeroCreditoTextBox = new System.Windows.Forms.TextBox();
            this.ImportarButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SegundoProcesamientoRadioButton = new System.Windows.Forms.RadioButton();
            this.PrimerProcesamientoRadioButton = new System.Windows.Forms.RadioButton();
            this.TipoProcesamientoLabel = new System.Windows.Forms.Label();
            this.TipoArchivoPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SeleccionarArchivoOpenFileDialog
            // 
            this.SeleccionarArchivoOpenFileDialog.DefaultExt = "\"xls\"";
            this.SeleccionarArchivoOpenFileDialog.Filter = "xls files (*.xls)|*.xls";
            this.SeleccionarArchivoOpenFileDialog.ShowReadOnly = true;
            this.SeleccionarArchivoOpenFileDialog.Title = "Seleccionar archivo para importar";
            // 
            // TipoArchivoPanel
            // 
            this.TipoArchivoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TipoArchivoPanel.Controls.Add(this.ArchivoLabel);
            this.TipoArchivoPanel.Controls.Add(this.ExaminarButton);
            this.TipoArchivoPanel.Controls.Add(this.NombreArchivoTextBox);
            this.TipoArchivoPanel.Controls.Add(this.HistoricoPagosCheckBox);
            this.TipoArchivoPanel.Controls.Add(this.NumeroCreditoLabel);
            this.TipoArchivoPanel.Controls.Add(this.button1);
            this.TipoArchivoPanel.Controls.Add(this.MovimientosCheckBox);
            this.TipoArchivoPanel.Controls.Add(this.TablaAmortizacionCheckBox);
            this.TipoArchivoPanel.Controls.Add(this.NumeroCreditoTextBox);
            this.TipoArchivoPanel.Location = new System.Drawing.Point(15, 12);
            this.TipoArchivoPanel.Name = "TipoArchivoPanel";
            this.TipoArchivoPanel.Size = new System.Drawing.Size(663, 96);
            this.TipoArchivoPanel.TabIndex = 3;
            // 
            // ArchivoLabel
            // 
            this.ArchivoLabel.AutoSize = true;
            this.ArchivoLabel.Location = new System.Drawing.Point(15, 13);
            this.ArchivoLabel.Name = "ArchivoLabel";
            this.ArchivoLabel.Size = new System.Drawing.Size(46, 13);
            this.ArchivoLabel.TabIndex = 5;
            this.ArchivoLabel.Text = "Archivo:";
            // 
            // ExaminarButton
            // 
            this.ExaminarButton.Location = new System.Drawing.Point(587, 8);
            this.ExaminarButton.Name = "ExaminarButton";
            this.ExaminarButton.Size = new System.Drawing.Size(59, 23);
            this.ExaminarButton.TabIndex = 4;
            this.ExaminarButton.Text = "Examinar";
            this.ExaminarButton.UseVisualStyleBackColor = true;
            this.ExaminarButton.Click += new System.EventHandler(this.ExaminarButton_Click);
            // 
            // NombreArchivoTextBox
            // 
            this.NombreArchivoTextBox.Location = new System.Drawing.Point(67, 10);
            this.NombreArchivoTextBox.Name = "NombreArchivoTextBox";
            this.NombreArchivoTextBox.ReadOnly = true;
            this.NombreArchivoTextBox.Size = new System.Drawing.Size(514, 20);
            this.NombreArchivoTextBox.TabIndex = 3;
            // 
            // HistoricoPagosCheckBox
            // 
            this.HistoricoPagosCheckBox.AutoSize = true;
            this.HistoricoPagosCheckBox.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.HistoricoPagosCheckBox.Enabled = false;
            this.HistoricoPagosCheckBox.Location = new System.Drawing.Point(535, 50);
            this.HistoricoPagosCheckBox.Name = "HistoricoPagosCheckBox";
            this.HistoricoPagosCheckBox.Size = new System.Drawing.Size(100, 31);
            this.HistoricoPagosCheckBox.TabIndex = 0;
            this.HistoricoPagosCheckBox.Text = "Historico de Pagos";
            this.HistoricoPagosCheckBox.UseVisualStyleBackColor = true;
            // 
            // NumeroCreditoLabel
            // 
            this.NumeroCreditoLabel.AutoSize = true;
            this.NumeroCreditoLabel.Location = new System.Drawing.Point(15, 58);
            this.NumeroCreditoLabel.Name = "NumeroCreditoLabel";
            this.NumeroCreditoLabel.Size = new System.Drawing.Size(98, 13);
            this.NumeroCreditoLabel.TabIndex = 2;
            this.NumeroCreditoLabel.Text = "Número de Crédito:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(670, 69);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Examinar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ExaminarButton_Click);
            // 
            // MovimientosCheckBox
            // 
            this.MovimientosCheckBox.AutoSize = true;
            this.MovimientosCheckBox.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.MovimientosCheckBox.Enabled = false;
            this.MovimientosCheckBox.Location = new System.Drawing.Point(425, 50);
            this.MovimientosCheckBox.Name = "MovimientosCheckBox";
            this.MovimientosCheckBox.Size = new System.Drawing.Size(70, 31);
            this.MovimientosCheckBox.TabIndex = 0;
            this.MovimientosCheckBox.Text = "Movimientos";
            this.MovimientosCheckBox.UseVisualStyleBackColor = true;
            // 
            // TablaAmortizacionCheckBox
            // 
            this.TablaAmortizacionCheckBox.AutoSize = true;
            this.TablaAmortizacionCheckBox.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.TablaAmortizacionCheckBox.Enabled = false;
            this.TablaAmortizacionCheckBox.Location = new System.Drawing.Point(275, 50);
            this.TablaAmortizacionCheckBox.Name = "TablaAmortizacionCheckBox";
            this.TablaAmortizacionCheckBox.Size = new System.Drawing.Size(116, 31);
            this.TablaAmortizacionCheckBox.TabIndex = 0;
            this.TablaAmortizacionCheckBox.Text = "Tabla de Amortización";
            this.TablaAmortizacionCheckBox.UseVisualStyleBackColor = true;
            // 
            // NumeroCreditoTextBox
            // 
            this.NumeroCreditoTextBox.Location = new System.Drawing.Point(119, 55);
            this.NumeroCreditoTextBox.Name = "NumeroCreditoTextBox";
            this.NumeroCreditoTextBox.ReadOnly = true;
            this.NumeroCreditoTextBox.Size = new System.Drawing.Size(131, 20);
            this.NumeroCreditoTextBox.TabIndex = 0;
            // 
            // ImportarButton
            // 
            this.ImportarButton.Enabled = false;
            this.ImportarButton.Location = new System.Drawing.Point(603, 118);
            this.ImportarButton.Name = "ImportarButton";
            this.ImportarButton.Size = new System.Drawing.Size(75, 23);
            this.ImportarButton.TabIndex = 1;
            this.ImportarButton.Text = "Importar";
            this.ImportarButton.UseVisualStyleBackColor = true;
            this.ImportarButton.Click += new System.EventHandler(this.ImportarButton_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.SegundoProcesamientoRadioButton);
            this.panel1.Controls.Add(this.PrimerProcesamientoRadioButton);
            this.panel1.Controls.Add(this.TipoProcesamientoLabel);
            this.panel1.Location = new System.Drawing.Point(15, 118);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(582, 48);
            this.panel1.TabIndex = 4;
            // 
            // SegundoProcesamientoRadioButton
            // 
            this.SegundoProcesamientoRadioButton.AutoSize = true;
            this.SegundoProcesamientoRadioButton.Location = new System.Drawing.Point(425, 18);
            this.SegundoProcesamientoRadioButton.Name = "SegundoProcesamientoRadioButton";
            this.SegundoProcesamientoRadioButton.Size = new System.Drawing.Size(122, 17);
            this.SegundoProcesamientoRadioButton.TabIndex = 1;
            this.SegundoProcesamientoRadioButton.TabStop = true;
            this.SegundoProcesamientoRadioButton.Text = "2do.  Procesamiento";
            this.SegundoProcesamientoRadioButton.UseVisualStyleBackColor = true;
            // 
            // PrimerProcesamientoRadioButton
            // 
            this.PrimerProcesamientoRadioButton.AutoSize = true;
            this.PrimerProcesamientoRadioButton.Checked = true;
            this.PrimerProcesamientoRadioButton.Location = new System.Drawing.Point(223, 18);
            this.PrimerProcesamientoRadioButton.Name = "PrimerProcesamientoRadioButton";
            this.PrimerProcesamientoRadioButton.Size = new System.Drawing.Size(119, 17);
            this.PrimerProcesamientoRadioButton.TabIndex = 1;
            this.PrimerProcesamientoRadioButton.TabStop = true;
            this.PrimerProcesamientoRadioButton.Text = "1er.  Procesamiento";
            this.PrimerProcesamientoRadioButton.UseVisualStyleBackColor = true;
            // 
            // TipoProcesamientoLabel
            // 
            this.TipoProcesamientoLabel.AutoSize = true;
            this.TipoProcesamientoLabel.Location = new System.Drawing.Point(15, 20);
            this.TipoProcesamientoLabel.Name = "TipoProcesamientoLabel";
            this.TipoProcesamientoLabel.Size = new System.Drawing.Size(119, 13);
            this.TipoProcesamientoLabel.TabIndex = 0;
            this.TipoProcesamientoLabel.Text = "Tipo de Procesamiento:";
            // 
            // CargaIndividualForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 193);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.TipoArchivoPanel);
            this.Controls.Add(this.ImportarButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CargaIndividualForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Carga Individual";
            this.TipoArchivoPanel.ResumeLayout(false);
            this.TipoArchivoPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog SeleccionarArchivoOpenFileDialog;
        private System.Windows.Forms.Panel TipoArchivoPanel;
        private System.Windows.Forms.CheckBox HistoricoPagosCheckBox;
        private System.Windows.Forms.CheckBox MovimientosCheckBox;
        private System.Windows.Forms.CheckBox TablaAmortizacionCheckBox;
        private System.Windows.Forms.Label NumeroCreditoLabel;
        private System.Windows.Forms.TextBox NumeroCreditoTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button ImportarButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton SegundoProcesamientoRadioButton;
        private System.Windows.Forms.RadioButton PrimerProcesamientoRadioButton;
        private System.Windows.Forms.Label TipoProcesamientoLabel;
        private System.Windows.Forms.Label ArchivoLabel;
        private System.Windows.Forms.Button ExaminarButton;
        private System.Windows.Forms.TextBox NombreArchivoTextBox;
    }
}

