namespace EnergyProfiler
{
    partial class Form1
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.FlukeChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.SerialRXBox = new System.Windows.Forms.RichTextBox();
            this.SendTextBox = new System.Windows.Forms.TextBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.TagPortBox = new System.Windows.Forms.ComboBox();
            this.FlukePortBox = new System.Windows.Forms.ComboBox();
            this.StartButton = new System.Windows.Forms.Button();
            this.TagBaudBox = new System.Windows.Forms.TextBox();
            this.FlukeBaudBox = new System.Windows.Forms.TextBox();
            this.FileBox = new System.Windows.Forms.TextBox();
            this.FileButton = new System.Windows.Forms.Button();
            this.ChartEnableCheck = new System.Windows.Forms.CheckBox();
            this.SerialRXEnableCheck = new System.Windows.Forms.CheckBox();
            this.TimestampCheck = new System.Windows.Forms.CheckBox();
            this.TagPortLabel = new System.Windows.Forms.Label();
            this.FlukePortLabel = new System.Windows.Forms.Label();
            this.Baud1Label = new System.Windows.Forms.Label();
            this.Baud2Label = new System.Windows.Forms.Label();
            this.CRCheck = new System.Windows.Forms.CheckBox();
            this.LFCheck = new System.Windows.Forms.CheckBox();
            this.AppendCRLFLabel = new System.Windows.Forms.Label();
            this.LogEnableCheck = new System.Windows.Forms.CheckBox();
            this.ClearLogButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.FlukeChart)).BeginInit();
            this.SuspendLayout();
            // 
            // FlukeChart
            // 
            chartArea3.CursorX.IsUserEnabled = true;
            chartArea3.CursorX.IsUserSelectionEnabled = true;
            chartArea3.Name = "ChartArea1";
            this.FlukeChart.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.FlukeChart.Legends.Add(legend3);
            this.FlukeChart.Location = new System.Drawing.Point(13, 13);
            this.FlukeChart.Name = "FlukeChart";
            this.FlukeChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series5.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            series5.Legend = "Legend1";
            series5.Name = "State";
            series5.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series6.Color = System.Drawing.Color.Red;
            series6.Legend = "Legend1";
            series6.Name = "Current";
            this.FlukeChart.Series.Add(series5);
            this.FlukeChart.Series.Add(series6);
            this.FlukeChart.Size = new System.Drawing.Size(824, 300);
            this.FlukeChart.TabIndex = 0;
            // 
            // SerialRXBox
            // 
            this.SerialRXBox.Location = new System.Drawing.Point(13, 329);
            this.SerialRXBox.Name = "SerialRXBox";
            this.SerialRXBox.Size = new System.Drawing.Size(588, 224);
            this.SerialRXBox.TabIndex = 1;
            this.SerialRXBox.Text = "";
            this.SerialRXBox.TextChanged += new System.EventHandler(this.SerialRXBox_TextChanged);
            // 
            // SendTextBox
            // 
            this.SendTextBox.Location = new System.Drawing.Point(13, 582);
            this.SendTextBox.Name = "SendTextBox";
            this.SendTextBox.Size = new System.Drawing.Size(491, 20);
            this.SendTextBox.TabIndex = 2;
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(526, 582);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(75, 20);
            this.SendButton.TabIndex = 3;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // TagPortBox
            // 
            this.TagPortBox.FormattingEnabled = true;
            this.TagPortBox.Location = new System.Drawing.Point(618, 464);
            this.TagPortBox.Name = "TagPortBox";
            this.TagPortBox.Size = new System.Drawing.Size(89, 21);
            this.TagPortBox.TabIndex = 4;
            // 
            // FlukePortBox
            // 
            this.FlukePortBox.FormattingEnabled = true;
            this.FlukePortBox.Location = new System.Drawing.Point(748, 464);
            this.FlukePortBox.Name = "FlukePortBox";
            this.FlukePortBox.Size = new System.Drawing.Size(89, 21);
            this.FlukePortBox.TabIndex = 5;
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(691, 560);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 45);
            this.StartButton.TabIndex = 6;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // TagBaudBox
            // 
            this.TagBaudBox.Location = new System.Drawing.Point(618, 517);
            this.TagBaudBox.Name = "TagBaudBox";
            this.TagBaudBox.Size = new System.Drawing.Size(89, 20);
            this.TagBaudBox.TabIndex = 7;
            this.TagBaudBox.Text = "57600";
            // 
            // FlukeBaudBox
            // 
            this.FlukeBaudBox.Location = new System.Drawing.Point(748, 517);
            this.FlukeBaudBox.Name = "FlukeBaudBox";
            this.FlukeBaudBox.Size = new System.Drawing.Size(89, 20);
            this.FlukeBaudBox.TabIndex = 7;
            this.FlukeBaudBox.Text = "57600";
            // 
            // FileBox
            // 
            this.FileBox.Location = new System.Drawing.Point(618, 330);
            this.FileBox.Name = "FileBox";
            this.FileBox.Size = new System.Drawing.Size(138, 20);
            this.FileBox.TabIndex = 7;
            this.FileBox.Text = "C:\\Users\\dpham\\Box Sync\\Duy Pham\\Logs";
            // 
            // FileButton
            // 
            this.FileButton.Location = new System.Drawing.Point(764, 329);
            this.FileButton.Name = "FileButton";
            this.FileButton.Size = new System.Drawing.Size(75, 20);
            this.FileButton.TabIndex = 6;
            this.FileButton.Text = "File";
            this.FileButton.UseVisualStyleBackColor = true;
            this.FileButton.Click += new System.EventHandler(this.FileButton_Click);
            // 
            // ChartEnableCheck
            // 
            this.ChartEnableCheck.AutoSize = true;
            this.ChartEnableCheck.ForeColor = System.Drawing.SystemColors.Control;
            this.ChartEnableCheck.Location = new System.Drawing.Point(618, 421);
            this.ChartEnableCheck.Name = "ChartEnableCheck";
            this.ChartEnableCheck.Size = new System.Drawing.Size(87, 17);
            this.ChartEnableCheck.TabIndex = 8;
            this.ChartEnableCheck.Text = "Enable Chart";
            this.ChartEnableCheck.UseVisualStyleBackColor = true;
            // 
            // SerialRXEnableCheck
            // 
            this.SerialRXEnableCheck.AutoSize = true;
            this.SerialRXEnableCheck.ForeColor = System.Drawing.SystemColors.Control;
            this.SerialRXEnableCheck.Location = new System.Drawing.Point(618, 398);
            this.SerialRXEnableCheck.Name = "SerialRXEnableCheck";
            this.SerialRXEnableCheck.Size = new System.Drawing.Size(106, 17);
            this.SerialRXEnableCheck.TabIndex = 8;
            this.SerialRXEnableCheck.Text = "Enable Serial RX";
            this.SerialRXEnableCheck.UseVisualStyleBackColor = true;
            // 
            // TimestampCheck
            // 
            this.TimestampCheck.AutoSize = true;
            this.TimestampCheck.Checked = true;
            this.TimestampCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TimestampCheck.ForeColor = System.Drawing.SystemColors.Control;
            this.TimestampCheck.Location = new System.Drawing.Point(748, 421);
            this.TimestampCheck.Name = "TimestampCheck";
            this.TimestampCheck.Size = new System.Drawing.Size(77, 17);
            this.TimestampCheck.TabIndex = 8;
            this.TimestampCheck.Text = "Timestamp";
            this.TimestampCheck.UseVisualStyleBackColor = true;
            // 
            // TagPortLabel
            // 
            this.TagPortLabel.AutoSize = true;
            this.TagPortLabel.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.TagPortLabel.Location = new System.Drawing.Point(615, 448);
            this.TagPortLabel.Name = "TagPortLabel";
            this.TagPortLabel.Size = new System.Drawing.Size(26, 13);
            this.TagPortLabel.TabIndex = 9;
            this.TagPortLabel.Text = "Tag";
            // 
            // FlukePortLabel
            // 
            this.FlukePortLabel.AutoSize = true;
            this.FlukePortLabel.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.FlukePortLabel.Location = new System.Drawing.Point(745, 448);
            this.FlukePortLabel.Name = "FlukePortLabel";
            this.FlukePortLabel.Size = new System.Drawing.Size(33, 13);
            this.FlukePortLabel.TabIndex = 9;
            this.FlukePortLabel.Text = "Fluke";
            // 
            // Baud1Label
            // 
            this.Baud1Label.AutoSize = true;
            this.Baud1Label.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.Baud1Label.Location = new System.Drawing.Point(615, 501);
            this.Baud1Label.Name = "Baud1Label";
            this.Baud1Label.Size = new System.Drawing.Size(32, 13);
            this.Baud1Label.TabIndex = 9;
            this.Baud1Label.Text = "Baud";
            // 
            // Baud2Label
            // 
            this.Baud2Label.AutoSize = true;
            this.Baud2Label.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.Baud2Label.Location = new System.Drawing.Point(745, 501);
            this.Baud2Label.Name = "Baud2Label";
            this.Baud2Label.Size = new System.Drawing.Size(32, 13);
            this.Baud2Label.TabIndex = 9;
            this.Baud2Label.Text = "Baud";
            // 
            // CRCheck
            // 
            this.CRCheck.AutoSize = true;
            this.CRCheck.ForeColor = System.Drawing.SystemColors.Control;
            this.CRCheck.Location = new System.Drawing.Point(526, 559);
            this.CRCheck.Name = "CRCheck";
            this.CRCheck.Size = new System.Drawing.Size(41, 17);
            this.CRCheck.TabIndex = 8;
            this.CRCheck.Text = "CR";
            this.CRCheck.UseVisualStyleBackColor = true;
            // 
            // LFCheck
            // 
            this.LFCheck.AutoSize = true;
            this.LFCheck.ForeColor = System.Drawing.SystemColors.Control;
            this.LFCheck.Location = new System.Drawing.Point(573, 559);
            this.LFCheck.Name = "LFCheck";
            this.LFCheck.Size = new System.Drawing.Size(38, 17);
            this.LFCheck.TabIndex = 8;
            this.LFCheck.Text = "LF";
            this.LFCheck.UseVisualStyleBackColor = true;
            // 
            // AppendCRLFLabel
            // 
            this.AppendCRLFLabel.AutoSize = true;
            this.AppendCRLFLabel.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.AppendCRLFLabel.Location = new System.Drawing.Point(470, 560);
            this.AppendCRLFLabel.Name = "AppendCRLFLabel";
            this.AppendCRLFLabel.Size = new System.Drawing.Size(50, 13);
            this.AppendCRLFLabel.TabIndex = 9;
            this.AppendCRLFLabel.Text = "Append: ";
            // 
            // LogEnableCheck
            // 
            this.LogEnableCheck.AutoSize = true;
            this.LogEnableCheck.Checked = true;
            this.LogEnableCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LogEnableCheck.ForeColor = System.Drawing.SystemColors.Control;
            this.LogEnableCheck.Location = new System.Drawing.Point(748, 397);
            this.LogEnableCheck.Name = "LogEnableCheck";
            this.LogEnableCheck.Size = new System.Drawing.Size(80, 17);
            this.LogEnableCheck.TabIndex = 8;
            this.LogEnableCheck.Text = "Enable Log";
            this.LogEnableCheck.UseVisualStyleBackColor = true;
            // 
            // ClearLogButton
            // 
            this.ClearLogButton.Location = new System.Drawing.Point(618, 356);
            this.ClearLogButton.Name = "ClearLogButton";
            this.ClearLogButton.Size = new System.Drawing.Size(138, 26);
            this.ClearLogButton.TabIndex = 6;
            this.ClearLogButton.Text = "Clear Log File";
            this.ClearLogButton.UseVisualStyleBackColor = true;
            this.ClearLogButton.Click += new System.EventHandler(this.ClearLogButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.ClientSize = new System.Drawing.Size(851, 614);
            this.Controls.Add(this.FlukePortLabel);
            this.Controls.Add(this.Baud2Label);
            this.Controls.Add(this.Baud1Label);
            this.Controls.Add(this.AppendCRLFLabel);
            this.Controls.Add(this.TagPortLabel);
            this.Controls.Add(this.TimestampCheck);
            this.Controls.Add(this.LFCheck);
            this.Controls.Add(this.CRCheck);
            this.Controls.Add(this.SerialRXEnableCheck);
            this.Controls.Add(this.LogEnableCheck);
            this.Controls.Add(this.ChartEnableCheck);
            this.Controls.Add(this.FileBox);
            this.Controls.Add(this.FlukeBaudBox);
            this.Controls.Add(this.TagBaudBox);
            this.Controls.Add(this.ClearLogButton);
            this.Controls.Add(this.FileButton);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.FlukePortBox);
            this.Controls.Add(this.TagPortBox);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.SendTextBox);
            this.Controls.Add(this.SerialRXBox);
            this.Controls.Add(this.FlukeChart);
            this.Name = "Form1";
            this.Text = "Energy Profiler";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.FlukeChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart FlukeChart;
        private System.Windows.Forms.RichTextBox SerialRXBox;
        private System.Windows.Forms.TextBox SendTextBox;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.ComboBox TagPortBox;
        private System.Windows.Forms.ComboBox FlukePortBox;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.TextBox TagBaudBox;
        private System.Windows.Forms.TextBox FlukeBaudBox;
        private System.Windows.Forms.TextBox FileBox;
        private System.Windows.Forms.Button FileButton;
        private System.Windows.Forms.CheckBox ChartEnableCheck;
        private System.Windows.Forms.CheckBox SerialRXEnableCheck;
        private System.Windows.Forms.CheckBox TimestampCheck;
        private System.Windows.Forms.Label TagPortLabel;
        private System.Windows.Forms.Label FlukePortLabel;
        private System.Windows.Forms.Label Baud1Label;
        private System.Windows.Forms.Label Baud2Label;
        private System.Windows.Forms.CheckBox CRCheck;
        private System.Windows.Forms.CheckBox LFCheck;
        private System.Windows.Forms.Label AppendCRLFLabel;
        private System.Windows.Forms.CheckBox LogEnableCheck;
        private System.Windows.Forms.Button ClearLogButton;
    }
}

