using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Forms;
using WMPLib;

namespace FileManipulator
{
    public partial class Form1 : Form
    {
        WindowsMediaPlayer player = new WindowsMediaPlayer();
        public Form1()
        {
            InitializeComponent();
            player.URL = "guile.mp3";
        }
        OpenFileDialog sourceDialog = new OpenFileDialog();
        OpenFileDialog exeDialog = new OpenFileDialog();
        FolderBrowserDialog destinationFolderDialog = new FolderBrowserDialog();
        FolderBrowserDialog sourceFolderDialog = new FolderBrowserDialog();
        bool btnFileChooserClickChecker = false;

        private void btnFileChooser_Click(object sender, EventArgs e)
        {
            btnFileChooserClickChecker = true;
            if (sourceDialog.ShowDialog() == DialogResult.OK)
            {
                txtSource.Text = sourceDialog.FileName;
            }
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            if(btnFileChooserClickChecker)
            {
                File.Copy(txtSource.Text, txtDestination.Text + "\\" + sourceDialog.SafeFileName, true);
                MessageBox.Show("AKTARIM BAŞARIYLA TAMAMLANDI");
            }
            else
            {
                String[] fileList = Directory.GetFiles(sourceFolderDialog.SelectedPath);

                foreach (String filePath in fileList)
                {
                    File.Copy(filePath, txtDestination.Text + "\\" + Path.GetFileName(filePath), true);
                }
                    MessageBox.Show("AKTARIM BAŞARILIYLA TAMAMLANDI");
            }
        }

        private void btnDestination_Click(object sender, EventArgs e)
        {
            if (destinationFolderDialog.ShowDialog() == DialogResult.OK)
            {
                txtDestination.Text = destinationFolderDialog.SelectedPath;
            }
        }

        private void btnDirChooser_Click(object sender, EventArgs e)
        {
            if(sourceFolderDialog.ShowDialog() == DialogResult.OK)
            {
                txtSource.Text = sourceFolderDialog.SelectedPath;
            }
        }

        private void btnStartup_Click(object sender, EventArgs e)
        {
            exeDialog.Filter = "Başlangıç Dosyaları (.exe;.bat;.vbs)|*.exe;*.bat;*.vbs|All files|*.*";
            if(exeDialog.ShowDialog() == DialogResult.OK)
            {
                RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                reg.SetValue(exeDialog.SafeFileName, exeDialog.FileName);
                MessageBox.Show("DOSYA BAŞLANGIÇTA ÇALIŞMAK İÇİN KAYDEDİLDİ");
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            bool mousePointerNotOnTaskBar = Screen.GetWorkingArea(this).Contains(Cursor.Position);
            if(this.WindowState == FormWindowState.Minimized && mousePointerNotOnTaskBar)
            {
                notifyIcon1.ShowBalloonTip(1000, "Askra Bildirimi", "Uygulama Sistem Tepsisine Küçültüldü", ToolTipIcon.Info);
                this.ShowInTaskbar = false;
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            if(this.WindowState == FormWindowState.Normal)
            {
                this.ShowInTaskbar = true;
                notifyIcon1.Visible = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            player.controls.play();
        }
    }
}
