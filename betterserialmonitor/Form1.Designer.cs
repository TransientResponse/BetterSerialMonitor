namespace BetterSerialMonitor
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

        //private System.Windows.Forms.IButtonControl AcceptButton;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.port = new System.IO.Ports.SerialPort(this.components);
            this.portSettings = new System.Windows.Forms.GroupBox();
            this.dataBitsList = new System.Windows.Forms.ComboBox();
            this.eolCharsBox = new System.Windows.Forms.ComboBox();
            this.eolLabel = new System.Windows.Forms.Label();
            this.parityBox = new System.Windows.Forms.ComboBox();
            this.parityListLabel = new System.Windows.Forms.Label();
            this.stopBitsList = new System.Windows.Forms.ComboBox();
            this.stopBitsLabel = new System.Windows.Forms.Label();
            this.portNameBox = new System.Windows.Forms.ComboBox();
            this.portCloseButton = new System.Windows.Forms.Button();
            this.portOpenButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.baudRateSetting = new System.Windows.Forms.ComboBox();
            this.txControls = new System.Windows.Forms.GroupBox();
            this.historyClearButton = new System.Windows.Forms.Button();
            this.clearSendBox = new System.Windows.Forms.CheckBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.sendingDataButton = new System.Windows.Forms.RadioButton();
            this.sendingTextButton = new System.Windows.Forms.RadioButton();
            this.sendNewline = new System.Windows.Forms.CheckBox();
            this.rxControls = new System.Windows.Forms.GroupBox();
            this.autoscrollBox = new System.Windows.Forms.CheckBox();
            this.pauseBox = new System.Windows.Forms.CheckBox();
            this.showBothButton = new System.Windows.Forms.RadioButton();
            this.clearButton = new System.Windows.Forms.Button();
            this.showHexButton = new System.Windows.Forms.RadioButton();
            this.showTextButton = new System.Windows.Forms.RadioButton();
            this.rxDataBox = new System.Windows.Forms.TextBox();
            this.sendDataTT = new System.Windows.Forms.ToolTip(this.components);
            this.txDataBox = new System.Windows.Forms.TextBox();
            this.clearTxBtn = new System.Windows.Forms.Button();
            this.portSettings.SuspendLayout();
            this.txControls.SuspendLayout();
            this.rxControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // port
            // 
            this.port.ErrorReceived += new System.IO.Ports.SerialErrorReceivedEventHandler(this.port_ErrorReceived);
            this.port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.updateBox);
            // 
            // portSettings
            // 
            this.portSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.portSettings.Controls.Add(this.dataBitsList);
            this.portSettings.Controls.Add(this.eolCharsBox);
            this.portSettings.Controls.Add(this.eolLabel);
            this.portSettings.Controls.Add(this.parityBox);
            this.portSettings.Controls.Add(this.parityListLabel);
            this.portSettings.Controls.Add(this.stopBitsList);
            this.portSettings.Controls.Add(this.stopBitsLabel);
            this.portSettings.Controls.Add(this.portNameBox);
            this.portSettings.Controls.Add(this.portCloseButton);
            this.portSettings.Controls.Add(this.portOpenButton);
            this.portSettings.Controls.Add(this.label3);
            this.portSettings.Controls.Add(this.label2);
            this.portSettings.Controls.Add(this.label1);
            this.portSettings.Controls.Add(this.baudRateSetting);
            this.portSettings.Location = new System.Drawing.Point(12, 12);
            this.portSettings.Name = "portSettings";
            this.portSettings.Size = new System.Drawing.Size(610, 137);
            this.portSettings.TabIndex = 0;
            this.portSettings.TabStop = false;
            this.portSettings.Text = "Port Settings";
            // 
            // dataBitsList
            // 
            this.dataBitsList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dataBitsList.FormattingEnabled = true;
            this.dataBitsList.Location = new System.Drawing.Point(278, 90);
            this.dataBitsList.Name = "dataBitsList";
            this.dataBitsList.Size = new System.Drawing.Size(47, 21);
            this.dataBitsList.TabIndex = 16;
            // 
            // eolCharsBox
            // 
            this.eolCharsBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.eolCharsBox.FormattingEnabled = true;
            this.eolCharsBox.Items.AddRange(new object[] {
            "CR+LF",
            "CR",
            "LF"});
            this.eolCharsBox.Location = new System.Drawing.Point(184, 90);
            this.eolCharsBox.Name = "eolCharsBox";
            this.eolCharsBox.Size = new System.Drawing.Size(68, 21);
            this.eolCharsBox.TabIndex = 15;
            // 
            // eolLabel
            // 
            this.eolLabel.AutoSize = true;
            this.eolLabel.Location = new System.Drawing.Point(181, 74);
            this.eolLabel.Name = "eolLabel";
            this.eolLabel.Size = new System.Drawing.Size(88, 13);
            this.eolLabel.TabIndex = 14;
            this.eolLabel.Text = "EOL Character(s)";
            // 
            // parityBox
            // 
            this.parityBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parityBox.FormattingEnabled = true;
            this.parityBox.Items.AddRange(new object[] {
            "None",
            "Even",
            "Odd",
            "Always 1",
            "Always 0"});
            this.parityBox.Location = new System.Drawing.Point(82, 90);
            this.parityBox.Name = "parityBox";
            this.parityBox.Size = new System.Drawing.Size(81, 21);
            this.parityBox.TabIndex = 13;
            // 
            // parityListLabel
            // 
            this.parityListLabel.AutoSize = true;
            this.parityListLabel.Location = new System.Drawing.Point(79, 74);
            this.parityListLabel.Name = "parityListLabel";
            this.parityListLabel.Size = new System.Drawing.Size(33, 13);
            this.parityListLabel.TabIndex = 12;
            this.parityListLabel.Text = "Parity";
            // 
            // stopBitsList
            // 
            this.stopBitsList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.stopBitsList.FormattingEnabled = true;
            this.stopBitsList.Items.AddRange(new object[] {
            "0",
            "1",
            "1.5",
            "2"});
            this.stopBitsList.Location = new System.Drawing.Point(10, 90);
            this.stopBitsList.Name = "stopBitsList";
            this.stopBitsList.Size = new System.Drawing.Size(49, 21);
            this.stopBitsList.TabIndex = 11;
            // 
            // stopBitsLabel
            // 
            this.stopBitsLabel.AutoSize = true;
            this.stopBitsLabel.Location = new System.Drawing.Point(7, 74);
            this.stopBitsLabel.Name = "stopBitsLabel";
            this.stopBitsLabel.Size = new System.Drawing.Size(49, 13);
            this.stopBitsLabel.TabIndex = 10;
            this.stopBitsLabel.Text = "Stop Bits";
            // 
            // portNameBox
            // 
            this.portNameBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.portNameBox.Font = new System.Drawing.Font("Courier New", 9F);
            this.portNameBox.FormattingEnabled = true;
            this.portNameBox.Location = new System.Drawing.Point(10, 41);
            this.portNameBox.Name = "portNameBox";
            this.portNameBox.Size = new System.Drawing.Size(102, 23);
            this.portNameBox.TabIndex = 8;
            this.portNameBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.openOnEnter);
            this.portNameBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.rescanPorts);
            // 
            // portCloseButton
            // 
            this.portCloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.portCloseButton.Location = new System.Drawing.Point(529, 36);
            this.portCloseButton.Name = "portCloseButton";
            this.portCloseButton.Size = new System.Drawing.Size(75, 23);
            this.portCloseButton.TabIndex = 7;
            this.portCloseButton.Text = "Close port";
            this.portCloseButton.UseVisualStyleBackColor = true;
            this.portCloseButton.Click += new System.EventHandler(this.portCloseButton_Click);
            // 
            // portOpenButton
            // 
            this.portOpenButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.portOpenButton.Location = new System.Drawing.Point(529, 12);
            this.portOpenButton.Name = "portOpenButton";
            this.portOpenButton.Size = new System.Drawing.Size(75, 23);
            this.portOpenButton.TabIndex = 6;
            this.portOpenButton.Text = "Open port";
            this.portOpenButton.UseVisualStyleBackColor = true;
            this.portOpenButton.Click += new System.EventHandler(this.portOpenButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(275, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Data Bits";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Port Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(138, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Baud Rate";
            // 
            // baudRateSetting
            // 
            this.baudRateSetting.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baudRateSetting.FormattingEnabled = true;
            this.baudRateSetting.Items.AddRange(new object[] {
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200",
            "115600"});
            this.baudRateSetting.Location = new System.Drawing.Point(138, 41);
            this.baudRateSetting.Name = "baudRateSetting";
            this.baudRateSetting.Size = new System.Drawing.Size(121, 23);
            this.baudRateSetting.TabIndex = 0;
            this.baudRateSetting.KeyUp += new System.Windows.Forms.KeyEventHandler(this.openOnEnter);
            // 
            // txControls
            // 
            this.txControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txControls.Controls.Add(this.clearTxBtn);
            this.txControls.Controls.Add(this.txDataBox);
            this.txControls.Controls.Add(this.historyClearButton);
            this.txControls.Controls.Add(this.clearSendBox);
            this.txControls.Controls.Add(this.sendButton);
            this.txControls.Controls.Add(this.sendingDataButton);
            this.txControls.Controls.Add(this.sendingTextButton);
            this.txControls.Controls.Add(this.sendNewline);
            this.txControls.Location = new System.Drawing.Point(12, 155);
            this.txControls.Name = "txControls";
            this.txControls.Size = new System.Drawing.Size(610, 73);
            this.txControls.TabIndex = 1;
            this.txControls.TabStop = false;
            this.txControls.Text = "Data to Send";
            // 
            // historyClearButton
            // 
            this.historyClearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.historyClearButton.Location = new System.Drawing.Point(529, 44);
            this.historyClearButton.Name = "historyClearButton";
            this.historyClearButton.Size = new System.Drawing.Size(75, 23);
            this.historyClearButton.TabIndex = 7;
            this.historyClearButton.Text = "Clear history";
            this.historyClearButton.UseVisualStyleBackColor = true;
            this.historyClearButton.Click += new System.EventHandler(this.historyClearButton_Click);
            // 
            // clearSendBox
            // 
            this.clearSendBox.AutoSize = true;
            this.clearSendBox.Location = new System.Drawing.Point(247, 47);
            this.clearSendBox.Name = "clearSendBox";
            this.clearSendBox.Size = new System.Drawing.Size(93, 17);
            this.clearSendBox.TabIndex = 5;
            this.clearSendBox.Text = "Clear on Send";
            this.clearSendBox.UseVisualStyleBackColor = true;
            // 
            // sendButton
            // 
            this.sendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sendButton.Enabled = false;
            this.sendButton.Location = new System.Drawing.Point(529, 14);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(75, 23);
            this.sendButton.TabIndex = 4;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // sendingDataButton
            // 
            this.sendingDataButton.AutoSize = true;
            this.sendingDataButton.Location = new System.Drawing.Point(62, 47);
            this.sendingDataButton.Name = "sendingDataButton";
            this.sendingDataButton.Size = new System.Drawing.Size(71, 17);
            this.sendingDataButton.TabIndex = 3;
            this.sendingDataButton.TabStop = true;
            this.sendingDataButton.Text = "Raw data";
            this.sendDataTT.SetToolTip(this.sendingDataButton, "Enter a series of bytes in decimal or hex, separated by spaces, commas, \r\nperiods" +
        ", or semicolons. No prefixes are necessary. ");
            this.sendingDataButton.UseVisualStyleBackColor = true;
            this.sendingDataButton.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // sendingTextButton
            // 
            this.sendingTextButton.AutoSize = true;
            this.sendingTextButton.Location = new System.Drawing.Point(10, 47);
            this.sendingTextButton.Name = "sendingTextButton";
            this.sendingTextButton.Size = new System.Drawing.Size(46, 17);
            this.sendingTextButton.TabIndex = 2;
            this.sendingTextButton.TabStop = true;
            this.sendingTextButton.Text = "Text";
            this.sendDataTT.SetToolTip(this.sendingTextButton, "Prefix hex bytes with 0x, %, or &. \r\nTo send more than one byte, use more than on" +
        "e prefix.");
            this.sendingTextButton.UseVisualStyleBackColor = true;
            // 
            // sendNewline
            // 
            this.sendNewline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sendNewline.AutoSize = true;
            this.sendNewline.Location = new System.Drawing.Point(407, 23);
            this.sendNewline.Name = "sendNewline";
            this.sendNewline.Size = new System.Drawing.Size(90, 17);
            this.sendNewline.TabIndex = 1;
            this.sendNewline.Text = "Send newline";
            this.sendNewline.UseVisualStyleBackColor = true;
            // 
            // rxControls
            // 
            this.rxControls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rxControls.Controls.Add(this.autoscrollBox);
            this.rxControls.Controls.Add(this.pauseBox);
            this.rxControls.Controls.Add(this.showBothButton);
            this.rxControls.Controls.Add(this.clearButton);
            this.rxControls.Controls.Add(this.showHexButton);
            this.rxControls.Controls.Add(this.showTextButton);
            this.rxControls.Controls.Add(this.rxDataBox);
            this.rxControls.Location = new System.Drawing.Point(12, 234);
            this.rxControls.Name = "rxControls";
            this.rxControls.Size = new System.Drawing.Size(610, 279);
            this.rxControls.TabIndex = 2;
            this.rxControls.TabStop = false;
            this.rxControls.Text = "Data Received";
            // 
            // autoscrollBox
            // 
            this.autoscrollBox.AutoSize = true;
            this.autoscrollBox.Location = new System.Drawing.Point(442, 17);
            this.autoscrollBox.Name = "autoscrollBox";
            this.autoscrollBox.Size = new System.Drawing.Size(72, 17);
            this.autoscrollBox.TabIndex = 6;
            this.autoscrollBox.Text = "Autoscroll";
            this.autoscrollBox.UseVisualStyleBackColor = true;
            this.autoscrollBox.CheckedChanged += new System.EventHandler(this.autoscrollBox_CheckedChanged);
            // 
            // pauseBox
            // 
            this.pauseBox.AutoSize = true;
            this.pauseBox.Location = new System.Drawing.Point(338, 17);
            this.pauseBox.Name = "pauseBox";
            this.pauseBox.Size = new System.Drawing.Size(97, 17);
            this.pauseBox.TabIndex = 5;
            this.pauseBox.Text = "Pause updates";
            this.pauseBox.UseVisualStyleBackColor = true;
            this.pauseBox.CheckedChanged += new System.EventHandler(this.pauseBox_CheckedChanged);
            // 
            // showBothButton
            // 
            this.showBothButton.AutoSize = true;
            this.showBothButton.Location = new System.Drawing.Point(173, 17);
            this.showBothButton.Name = "showBothButton";
            this.showBothButton.Size = new System.Drawing.Size(77, 17);
            this.showBothButton.TabIndex = 3;
            this.showBothButton.TabStop = true;
            this.showBothButton.Text = "Show Both";
            this.showBothButton.UseVisualStyleBackColor = true;
            this.showBothButton.CheckedChanged += new System.EventHandler(this.switchView);
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(256, 14);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 23);
            this.clearButton.TabIndex = 4;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // showHexButton
            // 
            this.showHexButton.AutoSize = true;
            this.showHexButton.Location = new System.Drawing.Point(92, 17);
            this.showHexButton.Name = "showHexButton";
            this.showHexButton.Size = new System.Drawing.Size(74, 17);
            this.showHexButton.TabIndex = 2;
            this.showHexButton.TabStop = true;
            this.showHexButton.Text = "Show Hex";
            this.showHexButton.UseVisualStyleBackColor = true;
            this.showHexButton.CheckedChanged += new System.EventHandler(this.switchView);
            // 
            // showTextButton
            // 
            this.showTextButton.AutoSize = true;
            this.showTextButton.Location = new System.Drawing.Point(10, 17);
            this.showTextButton.Name = "showTextButton";
            this.showTextButton.Size = new System.Drawing.Size(76, 17);
            this.showTextButton.TabIndex = 1;
            this.showTextButton.TabStop = true;
            this.showTextButton.Text = "Show Text";
            this.showTextButton.UseVisualStyleBackColor = true;
            this.showTextButton.CheckedChanged += new System.EventHandler(this.switchView);
            // 
            // rxDataBox
            // 
            this.rxDataBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rxDataBox.Font = new System.Drawing.Font("Courier New", 10F);
            this.rxDataBox.Location = new System.Drawing.Point(10, 40);
            this.rxDataBox.Multiline = true;
            this.rxDataBox.Name = "rxDataBox";
            this.rxDataBox.ReadOnly = true;
            this.rxDataBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.rxDataBox.Size = new System.Drawing.Size(594, 233);
            this.rxDataBox.TabIndex = 0;
            // 
            // txDataBox
            // 
            this.txDataBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txDataBox.Location = new System.Drawing.Point(10, 20);
            this.txDataBox.Name = "txDataBox";
            this.txDataBox.Size = new System.Drawing.Size(391, 21);
            this.txDataBox.TabIndex = 8;
            this.txDataBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.sendOnEnter);
            // 
            // clearTxBtn
            // 
            this.clearTxBtn.Location = new System.Drawing.Point(448, 44);
            this.clearTxBtn.Name = "clearTxBtn";
            this.clearTxBtn.Size = new System.Drawing.Size(75, 23);
            this.clearTxBtn.TabIndex = 9;
            this.clearTxBtn.Text = "Clear box";
            this.clearTxBtn.UseVisualStyleBackColor = true;
            this.clearTxBtn.Click += new System.EventHandler(this.clearTxBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 525);
            this.Controls.Add(this.rxControls);
            this.Controls.Add(this.txControls);
            this.Controls.Add(this.portSettings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Better Serial Monitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.finalCleanup);
            this.Load += new System.EventHandler(this.portSetup);
            this.portSettings.ResumeLayout(false);
            this.portSettings.PerformLayout();
            this.txControls.ResumeLayout(false);
            this.txControls.PerformLayout();
            this.rxControls.ResumeLayout(false);
            this.rxControls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.IO.Ports.SerialPort port;
        private System.Windows.Forms.GroupBox portSettings;
        private System.Windows.Forms.Button portCloseButton;
        private System.Windows.Forms.Button portOpenButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox txControls;
        private System.Windows.Forms.RadioButton sendingDataButton;
        private System.Windows.Forms.RadioButton sendingTextButton;
        private System.Windows.Forms.CheckBox sendNewline;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.GroupBox rxControls;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.RadioButton showHexButton;
        private System.Windows.Forms.RadioButton showTextButton;
        private System.Windows.Forms.TextBox rxDataBox;
        private System.Windows.Forms.CheckBox clearSendBox;
        private System.Windows.Forms.ComboBox portNameBox;
        private System.Windows.Forms.ComboBox baudRateSetting;
        private System.Windows.Forms.ToolTip sendDataTT;
        private System.Windows.Forms.RadioButton showBothButton;
        private System.Windows.Forms.ComboBox eolCharsBox;
        private System.Windows.Forms.Label eolLabel;
        private System.Windows.Forms.ComboBox parityBox;
        private System.Windows.Forms.Label parityListLabel;
        private System.Windows.Forms.ComboBox stopBitsList;
        private System.Windows.Forms.Label stopBitsLabel;
        private System.Windows.Forms.ComboBox dataBitsList;
        private System.Windows.Forms.CheckBox pauseBox;
        private System.Windows.Forms.CheckBox autoscrollBox;
        private System.Windows.Forms.Button historyClearButton;
        private System.Windows.Forms.TextBox txDataBox;
        private System.Windows.Forms.Button clearTxBtn;
    }
}

