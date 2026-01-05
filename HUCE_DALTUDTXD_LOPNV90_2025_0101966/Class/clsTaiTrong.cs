using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinhToanCoc.Class
{
    public class clsTaiTrong
    {
        private string _loaiTaiTrong;
        private float _giaTri;

        public string LoaiTaiTrong
        {
            get { return _loaiTaiTrong; }
            set { _loaiTaiTrong = value; }
        }

        public float GiaTri
        {
            get { return _giaTri; }
            set { _giaTri = value; }
        }

        public clsTaiTrong(string loaiTaiTrong, float giaTri)
        {
            _loaiTaiTrong = loaiTaiTrong;
            _giaTri = giaTri;
        }
    }
}
