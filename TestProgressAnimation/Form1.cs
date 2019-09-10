using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestProgressAnimation
{
    public partial class Form1 : Form
    {
        private int cnt = 0;

        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            this.StopTextLabel();
            this.VisibleProgressImage(false);
            // スレッドからの進捗通知を可能にする(これはデザイナでも変更可能)
            this.backgroundWorker1.WorkerReportsProgress = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.button1.Enabled = false;
            this.StopTextLabel();
            this.VisibleProgressImage(true);
            this.progressBar1.Value = 0;
            this.backgroundWorker1.RunWorkerAsync();
        }

        private void StopTextLabel() => this.label1.Text = "停止中";
        private void VisibleProgressImage(bool isVisible) => pictureProgress.Visible = isVisible;

        private void Processing()
        {
            Thread.Sleep(100);
        }

        // 別スレッドでの動作
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for ( int i = 1; i <= 100; i++ )
            {
                this.cnt = i;
                this.Processing();

                int progress = this.cnt;
                string msg = "実行中... ";
                this.backgroundWorker1.ReportProgress(progress, msg);
            }
        }

        // スレッドの進捗を受け取る
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.label1.Text = (string)e.UserState + " " + e.ProgressPercentage.ToString() + "%";
            this.progressBar1.Value = e.ProgressPercentage;
        }

        // スレッド完了通知を受け取る
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("処理完了！");
            this.VisibleProgressImage(false);
            this.StopTextLabel();
            this.button1.Enabled = true;
        }

    }
}
