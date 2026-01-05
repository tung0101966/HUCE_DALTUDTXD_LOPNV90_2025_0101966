using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TinhToanCoc.Class;
using TinhToanCoc.Pages;

namespace TinhToanCoc.ViewModels
{
    public class VM_TinhToanDaiCoc : BaseViewModel
    {
        public ICommand btnTinhToan { get; set; }
        public ICommand btnXoa { get; set; }
        public VM_TinhToanDaiCoc()
        {
            btnTinhToan = new RelayCommand<TinhToanDaiCoc>((p) => true, (p) => TinhToan(p));
            btnXoa = new RelayCommand<TinhToanDaiCoc>((p) => true, (p) => Xoa(p));
        }
        private void Xoa(TinhToanDaiCoc p)
        {
            // Reset về mặc định (bạn chỉnh theo ý)
            p.txtChieuDaiDai.Text = "2.0";
            p.txtChieuRongDai.Text = "1.5";
            p.txtChieuCaoDai.Text = "0.8";
            p.txtSoLuongCoc.Text = "4";

            p.txtDuongKinhCoc.Text = "300";
            p.txtChieuDaiCoc.Text = "15";
            p.txtKhoangCachCoc.Text = "0.9";
            p.txtSucChiuTaiCoc.Text = "800";

            p.txtTaiTrongDoc.Text = "2500";
            p.txtMomentX.Text = "150";
            p.txtMomentY.Text = "120";
            p.txtLucCat.Text = "80";

            p.txtCuongDoThep.Text = "365";
            p.txtHeSoAnToan.Text = "1.15";

            p.txtKetQuaChiuTai.Text = "Sức chịu tải: Đang tính...";
            p.txtKetQuaKiemTra.Text = "Kiểm tra ổn định: Đang tính...";
            p.txtThepYeuCau.Text = "Thép yêu cầu: Đang tính...";
            p.txtKienNghi.Text = "Kiến nghị: Đang tính...";

            RedrawLayoutOnly(p);
        }
        private void RedrawLayoutOnly(TinhToanDaiCoc p)
        {
            try
            {
                int n = int.TryParse(p.txtSoLuongCoc.Text, out var nn) ? nn : 4;
                double S = double.TryParse(p.txtKhoangCachCoc.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var ss) ? ss : 0.9;

                var piles = GeneratePileCoordinates(n, S);
                DrawPileLayout(p.canvasSoDoCoc, piles);

                p.canvasBieuDo.Children.Clear();
                p.canvasSoDoCoc.Children.Clear();
            }
            catch
            {
                // ignore
            }
        }
        public void TinhToan(TinhToanDaiCoc p)
        {
            try
            {
                // ---- Đọc input (đơn vị: m, kN, kNm, mm, MPa)
                double L = ReadDouble(p.txtChieuDaiDai, "Chiều dài đài L");
                double B = ReadDouble(p.txtChieuRongDai, "Chiều rộng đài B");
                double H = ReadDouble(p.txtChieuCaoDai, "Chiều cao đài H");
                int n = ReadInt(p.txtSoLuongCoc, "Số lượng cọc");

                double Dmm = ReadDouble(p.txtDuongKinhCoc, "Đường kính cọc D (mm)");
                double Lc = ReadDouble(p.txtChieuDaiCoc, "Chiều dài cọc Lc");
                double S = ReadDouble(p.txtKhoangCachCoc, "Khoảng cách cọc S");
                double Pc = ReadDouble(p.txtSucChiuTaiCoc, "Sức chịu tải cọc Pc");

                double N = ReadDouble(p.txtTaiTrongDoc, "Tải trọng đứng N");
                double Mx = ReadDouble(p.txtMomentX, "Moment X Mx");
                double My = ReadDouble(p.txtMomentY, "Moment Y My");
                double Q = ReadDouble(p.txtLucCat, "Lực cắt Q");

                double Rs = ReadDouble(p.txtCuongDoThep, "Cường độ thép Rs (MPa)");
                double gamma = ReadDouble(p.txtHeSoAnToan, "Hệ số an toàn γ");

                // ---- Tạo bố trí cọc (tự suy ra lưới gần vuông từ n & khoảng cách S)
                var piles = GeneratePileCoordinates(n, S);

                // ---- Tính phản lực từng cọc theo N, Mx, My (đài cứng)
                // Pi = N/n + Mx*yi/Σ(yi^2) + My*xi/Σ(xi^2)
                var reactions = ComputePileReactions(N, Mx, My, piles);

                double pMax = reactions.Max(r => r.P);
                double pMin = reactions.Min(r => r.P);

                double Pc_allow = Pc / gamma;

                // ---- Kết quả sức chịu tải (đơn giản: check phản lực max)
                bool okCapacity = pMax <= Pc_allow;
                bool uplift = pMin < 0;

                p.txtKetQuaChiuTai.Text =
                    $"Sức chịu tải: Pmax = {pMax:F2} kN; Pc/γ = {Pc_allow:F2} kN → {(okCapacity ? "ĐẠT" : "KHÔNG ĐẠT")}";

                // ---- Kiểm tra ổn định / cảnh báo nhổ cọc
                p.txtKetQuaKiemTra.Text =
                    uplift
                        ? $"Kiểm tra ổn định: Có phản lực âm (Pmin = {pMin:F2} kN) → NGUY CƠ NHỔ CỌC"
                        : $"Kiểm tra ổn định: Pmin = {pMin:F2} kN (≥ 0) → OK";

                // ---- Ước tính thép yêu cầu (demo)
                // Ý tưởng nhanh: tính moment biên theo phân phối phản lực:
                // Mx_cap ≈ Σ(Pi*yi), My_cap ≈ Σ(Pi*xi) (thực ra sẽ gần về Mx,My)
                // rồi lấy thép theo M = As*Rs*z ; z ~ 0.9*d (d ~ H - cover)
                double cover = 0.07; // 70mm giả định (bạn thay theo input nếu muốn)
                double d = Math.Max(0.05, H - cover); // m
                double z = 0.9 * d;                  // m

                // Lấy moment thiết kế theo phương lớn hơn (bạn có thể tách thép X/Y)
                double Mdesign = Math.Max(Math.Abs(Mx), Math.Abs(My)); // kNm

                // đổi kNm -> Nmm để dùng Rs(MPa = N/mm2)
                // 1 kNm = 1e6 Nmm
                // z(m) -> mm
                double z_mm = z * 1000.0;
                double As_mm2 = (Mdesign * 1e6) / (Rs * z_mm); // mm2 (very simplified)

                // Thép theo 1m bề rộng (demo): nếu bạn muốn chia theo B hoặc L thì chỉnh lại
                p.txtThepYeuCau.Text =
                    $"Thép yêu cầu (ước tính): As ≈ {As_mm2:F0} mm² (theo M = {Mdesign:F1} kNm, d≈{d:F2} m)";

                // ---- Kiến nghị
                p.txtKienNghi.Text = BuildRecommendation(okCapacity, uplift, pMax, Pc_allow, n, S, L, B, Q);

                // ---- Vẽ sơ đồ
                DrawPileLayout(p.canvasSoDoCoc, piles);
                DrawReactionDiagram(p.canvasBieuDo, piles, reactions);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi dữ liệu / tính toán", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private double ReadDouble(TextBox tb, string name)
        {
            if (tb == null) throw new Exception($"{name}: control null.");

            string s = (tb.Text ?? "").Trim().Replace(",", ".");
            if (!double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double v))
                throw new Exception($"{name}: giá trị không hợp lệ.");

            return v;
        }
        private int ReadInt(TextBox tb, string name)
        {
            if (tb == null) throw new Exception($"{name}: control null.");

            string s = (tb.Text ?? "").Trim();
            if (!int.TryParse(s, out int v))
                throw new Exception($"{name}: giá trị không hợp lệ.");

            return v;
        }
        private List<(double X, double Y)> GeneratePileCoordinates(int n, double spacing)
        {
            if (n <= 0) throw new Exception("Số lượng cọc phải > 0.");
            if (spacing <= 0) throw new Exception("Khoảng cách cọc S phải > 0.");

            // Chọn (rows, cols) gần vuông nhất
            int cols = (int)Math.Ceiling(Math.Sqrt(n));
            int rows = (int)Math.Ceiling((double)n / cols);

            var coords = new List<(double X, double Y)>(n);

            // Tạo lưới (0..cols-1, 0..rows-1) rồi dịch về tâm (0,0)
            // thứ tự fill theo hàng
            int idx = 0;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (idx >= n) break;

                    coords.Add((c * spacing, r * spacing));
                    idx++;
                }
            }

