using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TinhToanCoc.Class;
using TinhToanCoc.Pages;

namespace TinhToanCoc.ViewModels
{
    public class VM_KhaiBaoCoc : BaseViewModel
    {
        List<clsCoc> danhSachCoc = new List<clsCoc>();
        public ICommand luucoc { get; set; }
        public VM_KhaiBaoCoc()
        {
            luucoc = new RelayCommand<KhaiBaoCoc>((p) => true, (p) => LuuCoc(p));
        }
        public void LuuCoc(KhaiBaoCoc p)
        {
            clsCoc CocMoi = new clsCoc(p.txtCocName.Text, Convert.ToDouble(p.txtCocLength.Text), Convert.ToDouble(p.txtCocDiameter.Text), p.cbbMaterialType.Text);
            danhSachCoc.Add(CocMoi);
            clsBienToanCuc.ListCoc.Clear();
            foreach (clsCoc cocmoi in danhSachCoc)
            {
                clsBienToanCuc.ListCoc.Add(cocmoi);
                //p.cbbLoaiCoc.Items.Add(cocmoi.Name);
            }
            MessageBox.Show("Thêm cọc mới thành công");
        }
    }
}
