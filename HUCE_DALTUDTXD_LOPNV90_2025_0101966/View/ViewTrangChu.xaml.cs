using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TinhToanCoc.Class;

namespace TinhToanCoc.View
{
    /// <summary>
    /// Interaction logic for ViewTrangChu.xaml
    /// </summary>
    public partial class ViewTrangChu : Window
    {
        clsHoSoDat hoSoDat = new clsHoSoDat();
        

        public ViewTrangChu()
        {
            InitializeComponent();
        }

        private void btnXacNhan_Click(object sender, RoutedEventArgs e)
        {
            string imagePath = "";

            // Xử lý logic chọn ảnh
            switch (cbbTenDat.Text)
            {
                case "Sét":
                    imagePath = "/Resources/KyHieuLopDat/Set.png";
                    break;
                case "Sét Pha":
                    imagePath = "/Resources/KyHieuLopDat/SetPha.png";
                    break;
                case "Cát":
                    imagePath = "/Resources/KyHieuLopDat/Cat.png";
                    break;
                case "Cát Pha":
                    imagePath = "/Resources/KyHieuLopDat/CatPha.png";
                    break;
            }

            string thongtinlopdat =cbbTenDat.Text +"\n"+ "Chiều sâu: " + txt_TruDiaChat_DoDay.Text + ", "
                + "N: " + txt_TruDiaChat_N.Text + "\n"
                + "Độ sệt: " + txt_TruDiaChat_DoSet.Text + ", "
                + "Qc: " + txt_TruDiaChat_qc.Text + ", " +"\n" + "Trạng thái: "+ cbbTrangThai.Text;
            
            double doday = Convert.ToDouble(txt_TruDiaChat_DoDay.Text);
            double N = Convert.ToDouble(txt_TruDiaChat_N.Text);
            double doset = Convert.ToDouble(txt_TruDiaChat_DoSet.Text);
            double qc = Convert.ToDouble(txt_TruDiaChat_qc.Text);
            clsLopDat lopDat = new clsLopDat(cbbTenDat.Text,cbbLopDat.Text, doday,N,doset,qc,cbbTrangThai.Text);
            // Gán đường dẫn ảnh
            switch (cbbLopDat.Text)
            {
                case "1":
                    SelectedImage1.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(imagePath, UriKind.Relative));
                    lblThongTinLop1.Content = thongtinlopdat;
                    hoSoDat.CacLopDat[0] = lopDat;
                    break;
                case "2":
                    SelectedImage2.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(imagePath, UriKind.Relative));
                    lblThongTinLop2.Content = thongtinlopdat;
                    hoSoDat.CacLopDat[1] = lopDat;
                    break;
                case "3":
                    SelectedImage3.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(imagePath, UriKind.Relative));
                    lblThongTinLop3.Content = thongtinlopdat;
                    hoSoDat.CacLopDat[2] = lopDat;
                    break;
                case "4":
                    SelectedImage4.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(imagePath, UriKind.Relative));
                    lblThongTinLop4.Content = thongtinlopdat;
                    hoSoDat.CacLopDat[3] = lopDat;
                    break;
            }
        }
    }

}
