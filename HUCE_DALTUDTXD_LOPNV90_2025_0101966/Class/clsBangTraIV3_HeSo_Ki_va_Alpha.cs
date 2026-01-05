using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinhToanCoc.Class
{
    public class clsBangTraIV3_HeSo_Ki_va_Alpha
    {
        public string LoaiDat { get; set; } // Loại đất
        public double qcMin { get; set; } // Giá trị tối thiểu của qc (kPa)
        public double qcMax { get; set; } // Giá trị tối đa của qc (kPa)
        public double AlphaKhoan { get; set; } // Hệ số Alpha cho cọc khoan
        public double AlphaDong { get; set; } // Hệ số Alpha cho cọc đóng
        public double KiCocKhoan { get; set; } // Hệ số k cho cọc khoan
        public double KiCocDong { get; set; } // Hệ số k cho cọc đóng
        public int ToMaxKhoan { get; set; } // Giá trị cực đại của To cho cọc khoan
        public int ToMaxDong { get; set; } // Giá trị cực đại của To cho cọc đóng

        public clsBangTraIV3_HeSo_Ki_va_Alpha(string loaidat, double qcmin, double qcmax, double kicockhoan, double kicocdong, double alphakhoan, double alphadong,
                                   int tomaxkhoan, int tomaxdong)
        {
            LoaiDat = loaidat;
            qcMin = qcmin;
            qcMax = qcmax;
            AlphaKhoan = alphakhoan;
            AlphaDong = alphadong;
            KiCocKhoan = kicockhoan;
            KiCocDong = kicocdong;
            ToMaxKhoan = tomaxkhoan;
            ToMaxDong = tomaxdong;
        }

    }
    public class ThongSo
    {
        public double KiCocDong { get; set; } // Hệ số k cho cọc đóng
        public double AlphaDong { get; set; } // Hệ số Alpha cho cọc đóng
        public int ToMaxDong { get; set; } // Giá trị cực đại của To cho cọc đóng

        public ThongSo(double ki,double alpha,int tomax) 
        {
            KiCocDong = ki;
            AlphaDong = alpha;
            ToMaxDong = tomax;
        }

    }
}
