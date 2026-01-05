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
    public class VM_TinhToanCoc : BaseViewModel
    {
        public ICommand TTCOC { get; set; }
        public VM_TinhToanCoc()
        {
            TTCOC = new RelayCommand<pageTinhToanCoc>((p) => true, (p) => btnTinhToanCoc(p));
        }
        private void btnTinhToanCoc(pageTinhToanCoc p)
        {
            #region Tính P vật liệu
            clsChuongTrinhCon clsChuongTrinhCon = new clsChuongTrinhCon();
            double d = Convert.ToDouble(p.txtD.Text); // đương kính thép
            double m = 0.9;// hệ số làm việc
            double hesouondoc = 1;
            double Rb = Convert.ToDouble(p.txtRb.Text); //T/m2
            double Rs = Convert.ToDouble(p.txtRs.Text); //T/m2
            double As = Math.PI * Math.Pow(d, 2);
            double Ab = 1;
            double KQPghVatLieu = clsChuongTrinhCon.TinhPvl(m, hesouondoc, Rb, Ab, Rs, As);
            p.lblKQPghVatLieu.Content = Convert.ToString(clsChuongTrinhCon.TinhPvl(m, hesouondoc, Rb, Ab, Rs, As));
            #endregion

            #region Tính theo pp thống kê
            List<List<double>> doSauBinhQuan = TinhDoSauBinhQuan(p);
            List<double> giatri = new List<double>();
            for (int i = 0; i < clsBienToanCuc.hoSoDat.CacLopDat.Count; i++)
            {
                double doset = clsBienToanCuc.hoSoDat.CacLopDat[i].DoSet;
                for (int j = 0; j < doSauBinhQuan[i].Count; j++)
                {
                    double dsbq = doSauBinhQuan[i][j];
                    double giaTri = NoiSuy2Chieu(traBang, dsbq, doset);
                    giatri.Add(giaTri);
                }
            }
            clsCoc coc = TimCocDuocChon(p);
            double Fc = coc.DuongKinh * 0.01 * coc.DuongKinh * 0.01;
            double u = coc.DuongKinh * 0.01 * 4;
            double Wc = Fc * (coc.ChieuDai - 0.5) * coc.DuongKinh;
            List<List<double>> cocdachia = ChiaCocTheoPPThongKe(p);
            double kqqq = clsChuongTrinhCon.PhuongPhapThongKe(Wc, u, giatri, doSauBinhQuan, Fc);
            p.lblPghPPTK.Content = Convert.ToString(clsChuongTrinhCon.PhuongPhapThongKe(Wc, u, giatri, cocdachia, Fc));
            #endregion

            #region Tính theo Thí nghiệm xuyên tĩnh CPT
            List<double> doancoc = ChiaCocTheoCPT(p);
            List<double> lstQc = new List<double>();
            List<ThongSo> thongSo = new List<ThongSo>();
            for (int i = 0; i < doancoc.Count; i++)
            {
                double qc = clsBienToanCuc.hoSoDat.CacLopDat[i].QC * 100;
                lstQc.Add(qc);
                string trangthai = clsBienToanCuc.hoSoDat.CacLopDat[i].TrangThai;
                foreach (clsBangTraIV3_HeSo_Ki_va_Alpha row in bangTraIV_3)
                {
                    if (qc < row.qcMax && qc > row.qcMin && trangthai == row.LoaiDat)
                    {
                        thongSo.Add(new ThongSo(row.KiCocDong, row.AlphaDong, row.ToMaxDong));
                        break;
                    }
                }
            }
            double kqCPT = clsChuongTrinhCon.PPXuyenTinhCPT(Wc, u, doancoc, thongSo, Fc, lstQc);
            p.lblPghCPT.Content = Convert.ToString(kqCPT);
            #endregion

            #region Tính theo thí nghiệm SPT
            List<double> doancocSPT = ChiaCocTheoCPT(p);
            List<double> To = new List<double>();
            List<double> N = new List<double>();
            for (int i = 0; i < doancocSPT.Count; i++)
            {
                double to = 0.2 * clsBienToanCuc.hoSoDat.CacLopDat[i].N;
                To.Add(to);
                N.Add(clsBienToanCuc.hoSoDat.CacLopDat[i].N);
            }
            double kqSPT = clsChuongTrinhCon.PPXuyenTieuChuanSPT(N, doancocSPT, To, u, Wc, Fc, coc.ChieuDai);
            p.lblPghSPT.Content = Convert.ToString(kqSPT);
            #endregion
        }

        #region Tính độ sâu bình quân
        public List<List<double>> TinhDoSauBinhQuan(pageTinhToanCoc p)
        {
            List<List<double>> DoSauBinhQuan = new List<List<double>>();
            List<List<double>> cocdachia = ChiaCocTheoPPThongKe(p);
            for (int i = 0; i < cocdachia.Count; i++)
            {
                double sum = 0;
                if (i == 0)
                {
                    sum = Convert.ToDouble(p.txtCaoTrinhDayDai.Text);
                }
                else if (i == 1)
                {
                    sum = clsBienToanCuc.hoSoDat.CacLopDat[i - 1].DoDay;
                }
                else if (i == 2)
                {
                    sum = clsBienToanCuc.hoSoDat.CacLopDat[i - 1].DoDay + clsBienToanCuc.hoSoDat.CacLopDat[i - 2].DoDay;
                }
                else if (i == 3)
                {
                    sum = clsBienToanCuc.hoSoDat.CacLopDat[i - 1].DoDay + clsBienToanCuc.hoSoDat.CacLopDat[i - 2].DoDay + clsBienToanCuc.hoSoDat.CacLopDat[i - 3].DoDay;
                }
                List<double> list = new List<double>();
                for (int j = 0; j < cocdachia[i].Count; j++)
                {
                    if (j == 0)
                    {
                        double dsbq = cocdachia[i][j] / 2;
                        sum += dsbq;
                        list.Add(sum);
                    }
                    else
                    {
                        sum += cocdachia[i][j - 1] / 2 + cocdachia[i][j] / 2;
                        list.Add(sum);
                    }
                }
                DoSauBinhQuan.Add(list);
            }
            return DoSauBinhQuan;
        }
        #endregion

        #region Chia cọc theo phương pháp thống kê
        public List<List<double>> ChiaCocTheoPPThongKe(pageTinhToanCoc p)
        {
            List<double> doDaiDep = new List<double> { 0.5, 1, 1.5, 2 };
            List<List<double>> cacDoanCoc = new List<List<double>>();
            clsCoc Coc1 = TimCocDuocChon(p);
            if (Coc1.ChieuDai != 0)
            {
                double chieudaicoc = Coc1.ChieuDai - 0.5;
                for (int i = 0; i < clsBienToanCuc.hoSoDat.CacLopDat.Count; i++)
                {
                    double dodaylopdat = clsBienToanCuc.hoSoDat.CacLopDat[i].DoDay;
                    if (dodaylopdat > 0)
                    {
                        double chieuDaiConLai = 0;
                        if (chieudaicoc > dodaylopdat)
                        {
                            if (i == 0)
                            {
                                chieuDaiConLai = dodaylopdat - Convert.ToDouble(p.txtCaoTrinhDayDai.Text);
                            }
                            else
                            {
                                chieuDaiConLai = dodaylopdat;
                            }
                        }
                        else if (chieudaicoc < dodaylopdat)
                        {
                            chieuDaiConLai = chieudaicoc;
                        }
                        List<double> dodai = new List<double>();
                        // Sắp xếp danh sách độ dài "đẹp" từ lớn đến nhỏ
                        doDaiDep.Sort((a, b) => b.CompareTo(a));

                        while (chieuDaiConLai > 0)
                        {
                            // Tìm đoạn dài nhất phù hợp
                            double doDaiChon = doDaiDep.FirstOrDefault(d => d <= chieuDaiConLai);

                            if (doDaiChon > 0)
                            {
                                dodai.Add(doDaiChon);
                                chieuDaiConLai -= doDaiChon;
                            }
                            else
                            {
                                // Nếu không tìm được đoạn phù hợp (do lỗi làm tròn), thêm phần dư và thoát
                                dodai.Add(chieuDaiConLai);
                                chieuDaiConLai = 0;
                            }
                        }
                        cacDoanCoc.Add(dodai);
                        if (i == 0)
                        {
                            chieudaicoc -= dodaylopdat - Convert.ToDouble(p.txtCaoTrinhDayDai.Text);
                        }
                        else
                        {
                            chieudaicoc -= dodaylopdat;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Độ day lớp đất phải lớn hơn 0");
                    }
                }
            }
            else
            {
                return null;
            }
            return cacDoanCoc;
        }
        #endregion

        #region Chia cọc theo phương pháp CPT
        public List<double> ChiaCocTheoCPT(pageTinhToanCoc p)
        {
            List<double> cacDoanCoc = new List<double>();
            clsCoc Coc1 = TimCocDuocChon(p);
            if (Coc1.ChieuDai != 0)
            {
                double chieudaicoc = Coc1.ChieuDai - 0.5;
                for (int i = 0; i < clsBienToanCuc.hoSoDat.CacLopDat.Count; i++)
                {
                    double dodaylopdat = clsBienToanCuc.hoSoDat.CacLopDat[i].DoDay;
                    if (dodaylopdat > 0)
                    {
                        if (chieudaicoc > dodaylopdat)
                        {
                            if (i == 0)
                            {
                                double doancoc = dodaylopdat - Convert.ToDouble(p.txtCaoTrinhDayDai.Text);
                                chieudaicoc = chieudaicoc - doancoc;
                                cacDoanCoc.Add(doancoc);
                            }
                            else
                            {
                                double doancoc = dodaylopdat;
                                chieudaicoc = chieudaicoc - doancoc;
                                cacDoanCoc.Add(doancoc);
                            }
                        }
                        else if (chieudaicoc < dodaylopdat)
                        {
                            double doancoc = chieudaicoc;
                            cacDoanCoc.Add(doancoc);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Độ day lớp đất phải lớn hơn 0");
                    }
                }
            }
            else
            {
                return null;
            }
            return cacDoanCoc;
        }
        #endregion

        #region Hàm nội suy

        static double NoiSuy1Chieu(double x, double x1, double x2, double y1, double y2)
        {
            return y1 + ((x - x1) / (x2 - x1)) * (y2 - y1);
        }

        static double NoiSuy2Chieu(List<clsBangTra_IV_1> traBang, double chieuSau, double iL)
        {
            // Tìm hai giá trị chiều sâu gần nhất
            clsBangTra_IV_1 rowTruoc = null, rowSau = null;
            foreach (var row in traBang)
            {
                if (row.ChieuSau <= chieuSau) rowTruoc = row;
                if (row.ChieuSau > chieuSau)
                {
                    rowSau = row;
                    break;
                }
            }

            if (rowTruoc == null || rowSau == null)
            {
                throw new ArgumentException("Giá trị chiều sâu nằm ngoài phạm vi của bảng tra.");
            }

            // Tính nội suy 1 chiều theo chiều sâu cho giá trị T_i ở hai mức I_L
            double[] tiTruoc = rowTruoc.GiaTri;
            double[] tiSau = rowSau.GiaTri;

            // Tìm hai giá trị I_L liền kề
            int cotTruoc = (int)((iL - 0.2) / 0.1);
            int cotSau = cotTruoc + 1;

            if (cotTruoc < 0 || cotSau >= tiTruoc.Length)
            {
                throw new ArgumentException("Giá trị I_L nằm ngoài phạm vi của bảng tra.");
            }

            double iL1 = 0.2 + cotTruoc * 0.1;
            double iL2 = iL1 + 0.1;

            // Nội suy giá trị theo chiều sâu tại I_L = iL1 và I_L = iL2
            double t1 = NoiSuy1Chieu(chieuSau, rowTruoc.ChieuSau, rowSau.ChieuSau, tiTruoc[cotTruoc], tiSau[cotTruoc]);
            double t2 = NoiSuy1Chieu(chieuSau, rowTruoc.ChieuSau, rowSau.ChieuSau, tiTruoc[cotSau], tiSau[cotSau]);

            // Nội suy theo I_L để tìm giá trị cuối cùng
            return NoiSuy1Chieu(iL, iL1, iL2, t1, t2);
        }
        #endregion

        #region bảng tra To i theo chiều sâu trung bình lớp đất
        List<clsBangTra_IV_1> traBang = new List<clsBangTra_IV_1>
        {
            new clsBangTra_IV_1(1, new double[] { 35, 23, 15, 12, 8, 4, 4, 3, 2 }),
            new clsBangTra_IV_1(2, new double[] { 42, 30, 21, 17, 12, 7, 5, 4, 4 }),
            new clsBangTra_IV_1(3, new double[] { 48, 35, 25, 20, 14, 8, 7, 6, 5 }),
            new clsBangTra_IV_1(4, new double[] { 53, 38, 27, 22, 16, 9, 8, 7, 6 }),
            new clsBangTra_IV_1(5, new double[] { 56, 40, 29, 24, 17, 10, 8, 7, 6 }),
            new clsBangTra_IV_1(6, new double[] { 58, 42, 31, 25, 18, 10, 8, 7, 6 }),
            new clsBangTra_IV_1(8, new double[] { 62, 44, 33, 26, 19, 10, 8, 7, 6 }),
            new clsBangTra_IV_1(10, new double[] { 65, 46, 34, 27, 19, 10, 8, 7, 6 }),
            new clsBangTra_IV_1(15, new double[] { 72, 51, 38, 28, 20, 11, 8, 7, 6 }),
            new clsBangTra_IV_1(20, new double[] { 79, 56, 41, 30, 20, 12, 8, 7, 6 }),
            new clsBangTra_IV_1(25, new double[] { 86, 61, 44, 32, 20, 12, 8, 7, 6 }),
            new clsBangTra_IV_1(30, new double[] { 93, 66, 47, 34, 21, 12, 9, 8, 7 }),
            new clsBangTra_IV_1(35, new double[] { 100, 70, 50, 36, 22, 13, 9, 8, 7 })
        };
        #endregion

        List<clsBangTraIV3_HeSo_Ki_va_Alpha> bangTraIV_3 = new List<clsBangTraIV3_HeSo_Ki_va_Alpha>
        {
            new clsBangTraIV3_HeSo_Ki_va_Alpha("Sét mềm và bùn", 0, 2000, 0.40, 0.50, 30, 30, 15, 15),
            new clsBangTraIV3_HeSo_Ki_va_Alpha("Sét cứng trung bình", 2000, 5000, 0.35, 0.45, 40, 40, 35, 35),
            new clsBangTraIV3_HeSo_Ki_va_Alpha("Sét cứng và rất cứng", 5000, double.MaxValue, 0.45, 0.55, 60, 60, 35, 35),
            new clsBangTraIV3_HeSo_Ki_va_Alpha("Phù sa và cát chảy", 0, 2500, 0.40, 0.50, 120, 80, 35, 35),
            new clsBangTraIV3_HeSo_Ki_va_Alpha("Cát chặt trung bình", 2500, 10000, 0.40, 0.50, 180, 100, 80, 80),
            new clsBangTraIV3_HeSo_Ki_va_Alpha("Cát chặt và rất chặt", 10000, double.MaxValue, 0.30, 0.40, 150, 150, 120, 120)
        };

        public clsCoc TimCocDuocChon(pageTinhToanCoc p)
        {
            foreach (clsCoc coccantim in clsBienToanCuc.ListCoc)
            {
                if (coccantim.Name == p.cbbLoaiCoc.Text)
                {
                    return coccantim;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy cọc yêu cầu");
                    return null;
                }
            }
            return null;
        }
    }
}
