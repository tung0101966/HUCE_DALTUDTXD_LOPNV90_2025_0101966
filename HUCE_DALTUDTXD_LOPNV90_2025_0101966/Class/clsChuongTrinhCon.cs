using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinhToanCoc.Class
{
    public class clsChuongTrinhCon
    {
        public double TinhPvl(double m, double hesoUonDoc, double Rb, double Ab, double Rs, double As)
        {
            return (m * hesoUonDoc * (Rb * Ab + Rs * As)) / 1.6;
        }

        public double PhuongPhapThongKe(double Wc, double u, List<double> TI, List<List<double>> LI, double Fc)
        {
            List<double> Li = new List<double>();

            foreach (List<double> li in LI)
            {
                foreach (double giatri in li)
                {
                    Li.Add(giatri);
                }
            }
            double sum = 0.0;
            for (int i = 0; i < TI.Count && i < Li.Count; i++)
            {
                sum += u * TI[i] * Li[i];
            }
            double Pdn = sum + Fc * 3750;
            return ((sum + Fc * 3750) - Wc) / 1.6;
        }

        public double PPXuyenTinhCPT(double Wc, double u, List<double> Li, List<Thongso> ThongSo, double Fc, List<double> qc) // thông số được lấy từ class bảng tra và nội suy
        {
            double summation = 0;
            double Ri = 0;
            for (int i = 0; i < Li.Count && i < ThongSo.Count; i++)
            {
                double To = qc[i] / ThongSo[i].AlphaDong;
                if (To <= ThongSo[i].ToMaxDong)
                {
                    summation += u * Li[i] * To;
                }
                else
                {
                    summation += u * Li[i] * ThongSo[i].ToMaxDong;
                }
                if (i == ThongSo.Count - 1)
                {
                    Ri = ThongSo[i].KiCocDong * qc[i];
                }
            }
            double Pdn = summation + Fc * Ri;
            double Pgh = Pdn - Wc;
            return Pgh / 1.6;
        }

        public double PPXuyenTieuChuanSPT(List<double> N, List<double> Li, List<double> To, double u, double Wc, double Fc, double daicoc)
        {
            double sum = 0;
            double Ri = 0;
            for (int i = 0; i < Li.Count && i < N.Count; i++)
            {
                sum += u * Li[i] * To[i];
                if (i == N.Count - 1)
                {
                    Ri = 400 * N[i];
                }
            }
            double Pdn = sum + Fc * Ri / (daicoc - 0.5);
            double Pgh = Pdn - Wc;
            return Pgh / 1.6;
        }
    }
}
