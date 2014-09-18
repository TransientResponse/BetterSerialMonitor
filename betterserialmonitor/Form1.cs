using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BetterSerialMonitor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            port.Encoding = Encoding.ASCII;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        #region Delegate definitions
        private delegate void RxTextSetter(string text);
        private delegate bool RxFocusChecker();
        private delegate void AutoScroller();
        private delegate void FixedScroller(int sto);
        private delegate void HistoryInserter(string line);
        private delegate void HistoryClearer();
        #endregion

        private string StoredRxText; //Keep this around
        private StringBuilder pauseBuffer = null;
        private int scrollPosition = 0;
        private int cursorPosition = 0;
        private int selectedIndex = 0;
        //private List<string> storedHistory = new List<string>();

        const uint MAX_HISTORY = 256;

        private string matchByteToString(Match m)
        {
            byte b = byte.Parse(m.Groups[2].Value, System.Globalization.NumberStyles.HexNumber);
            string equiv = Encoding.ASCII.GetString(new byte[] { b });
            return equiv;
        }

        private void portSetup(object sender, EventArgs e)
        {
            //Populate available ports
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            foreach (string portt in ports)
                portNameBox.Items.Add(portt);
            //Set default baud rate
            baudRateSetting.SelectedIndex = 1;
            //Set default data bits
            dataBitsList.Items.Add(5);
            dataBitsList.Items.Add(6);
            dataBitsList.Items.Add(7);
            dataBitsList.Items.Add(8);
            dataBitsList.SelectedIndex = 3;

            //Selected parity bits
            parityBox.SelectedIndex = 0;
            //Selected stop bits
            stopBitsList.SelectedIndex = 1;
            //Selected EOL
            eolCharsBox.SelectedIndex = 0;

            //Disable close button
            portCloseButton.Enabled = false;

            //Default check "Show Text"
            showTextButton.Checked = true;
            //Default check "Send Text"
            sendingTextButton.Checked = true;

            //Default check "Clear on Send"
            clearSendBox.Checked = true;

            //icsCommands.SelectedIndex = 0;
            //icsCommands.Enabled = false;

            txDataBox.Enabled = false;

            autoscrollBox.Checked = true;

            historyClearButton.Enabled = false;

            sendingDataButton.Enabled = false;
            sendingTextButton.Enabled = false;
            clearSendBox.Enabled = false;
            sendNewline.Enabled = false;
            clearTxBtn.Enabled = false;

#if DEBUG
            /* FOR TESTING */
            rxDataBox.Text = "THIS IS A TEST BUILD";
#endif
        }

        private void openPort()
        {
            if (portNameBox.Text.Length > 0)
                port.PortName = portNameBox.Text;
            else
            {
                MessageBox.Show("No port name given", "No port", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (portNameBox.Text == "AT_LUDICROUS_SPEED")
            {
                MessageBox.Show("YOU'VE GONE TO PLAID!", "LUDICROUS SPEED", MessageBoxButtons.OK);
                return;
            }

            try
            {
                int baudRate = int.Parse(baudRateSetting.Text);
                port.BaudRate = baudRate;
            }
            catch (Exception ex)
            {
                string msg = string.Format("Error parsing baud rate.\nException: {0}", ex.Message);
                MessageBox.Show(msg, "Bad baud rate", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            port.DataBits = (int)dataBitsList.SelectedItem;

            //Set parity bits
            switch(parityBox.SelectedText)
            {
                case "None":
                    port.Parity = System.IO.Ports.Parity.None;
                    break;
                case "Even":
                    port.Parity = System.IO.Ports.Parity.Even;
                    break;
                case "Odd":
                    port.Parity = System.IO.Ports.Parity.Odd;
                    break;
                case "Always 1":
                    port.Parity = System.IO.Ports.Parity.Mark;
                    break;
                case "Always 0":
                    port.Parity = System.IO.Ports.Parity.Space;
                    break;
                default:
                    port.Parity = System.IO.Ports.Parity.None; //TILT
                    break;
            }

            //Set stop bits
            switch(stopBitsList.SelectedText)
            {
                case "0":
                    port.StopBits = System.IO.Ports.StopBits.None;
                    break;
                case "1":
                    port.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case "1.5":
                    port.StopBits = System.IO.Ports.StopBits.OnePointFive;
                    break;
                case "2":
                    port.StopBits = System.IO.Ports.StopBits.Two;
                    break;
                default:
                    port.StopBits = System.IO.Ports.StopBits.One; //TILT
                    break;
            }

            //Set EOL
            switch(eolCharsBox.SelectedText)
            {
                case "CR+LF":
                    port.NewLine = "\r\n"; //Windows (default)
                    break;
                case "CR":
                    port.NewLine = "\r"; //Who uses this?
                    break;
                case "LF":
                    port.NewLine = "\n"; //Unix
                    break;
                default:
                    port.NewLine = "\r\n";
                    break;
            }

            try
            {
                port.Open();
                portCloseButton.Enabled = true;
                sendButton.Enabled = true;
                //icsCommands.Enabled = true;
                portOpenButton.Enabled = false;
                portNameBox.Enabled = false;
                baudRateSetting.Enabled = false;
                stopBitsList.Enabled = false;
                parityBox.Enabled = false;
                eolCharsBox.Enabled = false;
                dataBitsList.Enabled = false;
                txDataBox.Enabled = true;
                historyClearButton.Enabled = true;
                sendingDataButton.Enabled = true;
                sendingTextButton.Enabled = true;
                clearSendBox.Enabled = true;
                sendNewline.Enabled = true;
                clearTxBtn.Enabled = true;
            }
            catch (Exception ex)
            {
                string msg = string.Format("Error opening port.\n.NET Exception: {0}", ex.Message);
                MessageBox.Show(msg, "Error opening port", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void portOpenButton_Click(object sender, EventArgs e)
        {
            openPort();
        }

        private void portCloseButton_Click(object sender, EventArgs e)
        {
            try
            {
                port.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Error closing port\n.Net Exception: {0}\nYou should probably close and reopen this program.", ex.Message), "Error closing port", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            portCloseButton.Enabled = false;
            sendButton.Enabled = false;
            portOpenButton.Enabled = true;
            portNameBox.Enabled = true;
            baudRateSetting.Enabled = true;
            stopBitsList.Enabled = true;
            parityBox.Enabled = true;
            eolCharsBox.Enabled = true;
            dataBitsList.Enabled = true;
            //icsCommands.Enabled = false;
            //txDataBox.Text = string.Empty;
            txDataBox.Enabled = false;
            historyClearButton.Enabled = false;
            sendingDataButton.Enabled = false;
            sendingTextButton.Enabled = false;
            clearSendBox.Enabled = false;
            sendNewline.Enabled = false;
            clearTxBtn.Enabled = false;
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            sendData();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            rxDataBox.Text = string.Empty; //Easy peasy.
            StoredRxText = string.Empty; //Also kill this
        }

        private string bothToText(string both)
        {
            both.Replace("TB", "\t");
            both.Replace("CR", "\r");
            both.Replace("LF", "\n");
            //both.Replace("SP", " ");
            string[] things = both.Split(new char[] { ' ' });
            StringBuilder buffer = new StringBuilder();

            foreach(string thing in things)
            {
                char cap = char.Parse(Regex.Match(thing, @"[0-9A-Z]{2}\[([^\]]+)\]").Groups[1].Value);
                buffer.Append(cap);
            }

            return buffer.ToString();
        }

        private string hexToText(string hexForm)
        {
            string[] things = hexForm.Split(new char[] { ' ' });
            StringBuilder buffer = new StringBuilder();
            foreach(string thing in things)
            {
                buffer.Append((char)byte.Parse(thing, System.Globalization.NumberStyles.HexNumber));
            }

            return buffer.ToString();
        }

        private void switchView(object sender, EventArgs e)
		{
			if (rxDataBox.Text.Length > 0)//Only change if there's text
			{
				if (showHexButton.Checked)
				{ //Now on show hex mode
                    //StoredRxText = rxDataBox.Text; //Store last text
					StringBuilder buffer = new StringBuilder();

                    buffer.Append(convertToHex(StoredRxText));

					SetRxText(buffer.ToString());
				}
                else if(showBothButton.Checked)
                {
                    SetRxText(convertToBoth(StoredRxText));
                }
				else //Now on show text mode
				{
                    //Reload last text
                    SetRxText(StoredRxText);
				}
			}
        }

        private void finalCleanup(object sender, FormClosingEventArgs e)
        {
            //Make sure the port is closed.
            if (port.IsOpen)
                port.Close();
        }

        private string convertToBoth(string text)
        {
            byte[] equiv = Encoding.ASCII.GetBytes(text);
            StringBuilder buffer = new StringBuilder();
            //buffer.Append(' ');
            for (int i = 0; i < equiv.Length; i++)
                buffer.AppendFormat("{0:X2}[{1}] ", equiv[i], text[i]);

            buffer.Remove(buffer.Length - 1, 1);

            buffer.Replace("0D[\r]", "0D[CR]");
            buffer.Replace("0A[\n]", "0A[LF]");
            buffer.Replace("09[\t]", "09[TAB]");
            buffer.Replace("20[ ]", "20[SP]");

            return buffer.ToString();
        }

        private string convertToHex(string text)
        {
            byte[] equiv = Encoding.ASCII.GetBytes(text);
            StringBuilder buffer = new StringBuilder();
            //buffer.Append(' ');
            for (int i = 0; i < equiv.Length; i++)
                buffer.AppendFormat("{0:X2} ", equiv[i]);

            //buffer.Remove(buffer.Length - 1, 1);

            return buffer.ToString();
        }

        private void updateRxBox()
        {
            if ((!port.IsOpen) || (port.BytesToRead == 0))
                return;

            //Buffer to store current text and add new text/data
            StringBuilder buffer = new StringBuilder(rxDataBox.Text);

            string newData = port.ReadExisting();
            //Append new data, but keep text form handy
            StoredRxText += newData;


            

            //Simple if in text mode.
            if (showTextButton.Checked)
                buffer.Append(newData);
            //More complicated for hex mode.
            else if (showBothButton.Checked)
                buffer.Append(convertToBoth(newData));
            else
                buffer.Append(convertToHex(newData));

            scrollPosition = rxDataBox.SelectionStart;

            if (pauseBox.Checked) //StoredRxText has been updated, so the new text is not lost
            {
                if (pauseBuffer == null)
                    pauseBuffer = new StringBuilder(newData);
                else
                    pauseBuffer.Append(newData);

                return;
            }
            else
            {
                //Replace text in box with the buffer contents
                SetRxText(buffer.ToString()); 
            }

            if (autoscrollBox.Checked)
                AutoScrollerator();
            else
                ScrollTo(scrollPosition);
        }

        #if __MonoCS__
        private void manualRefresh(object sender, System.EventArgs e)
        {
            updateRxBox();
        }
        #endif

        #region Delegate implementations
        private void SetRxText(string txt)
        {
            if (this.rxDataBox.InvokeRequired)
            {
                RxTextSetter rts = new RxTextSetter(SetRxText);
                this.Invoke(rts, new object[] { txt });
            }
            else
                this.rxDataBox.Text = txt;
        }

        private void AutoScrollerator()
        {
            if(this.rxDataBox.InvokeRequired)
            {
                AutoScroller asr = new AutoScroller(AutoScrollerator);
                this.Invoke(asr);
            }
            else
            {
                rxDataBox.SelectionStart = rxDataBox.Text.Length;
                rxDataBox.ScrollToCaret();
            }
        }

        private bool CheckRxFocus()
        {
            if (this.rxDataBox.InvokeRequired)
            {
                RxFocusChecker rfc = new RxFocusChecker(CheckRxFocus);
                return (bool)this.Invoke(rfc, null);
            }
            else
                return this.rxDataBox.Focused;
        }

        private void ScrollTo(int pos)
        {
            if (this.rxDataBox.InvokeRequired)
            {
                FixedScroller fsr = new FixedScroller(ScrollTo);
                this.Invoke(fsr, new object[] { pos });
            }
            else
            {
                rxDataBox.SelectionStart = pos;
                rxDataBox.ScrollToCaret();
            }
        }

        private void ClearHistory()
        {
            if(this.txDataBox.InvokeRequired)
            {
                HistoryClearer hc = new HistoryClearer(ClearHistory);
                this.Invoke(hc);
            }
            else
            {
                txDataBox.Items.Clear();
            }
        }

        private void AddToHistory(string line)
        {
            if(this.txDataBox.InvokeRequired)
            {
                HistoryInserter hi = new HistoryInserter(AddToHistory);
                this.Invoke(hi, new object[]{line});
            }
            else
            {
                txDataBox.Items.Insert(0, line);
                if (txDataBox.Items.Count > MAX_HISTORY)
                    txDataBox.Items.RemoveAt((int)MAX_HISTORY - 1);
            }
        }
        #endregion

        private void updateBox(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            System.Threading.Thread.Sleep(30);
            updateRxBox();
        }

        private void sendOnEnter(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Up && clearSendBox.Checked)
            {
                if (selectedIndex < txDataBox.Items.Count-1)
                {
                    selectedIndex++;
                    txDataBox.SelectionStart = txDataBox.Text.Length;
                    txDataBox.Text = (string)txDataBox.Items[selectedIndex]; 
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if(e.KeyCode == Keys.Down)
            {
                if (selectedIndex > 0)
                {
                    selectedIndex--;
                    txDataBox.SelectionStart = txDataBox.Text.Length;
                    txDataBox.Text = (string)txDataBox.Items[selectedIndex]; 
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void sendData()
        {
            if(txDataBox.Text.Length == 0)
            {
                MessageBox.Show("Cannot send nothing", "Empty send data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string toSend = txDataBox.Text;

            

            if (sendingTextButton.Checked)
            {
                AddToHistory(toSend);

                try
                {
                    string[] components = Regex.Split(toSend, "((?:%|&|0x)[0-9a-fA-F]{1,2})");
                    foreach (string component in components)
                    {
                        if (Regex.IsMatch(component, "(?:%|&|0x)([0-9a-fA-F]{1,2})"))
                            port.Write(new byte[] { byte.Parse(Regex.Match(component, "(?:%|&|0x)([0-9a-fA-F]{1,2})").Groups[1].Value, System.Globalization.NumberStyles.HexNumber) }, 0, 1);
                        else
                            port.Write(component);
                    }
                }
                catch (Exception e)
                {
                    string msg = string.Format("Could not parse byte escape, sending as text\nError: {0}", e.Message);
                    MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
                if (sendNewline.Checked)
                    port.WriteLine("");
            }
            else
            {
                char[] seps = new char[] { ' ', ',', '.', ';' };
                string[] broken = toSend.Split(seps);

                byte[] bytes = new byte[broken.Length];

                bool conversionComplete = false;

                for (int i = 0; i < broken.Length; i++)
                {
                    if ((broken[i].Length == 1) && (char.IsLetter(broken[i][0])))
                        bytes[i] = (byte)broken[i][0];
                    if (!byte.TryParse(broken[i], System.Globalization.NumberStyles.Integer, null, out bytes[i]))
                    {
                        //Parsing plain int failed.
                        string str = broken[i];
                        if (str.StartsWith("0x") || str.StartsWith("&h"))
                            str = str.Substring(2);
                        else if (str.StartsWith("%"))
                            str = str.Substring(1);

                        if (!byte.TryParse(str, System.Globalization.NumberStyles.HexNumber, null, out bytes[i]))
                        {
                            MessageBox.Show(string.Format("Could not convert '{0}' to byte\nCheck data group {1}", broken[i], i), "Data parsing failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return; //Bail out entirely rather than failing to send and clearing the box
                        }
                    }
                    if (i == broken.Length - 1)
                        conversionComplete = true; //Can only get here if conversion never failed.
                }

                if(conversionComplete)
                {
                    port.Write(bytes, 0, broken.Length);

                    if (sendNewline.Checked)
                        port.WriteLine("");

                    AddToHistory(Encoding.ASCII.GetString(bytes));
                }
                else
                    MessageBox.Show("Could not parse data", "Unknown error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (selectedIndex >= 0)
                    selectedIndex = -1;

                if (!clearSendBox.Checked)
                    cursorPosition = txDataBox.SelectionStart;
                else
                    cursorPosition = 0;

            }

            //Clear the box once its contents are sent
            if (clearSendBox.Checked)
                txDataBox.Text = string.Empty;
        }

        private void openOnEnter(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                try { openPort(); }
                catch(Exception ex)
                {
                    MessageBox.Show(string.Format("Cannot open port.\nError: {1}", ex.Message), "Error opening port", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (port.IsOpen)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true; 
                }
            }
        }

        private void rescanPorts(object sender, MouseEventArgs e)
        {
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            portNameBox.Items.Clear();
            foreach (string portt in ports)
                portNameBox.Items.Add(portt);
        }

        //private void icsCommands_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (!port.IsOpen)
        //        return;
            
        //    switch(icsCommands.SelectedIndex)
        //    {
        //        case 0:
        //            port.WriteLine("*IDN?;");
        //            break;
        //        case 1:
        //            port.WriteLine("PROGRAM:CURRENT,500;");
        //            break;
        //        case 2:
        //            port.WriteLine("PROGRAM:CURRENT,800;");
        //            break;
        //        case 3:
        //            port.WriteLine("PROGRAM:CURRENT,1000");
        //            break;
        //        default:
        //            MessageBox.Show("Unkown iCS option", "TILT", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            break;
        //    }
        //}

        private void port_ErrorReceived(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            MessageBox.Show("Received error on COM port", "Serial port error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void pauseBox_CheckedChanged(object sender, EventArgs e)
        {
            if (pauseBox.Checked)
                return;

            //Otherwise, it's now false. 
            if (pauseBuffer == null)//Null buffer? nothing to do.
                return;

            if (pauseBuffer.Length == 0)
                return;

            string bufferedData = pauseBuffer.ToString();
            StringBuilder buffer = new StringBuilder(rxDataBox.Text);

            //Simple if in text mode.
            if (showTextButton.Checked)
                buffer.Append(bufferedData);
            //More complicated for hex mode.
            else if (showBothButton.Checked)
                buffer.Append(convertToBoth(bufferedData));
            else
                buffer.Append(convertToHex(bufferedData));

            SetRxText(buffer.ToString());

            pauseBuffer = null; //Null it out. Garbage collector will kill it soon. 
        }

        private void autoscrollBox_CheckedChanged(object sender, EventArgs e)
        {
            if (autoscrollBox.Checked)
                return;

            //If now false
            scrollPosition = rxDataBox.Text.Length;
        }

        private void historyClearButton_Click(object sender, EventArgs e)
        {
            ClearHistory();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch(keyData)
            {
                case Keys.Escape:
                    if (port.IsOpen)
                    {
                        portCloseButton_Click(this, null); 
                    }
                    return true;
                default:
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void stopSelection(object sender, EventArgs e)
        {

        }

        private void txDataBox_MouseUp(object sender, MouseEventArgs e)
        {
            if(cursorPosition != 0)
            {
                txDataBox.SelectionLength = 0;
                txDataBox.SelectionStart = cursorPosition;
                cursorPosition = 0;
            }
            else
            {
                txDataBox.SelectionLength = 0;
                txDataBox.SelectionStart = txDataBox.Text.Length;
            }
        }

        private void clearTxBtn_Click(object sender, EventArgs e)
        {
            txDataBox.Text = "";
        }
    }
}
