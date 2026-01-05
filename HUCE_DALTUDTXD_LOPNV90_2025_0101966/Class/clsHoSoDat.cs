using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinhToanCoc.Class
{
    public class clsHoSoDat
    {
        private List<clsLopDat> _cacLopDat;

        public List<clsLopDat> CacLopDat
        {
            get { return _cacLopDat; }
            set { _cacLopDat = value; }
        }

        public clsHoSoDat()
        {
            clsLopDat lop1 = new clsLopDat();
            clsLopDat lop2 = new clsLopDat();
            clsLopDat lop3 = new clsLopDat();
            clsLopDat lop4 = new clsLopDat();
            _cacLopDat = new List<clsLopDat>() { lop1, lop2, lop3, lop4 };
        }

        public void ThemLopDat(clsLopDat lopDat)
        {
            _cacLopDat.Add(lopDat);
        }

        public clsLopDat LayThongTinLopDat(string sttlopdat)
        {
            return _cacLopDat.Find(ld => ld.STTLopDat == sttlopdat);
        }
    }
}
