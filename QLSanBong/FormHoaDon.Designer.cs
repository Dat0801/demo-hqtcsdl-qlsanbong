
namespace QLSanBong
{
    partial class FormHoaDon
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
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHoaDon));
            this.reportViewer2 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_InHoaDon = new System.Windows.Forms.Button();
            this.cbo_HoaDon = new System.Windows.Forms.ComboBox();
            this.HoaDonBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.QLSANBONGDataSet = new QLSanBong.QLSANBONGDataSet();
            this.HOADONTableAdapter = new QLSanBong.QLSANBONGDataSetTableAdapters.HOADONTableAdapter();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HoaDonBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QLSANBONGDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer2
            // 
            reportDataSource1.Name = "DataSetHoaDon";
            reportDataSource1.Value = this.HoaDonBindingSource;
            this.reportViewer2.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer2.LocalReport.ReportEmbeddedResource = "QLSanBong.ReportHoaDon.rdlc";
            this.reportViewer2.Location = new System.Drawing.Point(187, 134);
            this.reportViewer2.Name = "reportViewer2";
            this.reportViewer2.ServerReport.BearerToken = null;
            this.reportViewer2.Size = new System.Drawing.Size(875, 702);
            this.reportViewer2.TabIndex = 1;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.btn_InHoaDon);
            this.groupBox4.Controls.Add(this.cbo_HoaDon);
            this.groupBox4.Location = new System.Drawing.Point(187, 22);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox4.Size = new System.Drawing.Size(875, 97);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Thống kê";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(108, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 20);
            this.label5.TabIndex = 15;
            this.label5.Text = "Mã Hóa Đơn";
            // 
            // btn_InHoaDon
            // 
            this.btn_InHoaDon.BackColor = System.Drawing.Color.White;
            this.btn_InHoaDon.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_InHoaDon.Image = ((System.Drawing.Image)(resources.GetObject("btn_InHoaDon.Image")));
            this.btn_InHoaDon.Location = new System.Drawing.Point(646, 26);
            this.btn_InHoaDon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_InHoaDon.Name = "btn_InHoaDon";
            this.btn_InHoaDon.Size = new System.Drawing.Size(204, 53);
            this.btn_InHoaDon.TabIndex = 13;
            this.btn_InHoaDon.Text = "In Hóa Đơn";
            this.btn_InHoaDon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_InHoaDon.UseVisualStyleBackColor = false;
            this.btn_InHoaDon.Click += new System.EventHandler(this.btn_InHoaDon_Click);
            // 
            // cbo_HoaDon
            // 
            this.cbo_HoaDon.FormattingEnabled = true;
            this.cbo_HoaDon.Items.AddRange(new object[] {
            "2021",
            "2022",
            "2023"});
            this.cbo_HoaDon.Location = new System.Drawing.Point(338, 42);
            this.cbo_HoaDon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbo_HoaDon.Name = "cbo_HoaDon";
            this.cbo_HoaDon.Size = new System.Drawing.Size(147, 24);
            this.cbo_HoaDon.TabIndex = 12;
            // 
            // HoaDonBindingSource
            // 
            this.HoaDonBindingSource.DataSource = typeof(QLSanBong.DTO.HoaDon);
            // 
            // QLSANBONGDataSet
            // 
            this.QLSANBONGDataSet.DataSetName = "QLSANBONGDataSet";
            this.QLSANBONGDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // HOADONTableAdapter
            // 
            this.HOADONTableAdapter.ClearBeforeFill = true;
            // 
            // FormHoaDon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1297, 887);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.reportViewer2);
            this.Name = "FormHoaDon";
            this.Text = "FormHoaDon";
            this.Load += new System.EventHandler(this.FormHoaDon_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HoaDonBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QLSANBONGDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer2;
        private System.Windows.Forms.BindingSource HoaDonBindingSource;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_InHoaDon;
        private System.Windows.Forms.ComboBox cbo_HoaDon;
        private QLSANBONGDataSet QLSANBONGDataSet;
        private QLSANBONGDataSetTableAdapters.HOADONTableAdapter HOADONTableAdapter;
    }
}