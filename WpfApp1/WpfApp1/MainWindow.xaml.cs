using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfApp1
{    
    public partial class MainWindow : Window
    {
        private clsSteam objstm;

        public MainWindow()
        {
            InitializeComponent();

            objstm = clsSteam.getInstance;

            //리스트 헤드 설정
            InitListView();
        }

        private void InitListView()
        {
            GridView gridView = new GridView();

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "앱ID",
                DisplayMemberBinding = new Binding("APPID"),
                Width = 100
            });

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "설치 경로",
                DisplayMemberBinding = new Binding("INSTALLDIR"),
                Width = 400
            });

            steamListView.View = gridView;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            objstm.GetGameInstallList();

            // check install game
            foreach (var steamItem in objstm.STEAM_ITEM)
            {
                string strPath = $"{objstm.GetSteamPath("SteamPath")}\\steamapps\\common\\{steamItem.INSTALLDIR}";
                if (System.IO.Directory.Exists(strPath))
                {
                    steamListView.Items.Add(steamItem);
                }
            }
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text) || string.IsNullOrEmpty(txtPassword.Password))
                return;

            objstm.StartSteam(txtID.Text, txtPassword.Password);
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            objstm.StopSteam();
        }

    }
}
