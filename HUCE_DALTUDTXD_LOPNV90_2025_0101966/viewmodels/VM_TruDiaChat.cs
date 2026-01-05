using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using TinhToanCoc.Class;
using TinhToanCoc.Pages;

namespace TinhToanCoc.ViewModels
{

    public class VM_TruDiaChat : BaseViewModel
    {
        public ICommand XacNhan { get; set; }
        public VM_TruDiaChat()
        {
            XacNhan = new RelayCommand<TruDiaChat>((p) => true, (p) => btnXacNhan(p));
        }
        private void btnXacNhan(TruDiaChat p)
        {
            string imagePath = "";

            // Xử lý logic chọn ảnh
            switch (p.cbbTenDat.Text)
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

            string thongtinlopdat = p.cbbTenDat.Text + "\n" + "Chiều sâu: " + p.txt_TruDiaChat_DoDay.Text + ", "
                + "N: " + p.txt_TruDiaChat_N.Text + "\n"
                + "Độ sệt: " + p.txt_TruDiaChat_DoSet.Text + ", "
                + "Qc: " + p.txt_TruDiaChat_qc.Text + ", " + "\n" + "Trạng thái: " + p.cbbTrangThai.Text;

            double doday = Convert.ToDouble(p.txt_TruDiaChat_DoDay.Text);
            double N = Convert.ToDouble(p.txt_TruDiaChat_N.Text);
            double doset = Convert.ToDouble(p.txt_TruDiaChat_DoSet.Text);
            double qc = Convert.ToDouble(p.txt_TruDiaChat_qc.Text);
            clsLopDat lopDat = new clsLopDat(p.cbbTenDat.Text, p.cbbLopDat.Text, doday, N, doset, qc, p.cbbTrangThai.Text);
            // Gán đường dẫn ảnh
            switch (p.cbbLopDat.Text)
            {
                case "1":
                    p.SelectedImage1.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(imagePath, UriKind.Relative));
                    p.lblThongTinLop1.Content = thongtinlopdat;
                    clsBienToanCuc.hoSoDat.CacLopDat[0] = lopDat;
                    break;
                case "2":
                    p.SelectedImage2.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(imagePath, UriKind.Relative));
                    p.lblThongTinLop2.Content = thongtinlopdat;
                    clsBienToanCuc.hoSoDat.CacLopDat[1] = lopDat;
                    break;
                case "3":
                    p.SelectedImage3.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(imagePath, UriKind.Relative));
                    p.lblThongTinLop3.Content = thongtinlopdat;
                    clsBienToanCuc.hoSoDat.CacLopDat[2] = lopDat;
                    break;
                case "4":
                    p.SelectedImage4.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(imagePath, UriKind.Relative));
                    p.lblThongTinLop4.Content = thongtinlopdat;
                    clsBienToanCuc.hoSoDat.CacLopDat[3] = lopDat;
                    break;
            }
        }
    }
}