            // Dịch về tâm
            double xAvg = coords.Average(p => p.X);
            double yAvg = coords.Average(p => p.Y);

            for (int i = 0; i < coords.Count; i++)
                coords[i] = (coords[i].X - xAvg, coords[i].Y - yAvg);

            return coords;
        }
        private List<(double X, double Y, double P)> ComputePileReactions(
            double N, double Mx, double My, List<(double X, double Y)> piles)
        {
            int n = piles.Count;

            double sumX2 = piles.Sum(p => p.X * p.X);
            double sumY2 = piles.Sum(p => p.Y * p.Y);

            // Tránh chia 0 khi 1 hàng cọc
            if (sumX2 < 1e-12) sumX2 = 1e-12;
            if (sumY2 < 1e-12) sumY2 = 1e-12;

            var result = new List<(double X, double Y, double P)>(n);

            foreach (var p in piles)
            {
                double P = (N / n)
                           + (Mx * p.Y / sumY2)
                           + (My * p.X / sumX2);

                result.Add((p.X, p.Y, P));
            }

            return result;
        }
        private string BuildRecommendation(bool okCapacity, bool uplift, double pMax, double pcAllow, int n, double s, double L, double B, double Q)
        {
            var lines = new List<string>();

            if (!okCapacity)
                lines.Add($"• Tăng số cọc (hiện {n}) hoặc tăng Pc (cọc khỏe hơn) / giảm γ. (Hiện Pmax={pMax:F0} > Pc/γ={pcAllow:F0})");

            if (uplift)
                lines.Add("• Đang có nhổ cọc: cân nhắc tăng kích thước đài, tăng khoảng cách cọc hợp lý, tăng số cọc, hoặc bố trí cọc đối xứng hơn.");

            // gợi ý khoảng cách cọc thường (tham khảo kinh nghiệm): S >= 2.5D~3D
            // D nhập mm, nên ở đây chỉ nhắc logic; bạn có thể nâng cấp kiểm tra theo D.
            lines.Add($"• Kiểm tra cấu tạo: đảm bảo kích thước đài (L={L:F2}m, B={B:F2}m) đủ bao cọc + cover + neo thép.");

            if (Math.Abs(Q) > 0.0)
                lines.Add("• Có lực cắt Q: nên kiểm tra cắt – chọc thủng (punching) quanh cột/đài theo tiêu chuẩn bạn dùng.");

            if (lines.Count == 0)
                lines.Add("• Các kiểm tra cơ bản đang ổn. Bước tiếp theo: tính thép chi tiết theo dải X/Y + kiểm tra punching/shear theo tiêu chuẩn.");

            return "Kiến nghị:\n" + string.Join("\n", lines);
        }
        private void DrawPileLayout(Canvas canvas, List<(double X, double Y)> piles)
        {
            canvas.Children.Clear();

            double w = canvas.ActualWidth > 10 ? canvas.ActualWidth : 360;
            double h = canvas.ActualHeight > 10 ? canvas.ActualHeight : 200;

            // Khung
            var border = new Rectangle
            {
                Width = w - 10,
                Height = h - 10,
                Stroke = Brushes.LightGray,
                StrokeThickness = 1
            };
            Canvas.SetLeft(border, 5);
            Canvas.SetTop(border, 5);
            canvas.Children.Add(border);

            // scale theo bbox
            double minX = piles.Min(p => p.X);
            double maxX = piles.Max(p => p.X);
            double minY = piles.Min(p => p.Y);
            double maxY = piles.Max(p => p.Y);

            double spanX = Math.Max(1e-6, maxX - minX);
            double spanY = Math.Max(1e-6, maxY - minY);

            double margin = 30;
            double sx = (w - 2 * margin) / spanX;
            double sy = (h - 2 * margin) / spanY;
            double s = Math.Min(sx, sy);

            Point Map(double X, double Y)
            {
                double px = margin + (X - minX) * s;
                double py = margin + (maxY - Y) * s; // đảo trục Y cho đẹp
                return new Point(px, py);
            }

            // Vẽ tâm
            var center = new Ellipse { Width = 6, Height = 6, Fill = Brushes.Black };
            Canvas.SetLeft(center, w / 2 - 3);
            Canvas.SetTop(center, h / 2 - 3);
            canvas.Children.Add(center);

            // Vẽ cọc
            for (int i = 0; i < piles.Count; i++)
            {
                var p = piles[i];
                var pt = Map(p.X, p.Y);

                var e = new Ellipse
                {
                    Width = 18,
                    Height = 18,
                    Stroke = Brushes.DimGray,
                    StrokeThickness = 2,
                    Fill = Brushes.White
                };
                Canvas.SetLeft(e, pt.X - 9);
                Canvas.SetTop(e, pt.Y - 9);
                canvas.Children.Add(e);

                var t = new TextBlock
                {
                    Text = (i + 1).ToString(),
                    FontSize = 11,
                    FontWeight = FontWeights.SemiBold
                };
                Canvas.SetLeft(t, pt.X - 4);
                Canvas.SetTop(t, pt.Y - 8);
                canvas.Children.Add(t);
            }
        }

