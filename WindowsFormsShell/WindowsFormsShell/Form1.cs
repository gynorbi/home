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
        EventBasedProvider.Provider provider;
        public Form1()
        {
            InitializeComponent();
            provider = new EventBasedProvider.Provider();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            listBox1.Items.Add($"Current count in data: {provider.Count}");
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Run(
                () => provider.ReadAllFiles().Wait()
            );
        }
    }
}
