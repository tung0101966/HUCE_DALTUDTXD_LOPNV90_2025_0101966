using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinhToanCoc.Class
{
    public class clsCoc
    {
        private string _name;
        private double _chieuDai;
        private double _duongKinh;
        private string _vatLieu;

        public double ChieuDai
        {
            get { return _chieuDai; }
            set { _chieuDai = value; }
        }

        public double DuongKinh
        {
            get { return _duongKinh; }
            set { _duongKinh = value; }
        }

        public string VatLieu
        {
            get { return _vatLieu; }
            set { _vatLieu = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public clsCoc()
        {
            _chieuDai = 0.0;
            _duongKinh = 0.0;
            _vatLieu = "";
            _name = "";
        }

        public clsCoc(string name, double chieuDai, double duongKinh, string vatLieu)
        {
            _chieuDai = chieuDai;
            _duongKinh = duongKinh;
            _vatLieu = vatLieu;
            _name = name;
        }
    }
}