        // =========================
        // Vẽ biểu đồ phản lực cọc (cột)
        // =========================
        private void DrawReactionDiagram(Canvas canvas, List<(double X, double Y)> piles, List<(double X, double Y, double P)> reactions)
        {
            canvas.Children.Clear();

            double w = canvas.ActualWidth > 10 ? canvas.ActualWidth : 360;
            double h = canvas.ActualHeight > 10 ? canvas.ActualHeight : 150;

            // trục nền
            var baseLine = new Line
            {
                X1 = 10,
                X2 = w - 10,
                Y1 = h - 25,
                Y2 = h - 25,
                Stroke = Brushes.Gray,
                StrokeThickness = 1
            };
            canvas.Children.Add(baseLine);

            double pMax = reactions.Max(r => r.P);
            double pMin = reactions.Min(r => r.P);
            double absMax = Math.Max(Math.Abs(pMax), Math.Abs(pMin));
            absMax = Math.Max(absMax, 1e-6);

            int n = reactions.Count;
            double barW = (w - 20) / Math.Max(1, n);
            double x = 10;

            for (int i = 0; i < n; i++)
            {
                double p = reactions[i].P;
                double barH = (h - 50) * (Math.Abs(p) / absMax);

                // bar đi lên nếu p > 0, đi xuống nếu p < 0 (nhổ cọc)
                double y0 = h - 25;
                double yTop = p >= 0 ? (y0 - barH) : y0;
                double height = barH;

                var rect = new Rectangle
                {
                    Width = Math.Max(6, barW * 0.65),
                    Height = height,
                    Fill = (p >= 0) ? Brushes.SteelBlue : Brushes.IndianRed
                };

                Canvas.SetLeft(rect, x + barW * 0.175);
                Canvas.SetTop(rect, yTop);
                canvas.Children.Add(rect);

                var label = new TextBlock
                {
                    Text = $"{i + 1}",
                    FontSize = 10
                };
                Canvas.SetLeft(label, x + barW * 0.35);
                Canvas.SetTop(label, h - 20);
                canvas.Children.Add(label);

                var val = new TextBlock
                {
                    Text = $"{p:F0}",
                    FontSize = 10
                };
                Canvas.SetLeft(val, x + barW * 0.18);
                Canvas.SetTop(val, p >= 0 ? (yTop - 16) : (yTop + height + 2));
                canvas.Children.Add(val);

                x += barW;
            }

            var note = new TextBlock
            {
                Text = $"Pmax={pMax:F1} kN, Pmin={pMin:F1} kN",
                FontSize = 11,
                Foreground = Brushes.DimGray
            };
            Canvas.SetLeft(note, 10);
            Canvas.SetTop(note, 5);
            canvas.Children.Add(note);
        }
    }
}
