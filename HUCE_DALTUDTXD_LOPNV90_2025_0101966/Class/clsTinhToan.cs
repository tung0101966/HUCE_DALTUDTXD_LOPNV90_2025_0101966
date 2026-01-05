using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinhToanCoc.Class
{
    public class clsTinhToan
    {
        private clsCoc _coc;
        private clsHoSoDat _hoSoDat;
        private float _sucChiuTai;

        public clsCoc Coc
        {
            get { return _coc; }
            set { _coc = value; }
        }

        public clsHoSoDat HoSoDat
        {
            get { return _hoSoDat; }
            set { _hoSoDat = value; }
        }

        public float SucChiuTai
        {
            get { return _sucChiuTai; }
            set { _sucChiuTai = value; }
        }

        public clsTinhToan(clsCoc coc, clsHoSoDat hoSoDat, float sucChiuTai)
        {
            _coc = coc;
            _hoSoDat = hoSoDat;
            _sucChiuTai = sucChiuTai;
        }

        public float TinhToanSucChiuTai()
        {
            // Logic tính toán sức chịu tải
            return 0f; // Giá trị trả về giả định
        }

        public bool KiemTraAnToan()
        {
            // Logic kiểm tra an toàn
            return true; // Giá trị trả về giả định
        }
    }
}
