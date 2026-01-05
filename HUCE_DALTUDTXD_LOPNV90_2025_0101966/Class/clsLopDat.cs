using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinhToanCoc.Class
{
    public class clsLopDat
    {
        private string _tenDat;
        private string _trangThai;
        private string _sttLopDat;
        private double _doDay;
        private double _n;
        private double _doSet;
        private double _gocMaSat;
        private double _qc;

        public double QC
        {
            get { return _qc; }
            set { _qc = value; }
        }

        public string TrangThai
        {
            get { return _trangThai; }
            set { _trangThai = value; }
        }
        public string TenDat
        {
            get { return _tenDat; }
            set { _tenDat = value; }
        }

        public string STTLopDat
        {
            get { return _sttLopDat; }
            set { _sttLopDat = value; }
        }

        public double DoDay
        {
            get { return _doDay; }
            set { _doDay = value; }
        }

        public double N
        {
            get { return _n; }
            set { _n = value; }
        }

        public double DoSet
        {
            get { return _doSet; }
            set { _doSet = value; }
        }

        public double GocMaSat
        {
            get { return _gocMaSat; }
            set { _gocMaSat = value; }
        }

        public clsLopDat()
        {
            _tenDat = "";
            _doDay = 0;
            _sttLopDat = "";
            _doSet = 0;
            _qc = 0;
            _trangThai = "";
            _n = 0;
        }

        public clsLopDat(string tendat, string stt, double doday, double N, double lucDinh, double qc, string trangthai)
        {
            _tenDat = tendat;
            _doDay = doday;
            _sttLopDat = stt;
            _doSet = lucDinh;
            _qc = qc;
            _trangThai = trangthai;
            _n = N;
        }
    }
}
