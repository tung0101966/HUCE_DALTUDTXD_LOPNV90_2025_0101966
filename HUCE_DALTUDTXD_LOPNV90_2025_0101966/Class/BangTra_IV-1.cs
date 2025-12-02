using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinhToanCoc.Class
{
    public class clsBangTra_IV_1
    {
        public int ChieuSau { get; set; }        // Chiều sâu
        public double[] GiaTri { get; set; }    // Mảng giá trị T_i theo I_L

        // Constructor
        public clsBangTra_IV_1(int chieuSau, double[] giaTri)
        {
            ChieuSau = chieuSau;
            GiaTri = giaTri;
        }
    }
}
