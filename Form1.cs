using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SteamgameInfo
{
    public partial class Form1 : Form
    {
        clsSteam objstm = clsSteam.getInstance;

        public Form1()
        {
            InitializeComponent();

            InitListView();
        }

        private void InitListView()
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;

            listView1.Columns.Add("appid", 80, HorizontalAlignment.Center);
            listView1.Columns.Add("installdir", 450, HorizontalAlignment.Center);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            objstm.GetGameInstallList();

            //check install game
            for (int i = 0; i < objstm.STEAM_ITEM.Count; i++)
            {
                string strPath = string.Format("{0}\\steamapps\\common\\{1}", objstm.GetSteamPath("SteamPath"), objstm.STEAM_ITEM[i].INSTALLDIR);
                if (Directory.Exists(strPath))
                {
                    //Console.WriteLine("{0}:{1}", objstm.STEAM_ITEM[i].APPID, objstm.STEAM_ITEM[i].INSTALLDIR);
                    ListViewItem item = new ListViewItem();
                    item.Text = objstm.STEAM_ITEM[i].APPID;
                    item.SubItems.Add(objstm.STEAM_ITEM[i].INSTALLDIR);
                    listView1.Items.Add(item);
                }
            }  
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "" || txtPassword.Text == "")
                return;

            objstm.StartSteam(txtID.Text, txtPassword.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            objstm.StopSteam();
        }
    }
}
