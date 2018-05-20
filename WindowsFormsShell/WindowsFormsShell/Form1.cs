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
        List<List<long>> avgerages;
        List<Task<long>> tasks;
        public Form1()
        {
            InitializeComponent();
            avgerages = new List<List<long>>();
            tasks = new List<Task<long>>();

            if (radioButton1.Enabled)
            {
                provider = new EventBasedProvider.Provider();
            }
            else
            {
                provider = new ConcDictProvider.Provider();
            }
            for (int i = 0; i < numericUpDown1.Value; i++)
            {

                tasks.Add(RunOneQuery());
            }
        }

        private Task<long> RunOneQuery()
        {
            return SwTimer.Time(
                () => Task.Run(
                    () => provider.Count
                    )
                );
        }


        private void OnTick()
        {
            Task.WhenAll(tasks).ContinueWith((durations) =>
            {
                var durationValues = durations.Result;
                for (int i = 0; i < durationValues.Length; i++)
                {
                    avgerages[i].Add(durationValues[i]);
                }
            });

            //var t = Task.Run(() =>
            //{
            //    int count = 0;
            //    long countDuration = 0;
            //    var message = string.Empty;
            //    int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            //    try
            //    {
            //        countDuration = SwTimer.Time(() => count = provider.Count);
            //        return new { count, countDuration, message, threadId };
            //    }
            //    catch (Exception ex)
            //    {
            //        message = $"Error: {ex.Message}, {ex.InnerException?.Message}";
            //        return new { count, countDuration, message, threadId };
            //    }
            //}).Result;
            //if (t.message == string.Empty)
            //{
            //    avg.Add(t.countDuration);
            //    avgTextBox.Text = avg.Count > 0 ? ((double)avg.Sum() / avg.Count).ToString("0.0000") : "0";
            //    currentTextBox.Text = $"{t.count} ({t.countDuration}ms) ThrdId: {t.threadId}";
            //}
            //else
            //{
            //    currentListBox.Items.Add(t.message);
            //    currentListBox.SelectedIndex = currentListBox.Items.Count - 1;
            //}
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var duration = await SwTimer.Time(async () =>
                await Task.Run(
                    () => provider.ReadAllFiles().Wait()
            ));
            var message = $"ReadAllFiles duration: {duration}ms";
        }



        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            Task.WaitAll(tasks.ToArray());
            if (tasks.Count < numericUpDown1.Value)
            {


                for (int i = 0; i < numericUpDown1.Value - tasks.Count; i++)
                {
                    tasks.Add(RunOneQuery());
                }
            }
            else
            {
                tasks.RemoveRange(0, tasks.Count - (int)numericUpDown1.Value);
            }
            timer1.Enabled = true;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            provider.Clear();
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            OnTick();
        }
    }
}
