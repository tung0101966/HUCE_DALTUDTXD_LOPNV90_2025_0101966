using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TinhToanCoc.Class;
using TinhToanCoc.Pages;

namespace TinhToanCoc.View
{
    /// <summary>
    /// Interaction logic for ViewTrangChu.xaml
    /// </summary>
    public partial class ViewTrangChu : Window
    {
        //clsHoSoDat hoSoDat = new clsHoSoDat();
        

        public ViewTrangChu()
        {
            InitializeComponent();
            frame_Body.Content = new TinhToanDaiCoc();
        }

        private void btnKhaiBaoCoc_Click(object sender, RoutedEventArgs e)
        {
            frame_Body.Content = new KhaiBaoCoc();
        }

        private void btnTinhToanCoc_Click(object sender, RoutedEventArgs e)
        {
            frame_Body.Content = new pageTinhToanCoc();
        }

        private void btnTruDiaChat_Click(object sender, RoutedEventArgs e)
        {
            frame_Body.Content = new TruDiaChat();
        }

        private void btnTinhToanDaiCoc_Click(object sender, RoutedEventArgs e)
        {
            frame_Body.Content = new TinhToanDaiCoc();

        }
    }

}
