using CommonThings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsShell
{
    public partial class Form1 : Form
    {
        IProvider provider;
        List<long> avg1, avg2, avg3, avg4;
        public Form1()
        {
            InitializeComponent();
            avg1 = new List<long>();
            avg2 = new List<long>();
            avg3 = new List<long>();
            avg4 = new List<long>();

            if (radioButton1.Enabled)
            {
                provider = new EventBasedProvider.Provider();
            }
            else
            {
                provider = new ConcDictProvider.Provider();
            }
            timer1.Interval = timer2.Interval = timer3.Interval = timer4.Interval = (int)numericUpDown1.Value;
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            OnTick(listBox1, textBox1, avg1, avgTextBox1);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            OnTick(listBox2, textBox2, avg2, avgTextBox2);
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            OnTick(listBox3, textBox3, avg3, avgTextBox3);
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            OnTick(listBox4, textBox4, avg4, avgTextBox4);
        }

        private void OnTick(ListBox currentListBox, TextBox currentTextBox, List<long> avg, TextBox avgTextBox)
        {
            var t = Task.Run(() =>
            {
                int count = 0;
                long countDuration = 0;
                var message = string.Empty;
                int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                try
                {
                    countDuration = SwTimer.Time(() => count = provider.Count);
                    return new { count, countDuration, message, threadId };
                }
                catch (Exception ex)
                {
                    message = $"Error: {ex.Message}, {ex.InnerException?.Message}";
                    return new { count, countDuration, message, threadId };
                }
            }).Result;
            if (t.message == string.Empty)
            {
                avg.Add(t.countDuration);
                avgTextBox.Text = avg.Count > 0 ? ((double)avg.Sum() / avg.Count).ToString("0.0000") : "0";
                currentTextBox.Text = $"{t.count} ({t.countDuration}ms) ThrdId: {t.threadId}";
            }
            else
            {
                currentListBox.Items.Add(t.message);
                currentListBox.SelectedIndex = currentListBox.Items.Count - 1;
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var duration = await SwTimer.Time(async () =>
                await Task.Run(
                    () => provider.ReadAllFiles().Wait()
            ));
            var message = $"ReadAllFiles duration: {duration}ms";
            listBox1.Items.Add(message);
            listBox2.Items.Add(message);
            listBox3.Items.Add(message);
            listBox4.Items.Add(message);
        }



        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = timer2.Interval = timer3.Interval = timer4.Interval = (int)((NumericUpDown)sender).Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            provider.Clear();
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            avg1.Clear();
            avg2.Clear();
            avg3.Clear();
            avg4.Clear();
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sender.Equals(radioButton1) && radioButton1.Enabled)
            {
                provider = new EventBasedProvider.Provider();
            }
            else if (sender.Equals(radioButton2) && radioButton2.Enabled)
            {
                provider = new ConcDictProvider.Provider();
            }
        }
    }
}
