using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Windows.Forms;
using System.IO;

namespace BetterSerialMonitor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            port.Encoding = Encoding.ASCII;

            if (Type.GetType("Mono.Runtime") != null)
            {
                SettingsDir = Environment.GetEnvironmentVariable("HOME") + "/.BetterSerialMonitor";
                settingsfile = SettingsDir + "/settings.xml";
            }
            else
            {
                SettingsDir = Environment.GetEnvironmentVariable("LOCALAPPDATA") + @"\BetterSerialMonitor";
                settingsfile = SettingsDir + "\\settings.xml";
            }

            if (System.IO.File.Exists(settingsfile))
            {
                LoadSettings(settingsfile);
            }//if
            else//No settings file, set defaults
            {
                //Set default baud rate
                baudRateSetting.SelectedIndex = 1;
                //Set default data bits
                //dataBitsList.Items.Add(5);
                //dataBitsList.Items.Add(6);
                //dataBitsList.Items.Add(7);
                //dataBitsList.Items.Add(8);
                dataBitsList.SelectedIndex = 3;

                //Selected parity bits
                parityBox.SelectedIndex = 0;
                //Selected stop bits
                stopBitsList.SelectedIndex = 1;
                //Selected EOL
                eolCharsBox.SelectedIndex = 0;
            }
            
        }

        #region Delegate definitions
        private delegate void RxTextSetter(string text);
        private delegate bool RxFocusChecker();
        private delegate void AutoScroller();
        private delegate void FixedScroller(int sto);
        private delegate void HistoryInserter(string line);
        private delegate void HistoryClearer();
        private delegate int[] NewlineIndexer();
        private delegate int SelectionStartGetter();
        private delegate void AutoscrollEnabler(bool val);
        
        //I HATE CROSS-THREAD ACCESS RESTRICTION
        private delegate void BaudTextSetter(int baud);
        private delegate void ParitySetter(int index);
        private delegate void StopBitsSetter(int index);
        private delegate void EOLSetter(int index);
        private delegate void DataBitsSetter(int index);
        private delegate void RememberPortSetter(bool is_checked);
        #endregion

        private byte[] StoredRxText = new byte[]{}; //Keep this around
        private List<byte> pauseBuffer = new List<byte>();
        private int scrollPosition = 0;
        private int cursorPosition = 0;
        private int selectedIndex = 0;
        private static bool IGNORE;
        private int[] newlines;
        private static string settingsfile;
        //private List<string> storedHistory = new List<string>();

        private static string SettingsDir;

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
            autoscrollBox.Enabled = false;

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

            if (baudRateSetting.Text == "AT_LUDICROUS_SPEED")
            {
                MessageBox.Show("YOU'VE GONE TO PLAID!", "LUDICROUS SPEED", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

            port.DataBits = int.Parse((string)dataBitsList.SelectedItem);

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
                if (!autoscrollBox.Checked)
                    psLineSel.Enabled = true;
                saveSettingsBtn.Enabled = false;
            }
            catch (Exception ex)
            {
                string msg = string.Format("Error opening port.\n.NET Exception: {0}", ex.Message);
                MessageBox.Show(msg, "Error opening port", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            string portFile = Path.Combine(SettingsDir, portNameBox.Text.Replace("/", "_").Replace("COM", "COMM") + ".xml");
                
            if(File.Exists(portFile))
            {
                File.Delete(portFile);
            }

            SaveSettings(portFile);
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
            psLineSel.Enabled = false;
            saveSettingsBtn.Enabled = true;
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            sendData();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            rxDataBox.Text = string.Empty; //Easy peasy.
            StoredRxText = new byte[]{}; //Also kill this
            SetAutoscrollEnabled(false);
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
                    SetRxText(Encoding.ASCII.GetString(StoredRxText));
				}
			}
        }

        private void finalCleanup(object sender, FormClosingEventArgs e)
        {
            //Make sure the port is closed.
            if (port.IsOpen)
                port.Close();
        }

        private string convertToBoth(byte[] equiv)
        {
            //byte[] equiv = Encoding.ASCII.GetBytes(text);
            StringBuilder buffer = new StringBuilder();
            //buffer.Append(' ');
            for (int i = 0; i < equiv.Length; i++)
                buffer.AppendFormat("{0:X2}[{1}] ", equiv[i], equiv[i] > 127 ? '?' : (char)equiv[i]);

            buffer.Remove(buffer.Length - 1, 1);

            buffer.Replace("0D[\r]", "0D[CR]");
            buffer.Replace("0A[\n]", "0A[LF]");
            buffer.Replace("09[\t]", "09[TAB]");
            buffer.Replace("20[ ]", "20[SP]");

            return buffer.ToString();
        }

        private string convertToHex(byte[] equiv)
        {
            //byte[] equiv = Encoding.ASCII.GetBytes(text);
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



            //string newData = port.ReadExisting();
            int N = port.BytesToRead;
            byte[] newData = new byte[N];
            port.Read(newData, 0, N);
            //Append new data, but keep text form handy
            List<byte> combined = new List<byte>(StoredRxText);
            combined.AddRange(newData);
            StoredRxText = combined.ToArray();
            

            //Simple if in text mode.
            if (showTextButton.Checked)
                buffer.Append(Encoding.ASCII.GetString(newData));
            //More complicated for hex mode.
            else if (showBothButton.Checked)
                buffer.Append(convertToBoth(newData));
            else
                buffer.Append(convertToHex(newData));

            scrollPosition = GetSelectionStart();

            if (pauseBox.Checked) //StoredRxText has been updated, so the new text is not lost
            {
                if (pauseBuffer == null)
                    pauseBuffer = new List<byte>(newData);
                else
                    pauseBuffer.AddRange(newData);

                return;
            }
            else
            {
                //Replace text in box with the buffer contents
                SetRxText(buffer.ToString()); 
            }

            newlines = IndexNewlines();
            if(newlines.Length != 0)
                psLineSel.Maximum = newlines.Length-1;

            if (autoscrollBox.Checked)
                AutoScrollerator();
            else
                ScrollTo(scrollPosition);
        }

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

        private int[] IndexNewlines()
        {
            if(this.rxDataBox.InvokeRequired)
            {
                NewlineIndexer ni = new NewlineIndexer(IndexNewlines);
                return (int[])this.Invoke(ni);
            }
            else
            {
                List<int> nlIndeces = new List<int>();
                string text = rxDataBox.Text;
                int NL = text.IndexOf('\n', 0);
                while(NL >= 0)
                {
                    nlIndeces.Add(NL);
                    NL = text.IndexOf('\n', NL + 1);
                }
                return nlIndeces.ToArray();
            }
        }

        private int GetSelectionStart()
        {
            if(this.rxDataBox.InvokeRequired)
            {
                SelectionStartGetter ssg = new SelectionStartGetter(GetSelectionStart);
                return (int)this.Invoke(ssg);
            }
            else
            {
                return rxDataBox.SelectionStart;
            }
        }

        private void SetAutoscrollEnabled(bool val)
        {
            if (autoscrollBox.InvokeRequired)
            {
                AutoscrollEnabler ae = new AutoscrollEnabler(SetAutoscrollEnabled);
                this.Invoke(ae, val);
            }
            else
                autoscrollBox.Enabled = val;
        }

        private void SetBaudRate(int baud)
        {
            if (baudRateSetting.InvokeRequired)
            {
                BaudTextSetter bts = new BaudTextSetter(SetBaudRate);
                this.Invoke(bts, baud);
            }
            else
                baudRateSetting.Text = baud.ToString();
        }

        private void SetParity(int index)
        {
            if (index > 4 || index < 0)
                throw new ArgumentOutOfRangeException("Parity index is out of range.");

            if (parityBox.InvokeRequired)
            {
                ParitySetter ps = new ParitySetter(SetParity);
                this.Invoke(ps, index);
            }
            else
                parityBox.SelectedIndex = index;
        }

        private void SetDataBits(int index)
        {
            if (index > 3 || index < 0)
                throw new ArgumentOutOfRangeException("Data bits index is out of range.");

            if (dataBitsList.InvokeRequired)
            {
                DataBitsSetter dbs = new DataBitsSetter(SetDataBits);
                this.Invoke(dbs, index);
            }
            else
                dataBitsList.SelectedIndex = index;
        }

        private void SetEOL(int index)
        {
            if (index > 2 || index < 0)
                throw new ArgumentOutOfRangeException("EOL character index out of range.");

            if (eolCharsBox.InvokeRequired)
            {
                EOLSetter es = new EOLSetter(SetEOL);
                this.Invoke(es, index);
            }
            else
                eolCharsBox.SelectedIndex = index;
        }

        private void SetStopBits(int index)
        {
            if (index < 0 || index > 3)
                throw new ArgumentOutOfRangeException("Stop bits index out of range");

            if (stopBitsList.InvokeRequired)
            {
                StopBitsSetter sbs = new StopBitsSetter(SetStopBits);
                this.Invoke(sbs, index);
            }
            else
                stopBitsList.SelectedIndex = index;
        }

        private void SetRemember(bool is_checked)
        {
            if (rememberPortSettings.InvokeRequired)
            {
                RememberPortSetter rps = new RememberPortSetter(SetRemember);
                this.Invoke(rps, is_checked);
            }
            else
                rememberPortSettings.Checked = is_checked;
        }
        #endregion

        private void updateBox(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            System.Threading.Thread.Sleep(30);
            updateRxBox();
            SetAutoscrollEnabled(true);
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
                AddToHistory(toSend);
                
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

            //Stop it going out of order.
            txDataBox.SelectedIndex = -1;
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

            if (pauseBuffer.Count == 0)
                return;

            //string bufferedData = pauseBuffer.ToString();
            StringBuilder buffer = new StringBuilder(rxDataBox.Text);

            //Simple if in text mode.
            if (showTextButton.Checked)
                buffer.Append(Encoding.ASCII.GetString(pauseBuffer.ToArray()));
            //More complicated for hex mode.
            else if (showBothButton.Checked)
                buffer.Append(convertToBoth(pauseBuffer.ToArray()));
            else
                buffer.Append(convertToHex(pauseBuffer.ToArray()));

            SetRxText(buffer.ToString());

            pauseBuffer = null; //Null it out. Garbage collector will kill it soon. 
        }

        private void autoscrollBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!autoscrollBox.Checked)
            {
                newlines = IndexNewlines();
                if (newlines.Length == 0)
                    return;
                psLineSel.Maximum = newlines.Length - 1;
                psLineSel.Value = newlines.Length - 1;
                scrollPosition = newlines.Last();
                psLineSel.Enabled = true;
            }
            else
            {
                scrollPosition = rxDataBox.Text.Length;
                psLineSel.Enabled = false;
            }
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

        private void setAutoscroll(object sender, EventArgs e)
        {
            if (IGNORE)
                return;
            scrollPosition = (int)psLineSel.Value;
            ScrollTo(newlines[scrollPosition]);
        }

        private void manualRefresh(object sender, MouseEventArgs e)
        {
            updateRxBox();
        }

        private void changeItem(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                if (txDataBox.SelectedIndex < MAX_HISTORY - 1)
                {
                    txDataBox.SelectedIndex++;
                    txDataBox.SelectionStart = txDataBox.Text.Length;
                    txDataBox.SelectionLength = 0;
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (txDataBox.SelectedIndex >= 0)
                {
                    txDataBox.SelectedIndex--;
                    txDataBox.SelectionStart = txDataBox.Text.Length;
                    txDataBox.SelectionLength = 0;
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else
            {
                e.Handled = false;
                e.SuppressKeyPress = false;
            }
        }

        private void saveSettingsBtn_Click(object sender, EventArgs e)
        {
            SaveSettings(settingsfile, rememberPortSettings.Checked);
        }

        private void SaveSettings(string filename, bool setRemember = false)
        {
            //string settingsfile = Environment.GetEnvironmentVariable("LOCALAPPDATA") + @"\BetterSerialMonitor\settings.xml";
            string parity = parityBox.Text;
            if (parity == "Always 1")
                parity = "one";
            else if (parity == "Always 0")
                parity = "zero";
            string xmlSets = string.Format(
                "<?xml version=\"1.0\"?>\n" +
                "<settings>\n" +
                "    <baud-rate>{0}</baud-rate>\n" +
                "    <parity>{1}</parity>\n" +
                "    <data-bits>{2}</data-bits>\n" +
                "    <stop-bits>{3}</stop-bits>\n" +
                "    <line-end>{4}</line-end>\n" +
                "    <remember-port>{5}</remember-port>\n" +
                "</settings>", baudRateSetting.Text, parity.ToLower(), dataBitsList.SelectedItem, stopBitsList.SelectedItem.ToString().ToLower(), eolCharsBox.SelectedItem, setRemember.ToString().ToLower()); 

            System.IO.File.WriteAllText(filename, xmlSets);
        }

        private void LoadSettings(string filename, bool ignorePorts = false)
        {
            XmlDocument settings = new XmlDocument();
            settings.Load(filename);

            XmlNode root = settings.SelectSingleNode("settings");
            XmlElement temp;

            //Baud rate
            temp = root["baud-rate"];

            int baud;
            if (temp != null)
            {
                bool res = int.TryParse(temp.InnerText, out baud);
                if (!res)
                {
                    MessageBox.Show("Invalid baud rate ({0}) in settings file.".format(baud), "Invalid settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    baud = 9600;
                }
            }
            else
                baud = 9600;
            if (baud % 100 != 0)
            {
                MessageBox.Show("Invalid baud rate ({0}) in settings file.".format(baud), "Invalid settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SetBaudRate(9600);
            }
            else
                SetBaudRate(baud);

            //Parity

            temp = root["parity"];
            string parity;
            if (temp != null)
                parity = temp.InnerText.ToLower();
            else
                parity = "none";

            switch (parity)
            {
                case "none":
                    SetParity(0);
                    break;
                case "even":
                    SetParity(1);
                    break;
                case "odd":
                    SetParity(2);
                    break;
                case "one":
                    SetParity(3);
                    break;
                case "zero":
                    SetParity(4);
                    break;
                default:
                    SetParity(0);
                    MessageBox.Show("Invalid parity ({0}) in settings file.".format(parity), "Invalid settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }

            //Stop bits
            temp = root["stop-bits"];

            float stopBits;
            if (temp != null)
            {
                bool res = float.TryParse(temp.InnerText, out stopBits);
                if (!res)
                {
                    MessageBox.Show("Invalid stop bits ({0}) in settings file.".format(stopBits), "Invalid settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    stopBits = 1;
                }
            }
            else
                stopBits = 0;
            if (stopBits == 0)
                SetStopBits(0);
            else if (stopBits == 1)
                SetStopBits(1);
            else if (stopBits == 1.5)
                SetStopBits(2);
            else if (stopBits == 2)
                SetStopBits(3);
            else
            {
                SetStopBits(3);
                MessageBox.Show("Invalid stop bits ({0}) in settings file.".format(stopBits), "Invalid settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //Data bits
            temp = root["data-bits"];
            int dataBits;
            if (temp != null)
            {
                bool res = int.TryParse(temp.InnerText, out dataBits);
                if (!res)
                {
                    MessageBox.Show("Invalid data bits ({0}) in settings file.".format(dataBits), "Invalid settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dataBits = 8;
                }
            }
            else
                dataBits = 8;

            if (dataBits < 5 || dataBits > 8)
            {
                SetDataBits(3);
                MessageBox.Show("Invalid data bits ({0}) in settings file.".format(dataBits), "Invalid settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                SetDataBits(dataBits - 5);

            //Line ending
            temp = root["line-end"];
            string lineEnds;
            if (temp != null)
                lineEnds = temp.InnerText.ToUpper();
            else
                lineEnds = "CRLF";


            switch (lineEnds)
            {
                case "CRLF":
                    SetEOL(0);
                    break;
                case "CR+LF":
                    SetEOL(0);
                    break;
                case "CR":
                    SetEOL(1);
                    break;
                case "LF":
                    SetEOL(2);
                    break;
                default:
                    MessageBox.Show("Invalid line endings ({0}) in settings file.".format(lineEnds), "Invalid settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SetEOL(0);
                    break;
            }

            if (!ignorePorts)
            {
                temp = root["remember-port"];
                string remember;
                if (temp != null)
                    remember = temp.InnerText.ToLower();
                else
                    remember = "false";

                if (remember == "true")
                    SetRemember(true);
                else if (remember == "false")
                    SetRemember(false);
                else
                    MessageBox.Show("Invalid setting for remembering port settings: \"{0}\"".format(remember), "Invalid port remember setting", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
            }
        }

        private void portNameBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string portFile = Path.Combine(SettingsDir, portNameBox.Text.Replace("/", "_").Replace("COM", "COMM") + ".xml");

            if (File.Exists(portFile))
            {
                if (rememberPortSettings.Checked)
                    LoadSettings(portFile, true);
            }
            else
                LoadSettings(settingsfile, true);
        }

        private void forgetPortBtn_Click(object sender, EventArgs e)
        {
            string portFile = Path.Combine(SettingsDir, portNameBox.Text.Replace("/", "_").Replace("COM", "COMM") + ".xml");

            if (File.Exists(portFile))
                File.Delete(portFile);
        }
    }
}
